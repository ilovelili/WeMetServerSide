using System;
using System.IO;
using System.Threading;
using AbbyyOnlineSdk;
using NamecardScanner.Core;
using NamecardScanner.Models;
using NamecardScanner.Modules.Abstract;
using Nancy;
using Nancy.Helpers;
using Newtonsoft.Json;

namespace NamecardScanner.Modules.NameCard
{
    public class Namecard : BaseModule
    {
        public Namecard()
        {
            var tempFile = Path.Combine(Config.Config.TempFileDir, Guid.NewGuid() + ".tmp");
            File.Create(tempFile);

            Post["Recognize"] = _ =>
            {
                var stream = this.Request.Body;
                RecognizeRequest req;

                using (var streamreader = new StreamReader(stream))
                {
                    req = JsonConvert.DeserializeObject<RecognizeRequest>(HttpUtility.UrlDecode(streamreader.ReadToEnd()));
                }

                var task = new NamecardRecognizer().Recognize(req.Data, tempFile);

                // polling task status sync
                if (PollingTaskStatus(task) != TaskStatus.Completed.ToString())
                {
                    return this.Response.AsJson(new RecognizeResponse()
                    {
                        TaskStatus = task.TaskStatus,
                    });
                }

                var response = RecognizeFieldParser.ParseRecognizeResponse(tempFile);
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }

                response.TaskStatus = TaskStatus.Completed.ToString();
                return this.Response.AsJson(response);
            };

            // Just for testing
            Post["RecognizeMock"] = _ =>
            {
                var response = new RecognizeResponse()
                {
                    Name = "Jane Doe",
                    Address = "Awesome Street",
                    Company = "Awesome Company",
                    Email = "JaneDoe@awesome.com",
                    Job = "Web designer",
                    Phone = "012345678",
                    Web = "http://wwww.awesome.com",
                    Text = "This is mock data",
                    TaskStatus = TaskStatus.Completed.ToString(),
                };

                return this.Response.AsJson(response);
            };
        }

        private static string PollingTaskStatus(UserTask task)
        {
            const int maxPollingCount = 30;
            for (var i = 0; i < maxPollingCount; i++)
            {
                // SimpleLogger.WriteFileLog($"Status is {task.TaskStatus} at {i + 1}th loop");

                if (task.TaskStatus == TaskStatus.Completed.ToString())
                {
                    return TaskStatus.Completed.ToString();
                }

                Thread.Sleep(1000);
            }

            return task.TaskStatus;
        }
    }
}