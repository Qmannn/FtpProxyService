using System;
using System.Collections.Generic;
using System.Web.Http;
using Proxynet.Models;
using Proxynet.Service.Finders;
using Proxynet.Service.Removers;
using Proxynet.Service.Savers;
using Proxynet.Service.Validators;

namespace Proxynet.Controllers
{
    [RoutePrefix("service/users")]
    public class UserApiController : BaseApiController
    {
        private readonly IUserSaver _userSaver;
        private readonly IUserDataFinder _userDataFinder;
        private readonly IDataRemover _dataRemover;
        private readonly IUserValidator _userValidator;

        public UserApiController( IUserSaver userSaver, 
            IUserDataFinder userDataFinder, 
            IDataRemover dataRemover, 
            IUserValidator userValidator)
        {
            _userSaver = userSaver;
            _userDataFinder = userDataFinder;
            _dataRemover = dataRemover;
            _userValidator = userValidator;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetUsers()
        {
            List<UserDto> users = _userDataFinder.GetUsersWithGroups();
            return Ok(users);
        }

        [HttpGet]
        [Route("get-user")]
        public IHttpActionResult GetUser(int userId)
        {
            UserDto userDto = _userDataFinder.GetUser(userId);
            return Ok(userDto);
        }

        [HttpPut]
        [Route("save-user")]
        public IHttpActionResult SaveUser(UserDto user)
        {
            if (user == null )
            {
                return NotFound();
            }
            int savedUserId = _userSaver.SaveUser(user);
            UserDto userDto = _userDataFinder.GetUser(savedUserId);
            return Ok(userDto);
        }

        [HttpDelete]
        [Route("delete-user")]
        public IHttpActionResult DeleteSite(int userId)
        {
            _dataRemover.RemoveUser(userId);
            return Ok();
        }

        [HttpGet]
        [Route("check-user-name")]
        public IHttpActionResult CheckUserName(string login, int userId)
        {
            if (String.IsNullOrEmpty(login))
            {
                return Ok(false);
            }

            bool loginIsValid = _userValidator.ValidateUserName(login, userId);
            return Ok(loginIsValid);
        }
    }
}