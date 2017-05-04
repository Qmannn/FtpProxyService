using System;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public interface IDbAuthController
    {
        UserRole GetUserRole( string login );

        DateTime? GetAccessTime( string login, string password );

        void SetAccessTime( string login, string password );
    }
}