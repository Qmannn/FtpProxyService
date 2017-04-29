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
        Dictionary<User, List<UserGroup>> GetUsersByGroups(); 
        List<UserGroup> GetUserGroups( string userLogin );
        List<UserGroup> GetUserGroups( int userId );
        List<UserGroup> GetUserGroups();
        void SaveUser( User user );
        void SaveUserGroups( int userId, List<int> userGroupIds);
    }
}