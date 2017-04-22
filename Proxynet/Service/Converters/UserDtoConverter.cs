using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public class UserDtoConverter : IUserDtoConverter
    {
        public List<UserDto> Convert( List<User> users )
        {
            return users.ConvertAll( Convert );
        }

        public UserDto Convert( User user )
        {
            return new UserDto
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.DisplayName
            };
        }
    }
}