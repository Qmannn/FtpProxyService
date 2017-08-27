using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using UsersLib.Configuration;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace UsersLib.Service.ActiveDirectory
{
    public class UsersUpdater : IUsersUpdater
    {
        private readonly IDbUserController _dbUserController;

        public UsersUpdater(IDbUserController dbUserController)
        {
            _dbUserController = dbUserController;
        }

        public int Update()
        {
            List<User> users = GetUsersFromDomainGroup();
            return _dbUserController.UpdateUsers(users);
        }

        private List<User> GetUsersFromDomainGroup()
        {
            List<User> users = new List<User>();
            using (var context = new PrincipalContext(
                ContextType.Domain,
                Config.LdapDomain,
                Config.LdapServiceAccount,
                Config.LdapServicePassword))
            {
                using (var group = GroupPrincipal.FindByIdentity(context, Config.LdapGroupName))
                {
                    if (group == null)
                    {
                        return users;
                    }
                    var adUsers = group.GetMembers(true);
                    List<UserPrincipal> userPrincipals =
                        adUsers.Select(item => item as UserPrincipal).Where(item => item != null).ToList();
                    users =
                        userPrincipals.ConvertAll(
                            item => new User {DisplayName = item.DisplayName, Login = item.SamAccountName});
                }
            }
            return users;
        }
    }
}