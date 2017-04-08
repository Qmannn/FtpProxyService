using System.Collections.Generic;
using UsersLib.DbEntity;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public interface IDbUserController
    {
        List<User> GetUsers();
        User GetUser( int userId );
        User GetUser( string userLogin );
        List<UserGroup> GetUserGroups( string userLogin );
        List<UserGroup> GetUserGroups( int userId );
        void SaveUser( User user );
    }
}