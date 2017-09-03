using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public interface IUserAccountDtoConverter
    {
        UserAccount Convert(UserAccountDto userAccountDto);
        UserAccountDto Convert(UserAccount userAccount);
    }
}