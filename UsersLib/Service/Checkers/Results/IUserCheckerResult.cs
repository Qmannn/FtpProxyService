namespace UsersLib.Service.Checkers.Results
{
    public interface IUserCheckerResult
    {
        /// <summary>
        /// Адрес подключения к удаленному серверу
        /// </summary>
        string UrlAddress { get; }

        /// <summary>
        /// Порт подключения к удаленному серверу
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Логин подключения к удаленному серверу
        /// </summary>
        string Login { get; }

        /// <summary>
        /// Пароль подключения к удаленному серверу
        /// </summary>
        string Pass { get; }
    }
}