using System.Collections.Generic;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public interface IDbUserController
    {
        List<User> GetUsers();
        User GetUser( int userId );
        User GetUser( string userLogin );
        Dictionary<User, List<Group>> GetUsersByGroups();
        List<Group> GetUserGroups( string userLogin );
        List<Group> GetUserGroups( int userId );
        void SaveUser( User user );
        void SaveUserGroups( int userId, List<int> userGroupIds );
        int UpdateUsers( List<User> users );
    }
}