using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public class UserGroupDtoConverter : IUserGroupDtoConverter
    {
        public UserGroupDto Convert( UserGroup userGroup )
        {
            return new UserGroupDto
            {
                Id = userGroup.Id,
                Name = userGroup.Name
            };
        }

        public List<UserGroupDto> Convert( List<UserGroup> userGroups )
        {
            return userGroups.ConvertAll( Convert );
        }

        public UserGroup Convert( UserGroupDto userGroup )
        {
            return new UserGroup
            {
                Id = userGroup.Id,
                Name = userGroup.Name
            };
        }

        public List<UserGroup> Convert( List<UserGroupDto> userGroup )
        {
            return userGroup.ConvertAll( Convert );
        }
    }
}