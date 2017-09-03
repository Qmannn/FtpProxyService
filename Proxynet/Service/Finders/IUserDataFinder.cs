using System.Collections.Generic;
using Proxynet.Models;

namespace Proxynet.Service.Finders
{
    public interface IUserDataFinder
    {
        UserDto GetUser(int userId);
        List<UserDto> GetUsersWithGroups();
    }
}