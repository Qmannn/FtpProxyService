using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using UsersLib.Configuration;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace UsersLib.Secure.ActiveDirectory
{
    public class UsersUpdater : IUsersUpdater
    {
        public int Update()
        {
            List<User> users = GetUsersFromDomainGroup();
            IDbUserController dbUserController = new DbUserController();
            return dbUserController.UpdateUsers( users );
        }

        private List<User> GetUsersFromDomainGroup()
        {
            List<User> users = new List<User>();
            using ( var context = new PrincipalContext( 
                ContextType.Domain, 
                Config.LdapDomain, 
                Config.LdapServiceAccount, 
                Config.LdapServicePassword ) )
            {
                using ( var group = GroupPrincipal.FindByIdentity( context, Config.LdapGroupName ) )
                {
                    if ( group == null )
                    {
                        return users;
                    }
                    var adUsers = group.GetMembers( true );
                    foreach ( var principal in adUsers )
                    {
                        UserPrincipal user = principal as UserPrincipal;
                        if ( user == null )
                        {
                            continue;
                        }
                        users.Add( new User
                        {
                            DisplayName = user.DisplayName,
                            Login = user.SamAccountName
                        } );
                    }
                }
            }

            return users;
        }
    }
}