using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public interface IGroupDtoConverter
    {
        GroupDto Convert( Group userGroup );
        List<GroupDto> Convert( List<Group> groups );

        Group Convert( GroupDto group );
        List<Group> Convert( List<GroupDto> group );
    }
}