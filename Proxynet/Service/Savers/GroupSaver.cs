using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Service.Savers
{
    public class GroupSaver : IGroupSaver
    {
        private readonly IDbGroupController _dbGroupController;
        private readonly IGroupDtoConverter _groupDtoConverter;

        public GroupSaver(IDbGroupController dbGroupController, IGroupDtoConverter groupDtoConverter)
        {
            _dbGroupController = dbGroupController;
            _groupDtoConverter = groupDtoConverter;
        }

        public GroupDto SaveGroup(GroupDto groupDto)
        {
            Group group = _groupDtoConverter.Convert(groupDto);
            group = _dbGroupController.SaveGroup(group);
            return _groupDtoConverter.Convert(group);
        }
    }
}