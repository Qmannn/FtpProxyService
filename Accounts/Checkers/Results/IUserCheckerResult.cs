using System.Net;

namespace Accounts.Checkers.Results
{
    public interface IUserCheckerResult
    {
        /// <summary>
        /// Точка подключения к удаленному серверу
        /// </summary>
        EndPoint ServerEndPoint { get; }

        /// <summary>
        /// Обозначает существование пользователя
        /// </summary>
        bool IsExist { get; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        string Login { get; }
    }
}