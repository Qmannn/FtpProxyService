using System.Collections.Generic;
using Proxynet.Models;
using UsersLib.Entity;

namespace Proxynet.Service.Converters
{
    public class UserDtoConverter : IUserDtoConverter
    {
        private readonly IGroupDtoConverter _groupDtoConverter;

        public UserDtoConverter( IGroupDtoConverter groupDtoConverter )
        {
            _groupDtoConverter = groupDtoConverter;
        }

        public List<UserDto> Convert( List<User> users )
        {
            return users.ConvertAll( Convert );
        }

        public UserDto Convert( User user )
        {
            return new UserDto
            {
                Id = user.UserId,
                Name = user.DisplayName
            };
        }

        public User Convert( UserDto user )
        {
            return new User
            {
                DisplayName = user.Name,
                UserId = user.Id
            };
        }

        public List<UserDto> ConvertFromUsersWithGroups( Dictionary<User, List<Group>> users )
        {
            List<UserDto> result = new List<UserDto>();
            foreach ( KeyValuePair<User, List<Group>> pair in users )
            {
                UserDto user = Convert( pair.Key );
                user.Groups = _groupDtoConverter.Convert( pair.Value );
                result.Add( user );
            }
            return result;
        }
    }
}