using System.Collections.Generic;
using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Service.Finders
{
    public class UserDataFinder : IUserDataFinder
    {
        private readonly IDbUserController _dbUserController;
        private readonly IUserDtoConverter _userDtoConverter;
        private readonly IGroupDtoConverter _groupDtoConverter;
        private readonly IUserAccountDtoConverter _userAccountDtoConverter;
        private readonly IDbAuthController _dbAuthController;

        public UserDataFinder(IDbUserController dbUserController, 
            IUserDtoConverter userDtoConverter, 
            IGroupDtoConverter groupDtoConverter, 
            IUserAccountDtoConverter userAccountDtoConverter, 
            IDbAuthController dbAuthController)
        {
            _dbUserController = dbUserController;
            _userDtoConverter = userDtoConverter;
            _groupDtoConverter = groupDtoConverter;
            _userAccountDtoConverter = userAccountDtoConverter;
            _dbAuthController = dbAuthController;
        }

        public UserDto GetUser(int userId)
        {
            User user = _dbUserController.GetUser(userId);
            if (user == null)
            {
                return null;
            }
            List<Group> groups = _dbUserController.GetUserGroups(user.UserId);

            UserDto userDto = _userDtoConverter.Convert(user);
            userDto.Groups = _groupDtoConverter.Convert(groups);

            UserAccount userAccount = _dbAuthController.GetUserAccount(user.UserId);
            if (userAccount != null)
            {
                userDto.Account = _userAccountDtoConverter.Convert(userAccount);
            }

            return userDto;
        }

        public List<UserDto> GetUsersWithGroups()
        {
            Dictionary<User, List<Group>> users = _dbUserController.GetUsersByGroups();
            return _userDtoConverter.ConvertFromUsersWithGroups(users);
        }
    }
}