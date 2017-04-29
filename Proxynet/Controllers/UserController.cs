using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Controllers
{
    public class UserController : BaseController
    {
        private readonly IDbUserController _dbUserController;

        private readonly IUserDtoConverter _userDtoConverter;
        private readonly IUserGroupDtoConverter _userGroupDtoConverter;

        public UserController( 
            IDbUserController dbUserController,
            IUserDtoConverter userDtoConverter, 
            IUserGroupDtoConverter userGroupDtoConverter )
        {
            _dbUserController = dbUserController;
            _userDtoConverter = userDtoConverter;
            _userGroupDtoConverter = userGroupDtoConverter;
        }

        [HttpPost]
        public ContentResult GetUsers()
        {
            Dictionary<User, List<UserGroup>> users = _dbUserController.GetUsersByGroups();

            return Json( _userDtoConverter.ConvertFromUsersWithGroups( users ) );
        }

        [HttpPost]
        public ActionResult GetUser( int userId )
        {
            User user = _dbUserController.GetUser( userId );
            if ( user == null )
            {
                return HttpNotFound( $"User #{userId} was not found" );
            }
            List<UserGroup> groups = _dbUserController.GetUserGroups( user.Id );

            UserDto userDto = _userDtoConverter.Convert( user );
            userDto.Groups = _userGroupDtoConverter.Convert( groups );


            return Json( userDto );
        }

        [HttpPost]
        public ContentResult GetGroups()
        {
            List<UserGroup> userGroups = _dbUserController.GetUserGroups();
            List<UserGroupDto> userGroupsDto = userGroups.ConvertAll( _userGroupDtoConverter.Convert );

            return Json( userGroupsDto );
        }

        [HttpPost]
        public ActionResult SaveUser( string users )
        {
            var userDto = JsonConvert.DeserializeObject<UserDto>(users);

            if ( userDto == null || userDto.Id == 0 )
            {
                return HttpNotFound( "User not found" );
            }

            User user = _userDtoConverter.Convert( userDto );
            List<UserGroup> userGroups = _userGroupDtoConverter.Convert( userDto.Groups );

            _dbUserController.SaveUser( user );
            _dbUserController.SaveUserGroups( user.Id, userGroups.Select( item => item.Id ).ToList() );

            return Json( userDto );
        }
    }
}