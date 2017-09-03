using System.Collections.Generic;
using UsersLib.Entity;

namespace UsersLib.Service.Savers
{
    public interface IUserSaver
    {
        User SaveUser(User user);
        User SaveUser(User user, UserAccount userAccount);
        void SaveUserGroups(int userId, List<int> grupIds);
    }
}