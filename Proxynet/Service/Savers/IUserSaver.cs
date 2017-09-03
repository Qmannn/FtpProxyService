using Proxynet.Models;

namespace Proxynet.Service.Savers
{
    public interface IUserSaver
    {
        int SaveUser(UserDto userDto);
    }
}