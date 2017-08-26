using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Service.ActiveDirectory;

namespace Proxynet.Controllers
{
    [RoutePrefix("service/users")]
    public class UserApiController : BaseApiController
    {
        private readonly IDbUserController _dbUserController;
        private readonly IDbGroupController _dbGroupController;
        private readonly IUsersUpdater _usersUpdater;

        private readonly IUserDtoConverter _userDtoConverter;
        private readonly IGroupDtoConverter _userGroupDtoConverter;

        public UserApiController(
            IDbUserController dbUserController, 
            IDbGroupController dbGroupController, 
            IUsersUpdater usersUpdater, 
            IUserDtoConverter userDtoConverter, 
            IGroupDtoConverter userGroupDtoConverter)
        {
            _dbUserController = dbUserController;
            _dbGroupController = dbGroupController;
            _usersUpdater = usersUpdater;
            _userDtoConverter = userDtoConverter;
            _userGroupDtoConverter = userGroupDtoConverter;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetUsers()
        {
            Dictionary<User, List<Group>> users = _dbUserController.GetUsersByGroups();

            return Ok(_userDtoConverter.ConvertFromUsersWithGroups(users));
        }

        [HttpGet]
        [Route("get-user")]
        public IHttpActionResult GetUser(int userId)
        {
            User user = _dbUserController.GetUser(userId);
            if (user == null)
            {
                return NotFound();
            }
            List<Group> groups = _dbUserController.GetUserGroups(user.Id);

            UserDto userDto = _userDtoConverter.Convert(user);
            userDto.Groups = _userGroupDtoConverter.Convert(groups);
            return Ok(userDto);
        }

        [HttpGet]
        [Route("get-groups")]
        public IHttpActionResult GetGroups()
        {
            List<Group> userGroups = _dbUserController.GetUserGroups();
            List<GroupDto> userGroupsDto = userGroups.ConvertAll(_userGroupDtoConverter.Convert);

            return Ok(userGroupsDto);
        }

        [HttpPost]
        [Route("save-user")]
        public IHttpActionResult SaveUser(UserDto user)
        {
            if (user == null || user.Id == 0)
            {
                return NotFound();
            }

            User userToSave = _userDtoConverter.Convert(user);
            List<Group> userGroups = _userGroupDtoConverter.Convert(user.Groups);

            _dbUserController.SaveUser(userToSave);
            _dbUserController.SaveUserGroups(userToSave.Id, userGroups.Select(item => item.Id).ToList());

            return Ok(user);
        }

        [HttpPost]
        [Route("save-group")]
        public IHttpActionResult SaveGroup(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            // TODO group saver
            GroupDto groupDto = new GroupDto
            {
                Name = name,
                Id = _dbGroupController.SaveGroup(name)
            };

            return Ok(groupDto);
        }

        [HttpPost]
        [Route("update-users")]
        public IHttpActionResult UpdateUsers()
        {
            _usersUpdater.Update();
            return Ok();
        }
    }
}