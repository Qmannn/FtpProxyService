using System.Collections.Generic;
using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Service.Finders
{
    public class GroupDataFinder : IGroupDataFinder
    {
        private readonly IDbGroupController _dbGroupController;
        private readonly IGroupDtoConverter _groupDtoConverter;

        public GroupDataFinder(IDbGroupController dbGroupController, IGroupDtoConverter groupDtoConverter)
        {
            _dbGroupController = dbGroupController;
            _groupDtoConverter = groupDtoConverter;
        }

        public List<GroupDto> GetGroupsDto()
        {
            List<Group> groups = _dbGroupController.GetGroups();
            return _groupDtoConverter.Convert(groups);
        }
    }
}