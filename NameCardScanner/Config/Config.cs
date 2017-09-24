using System.Configuration;

namespace NamecardScanner.Config
{
    public static class Config
    {
        public static string ApplicationId = GetAppConfig("ApplicationId");
        public static string Password = GetAppConfig("Password");
        public static string Languages = GetAppConfig("Languages");

        private static string GetAppConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
