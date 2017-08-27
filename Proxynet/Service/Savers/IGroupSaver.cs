using Proxynet.Models;

namespace Proxynet.Service.Savers
{
    public interface IGroupSaver
    {
        GroupDto SaveGroup(GroupDto groupDto);
    }
}