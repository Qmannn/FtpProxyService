using System.Configuration;

namespace UsersLib.Configuration
{
    public static class Config
    {
        /// <summary>
        /// Пароль для шифрования/дешифрования паролей и логинов аккаунтов сайтов
        /// </summary>
        public static string MasterWord => ConfigurationManager.AppSettings["MasterWord"];

    }
}