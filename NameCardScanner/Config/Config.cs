using System.Configuration;
using System.IO;

namespace NamecardScanner.Config
{
    public static class Config
    {
        public static string ApplicationId = GetAppConfig("ApplicationId");
        public static string Password = GetAppConfig("Password");
        public static string Languages = GetAppConfig("Languages");
        public static string LogOutput = GetAppConfig("LogOutput");

        public static string TempFileDir 
        {
            get
            {
                var tempFileDir = GetAppConfig("tempFileDir");
                if (Directory.Exists(tempFileDir))
                {
                    Directory.CreateDirectory(tempFileDir);
                }

                return tempFileDir;
            }
        }

        private static string GetAppConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
