using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public interface IUserGroupDtoConverter
    {
        UserGroupDto Convert( UserGroup userGroup );
        List<UserGroupDto> Convert( List<UserGroup> userGroup );

        UserGroup Convert( UserGroupDto userGroup );
        List<UserGroup> Convert( List<UserGroupDto> userGroup );
    }
}