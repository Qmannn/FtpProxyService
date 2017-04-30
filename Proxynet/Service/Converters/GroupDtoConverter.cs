using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public class GroupDtoConverter : IGroupDtoConverter
    {
        public GroupDto Convert( Group userGroup )
        {
            return new GroupDto
            {
                Id = userGroup.Id,
                Name = userGroup.Name
            };
        }

        public List<GroupDto> Convert( List<Group> groups )
        {
            return groups.ConvertAll( Convert );
        }

        public Group Convert( GroupDto userGroup )
        {
            return new Group
            {
                Id = userGroup.Id,
                Name = userGroup.Name
            };
        }

        public List<Group> Convert( List<GroupDto> userGroup )
        {
            return userGroup.ConvertAll( Convert );
        }
    }
}