using System;
using System.ComponentModel;

namespace NamecardScanner.Core
{
    public sealed class UserTask : INotifyPropertyChanged
    {
        private const string UnknownTask = "<unknown>";
        private const string InitStatus = "<initializing>";


        public UserTask()
        {
            TaskId = UnknownTask;
            TaskStatus = InitStatus;
        }

        public string TaskId
        {
            get
            {
                return _taskId;
            }
            set
            {
                _taskId = value;
                NotifyPropertyChanged("TaskId");
            }
        }

        public string TaskStatus
        {
            get
            {
                return _taskStatus;
            }
            set
            {
                _taskStatus = value;
                NotifyPropertyChanged("TaskStatus");
            }
        }

        public string OutputFilePath
        {
            get
            {
                return _outputFilePath;
            }
            set
            {
                _outputFilePath = value;
                NotifyPropertyChanged("OutputFilePath");
            }
        }

        public int PagesCount
        {
            get { return _pagesCount; }
            set { _pagesCount = value; NotifyPropertyChanged("PagesCount"); }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; NotifyPropertyChanged("Description"); }
        }

        public DateTime RegistrationTime
        {
            get { return _registrationTime; }
            set
            {
                _registrationTime = value;
                NotifyPropertyChanged("RegistrationTime");
            }
        }

        public DateTime StatusChangeTime
        {
            get { return _statusChangeTime; }
            set
            {
                _statusChangeTime = value;
                NotifyPropertyChanged("StatusChangeTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        public bool IsFieldLevel
        {
            get;
            set;
        }

        public string RecognizedText
        {
            get { return _recognizedText; }
            set { _recognizedText = value; NotifyPropertyChanged("RecognizedText"); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; NotifyPropertyChanged("ErrorMessage"); }
        }

        private string _taskId;
        private string _taskStatus;

        private int _pagesCount;
        private string _description;

        private string _outputFilePath;

        private DateTime _registrationTime;
        private DateTime _statusChangeTime;

        private string _recognizedText = null;
        private string _errorMessage;
    }
}
