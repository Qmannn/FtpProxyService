using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Proxynet.Models;
using Proxynet.Service.Converters;
using Proxynet.Service.Finders;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Service.ActiveDirectory;

namespace Proxynet.Controllers
{
    [RoutePrefix("service/users")]
    public class UserApiController : BaseApiController
    {
        private readonly IDbUserController _dbUserController;
        private readonly IUsersUpdater _usersUpdater;

        private readonly IUserDtoConverter _userDtoConverter;
        private readonly IGroupDtoConverter _groupDtoConverter;

        public UserApiController(
            IDbUserController dbUserController, 
            IUsersUpdater usersUpdater, 
            IUserDtoConverter userDtoConverter, 
            IGroupDtoConverter groupDtoConverter)
        {
            _dbUserController = dbUserController;
            _usersUpdater = usersUpdater;
            _userDtoConverter = userDtoConverter;
            _groupDtoConverter = groupDtoConverter;
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
            userDto.Groups = _groupDtoConverter.Convert(groups);
            return Ok(userDto);
        }

        [HttpPut]
        [Route("save-user")]
        public IHttpActionResult SaveUser(UserDto user)
        {
            if (user == null || user.Id == 0)
            {
                return NotFound();
            }

            User userToSave = _userDtoConverter.Convert(user);
            List<Group> userGroups = _groupDtoConverter.Convert(user.Groups);

            _dbUserController.SaveUser(userToSave);
            _dbUserController.SaveUserGroups(userToSave.Id, userGroups.Select(item => item.Id).ToList());

            return Ok(user);
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