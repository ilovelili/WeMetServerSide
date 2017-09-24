using System;
using System.Collections.ObjectModel;
using AbbyyOnlineSdk;

namespace NamecardScanner.Core
{
    public sealed class NamecardRecognizer
    {
        private readonly RestServiceClient _restClient = new RestServiceClient
        {
            ApplicationId = Config.Config.ApplicationId,
            Password = Config.Config.Password
        };

        private readonly RestServiceClientAsync _restClientAsync;
        private readonly BusCardProcessingSettings _settings;

        private static BusCardProcessingSettings GetBusCardProcessingSettings()
        {
            return new BusCardProcessingSettings
            {
                Language = Config.Config.Languages,
                OutputFormat = BusCardProcessingSettings.OutputFormatEnum.Xml,
            };
        }

        public NamecardRecognizer()
        {
            this._restClientAsync = new RestServiceClientAsync(this._restClient);
            this._restClientAsync.UploadFileCompleted += UploadCompleted;
            this._restClientAsync.TaskProcessingCompleted += ProcessingCompleted;
            this._restClientAsync.DownloadFileCompleted += DownloadCompleted;

            this._settings = GetBusCardProcessingSettings();
        }

        public UserTask Recognize(string base64Image, string outputFilePath)
        {
            var imageData = Convert.FromBase64String(base64Image);

            var task = new UserTask()
            {
                TaskStatus = "Uploading",
                OutputFilePath = outputFilePath,
            };

            UserTasks.Add(task);
            this._restClientAsync.ProcessBusinessCardAsync(imageData, this._settings, task);

            return task;
        }

        #region Async client callbacks
        private static void UploadCompleted(object sender, UploadCompletedEventArgs e)
        {
            var task = e.UserState as UserTask;
            if (task == null) return;

            task.TaskStatus = "Processing";
            task.TaskId = e.Result.Id.ToString();
        }

        private void ProcessingCompleted(object sender, TaskEventArgs e)
        {
            var task = e.UserState as UserTask;

            if (e.Error != null)
            {
                if (task == null) return;

                task.TaskStatus = "Processing error";
                task.OutputFilePath = "<error>";
                task.ErrorMessage = e.Error.Message;
                if (task.IsFieldLevel)
                {
                    // ErrorMessage is not mapped into a column for
                    // field level tasks
                    task.RecognizedText = $"<{task.ErrorMessage}>";
                }

                MoveTaskToCompleted(task);
                return;
            }

            switch (e.Result.Status)
            {
                case TaskStatus.NotEnoughCredits:
                    if (task == null) return;
                    task.TaskStatus = "Not enough credits";
                    task.OutputFilePath = "<error>";
                    task.ErrorMessage = e.Result.Error;
                    MoveTaskToCompleted(task);
                    return;

                case TaskStatus.ProcessingFailed:
                    if (task == null) return;

                    task.TaskStatus = "Internal server error";
                    task.OutputFilePath = "<error>";
                    task.ErrorMessage = e.Result.Error;
                    MoveTaskToCompleted(task);
                    return;
            }

            if (e.Result.Status != TaskStatus.Completed)
            {
                if (task == null) return;

                task.TaskStatus = task.ErrorMessage = e.Result.Status.ToString();
                task.OutputFilePath = "<error>";
                MoveTaskToCompleted(task);
                return;
            }

            if (task == null) return;
            task.TaskStatus = "Downloading";
            
            // Start downloading
            this._restClientAsync.DownloadFileAsync(e.Result, task.OutputFilePath, task);
        }

        private void DownloadCompleted(object sender, TaskEventArgs e)
        {
            var task = e.UserState as UserTask;
            if (e.Error != null)
            {
                if (task == null) return;

                task.TaskStatus = "Downloading error";
                task.OutputFilePath = "<error>";
                task.ErrorMessage = e.Error.Message;
                MoveTaskToCompleted(task);
                return;
            }

            if (task == null) return;
            task.TaskStatus = "Ready";

            MoveTaskToCompleted(task);
        }
        #endregion

        #region tasks
        // Not completed tasks in left list

        // Completed and failed tasks in right list 

        // List of tasks on server

        // List of field-level tasks

        public ObservableCollection<UserTask> UserTasks { get; } = new ObservableCollection<UserTask>();

        public ObservableCollection<UserTask> CompletedTasks { get; } = new ObservableCollection<UserTask>();

        public ObservableCollection<UserTask> ServerTasks { get; } = new ObservableCollection<UserTask>();

        public ObservableCollection<UserTask> FieldLevelTasks { get; } = new ObservableCollection<UserTask>();

        // Move task from _userTasks to _completedTasks
        private void MoveTaskToCompleted(UserTask task)
        {
            task.TaskStatus = TaskStatus.Completed.ToString();
            UserTasks.Remove(task);
            CompletedTasks.Insert(0, task);
        }
        #endregion
    }
}
