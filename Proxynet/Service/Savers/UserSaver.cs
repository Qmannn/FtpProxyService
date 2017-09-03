using System.Collections.Generic;
using System.Linq;
using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.Entity;

namespace Proxynet.Service.Savers
{
    public class UserSaver : IUserSaver
    {
        private readonly IUserDtoConverter _userDtoConverter;
        private readonly IGroupDtoConverter _groupDtoConverter;
        private readonly IUserAccountDtoConverter _accountDtoConverter;

        private readonly UsersLib.Service.Savers.IUserSaver _userSaver;

        public UserSaver(IUserDtoConverter userDtoConverter, 
            UsersLib.Service.Savers.IUserSaver userSaver, 
            IGroupDtoConverter groupDtoConverter, IUserAccountDtoConverter accountDtoConverter)
        {
            _userDtoConverter = userDtoConverter;
            _userSaver = userSaver;
            _groupDtoConverter = groupDtoConverter;
            _accountDtoConverter = accountDtoConverter;
        }

        public int SaveUser(UserDto userDto)
        {
            User userToSave = _userDtoConverter.Convert(userDto);
            List<Group> userGroups = _groupDtoConverter.Convert(userDto.Groups);
            if (userDto.Account != null && userDto.Account.NeedSaveAccount)
            {
                UserAccount userAccount = _accountDtoConverter.Convert(userDto.Account);
                _userSaver.SaveUser(userToSave, userAccount);

            }
            else
            {
                userToSave = _userSaver.SaveUser(userToSave);
            }
            _userSaver.SaveUserGroups(userToSave.UserId, userGroups.Select(item => item.Id).ToList());
            return userToSave.UserId;
        }
    }
}