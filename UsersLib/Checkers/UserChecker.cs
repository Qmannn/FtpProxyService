using System;
using UsersLib.Checkers.Results;

namespace UsersLib.Checkers
{
    internal class UserChecker : IUserChecker
    {
        private readonly IUserChecker _checkerStrategy;

        /// <summary>
        /// Конструктор для конкретной реализации проверки
        /// </summary>
        public UserChecker()
        {
            _checkerStrategy = null;
        }

        /// <summary>
        /// Конструктор для использования реализации проверки
        /// </summary>
        /// <param name="checkerSreategy"></param>
        public UserChecker( IUserChecker checkerSreategy )
        {
            _checkerStrategy = checkerSreategy;
        }

        public IUserCheckerResult Check( string userLogin, string userPass, string serverIdentify )
        {
            if ( _checkerStrategy == null )
            {
                throw new NotImplementedException( "checkerStrategy not implemented" );
            }
            return _checkerStrategy.Check( userLogin, userPass, serverIdentify );
        }
    }
}