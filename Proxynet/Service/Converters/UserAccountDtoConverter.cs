using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public class UserAccountDtoConverter : IUserAccountDtoConverter
    {
        public UserAccountDto Convert(UserAccount userAccount)
        {
            return new UserAccountDto
            {
                Login = userAccount.Login
            };
        }

        public UserAccount Convert(UserAccountDto userAccountDto)
        {
            return new UserAccount
            {
                Login = userAccountDto.Login,
                Password = userAccountDto.Password
            };
        }
    }
}