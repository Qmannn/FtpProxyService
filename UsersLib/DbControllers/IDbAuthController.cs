using System;
using System.Collections.Generic;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public interface IDbAuthController
    {
        List<UserRoleKind> GetUserRoles(int userId);
        UserAccount GetUserAccount(string login, string password);
        UserAccount SaveUserAccount(UserAccount userAccount);
        int GetUserId(string login);
        UserAccount GetUserAccount(int userId);
    }
}