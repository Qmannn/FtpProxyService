using System.Collections.Generic;
using Proxynet.Models;

namespace Proxynet.Service.Finders
{
    public interface IGroupDataFinder
    {
        List<GroupDto> GetGroupsDto();
    }
}