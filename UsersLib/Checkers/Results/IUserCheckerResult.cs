using System.Net;

namespace UsersLib.Checkers.Results
{
    public interface IUserCheckerResult
    {
        /// <summary>
        /// Точка подключения к удаленному серверу
        /// </summary>
        IPEndPoint ServerEndPoint { get; }
        
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