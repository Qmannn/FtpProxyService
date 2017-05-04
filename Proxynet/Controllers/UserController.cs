using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using Proxynet.Models;
using Proxynet.Service.Converters;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Secure.ActiveDirectory;

namespace Proxynet.Controllers
{
    public class UserController : BaseController
    {
        private readonly IDbUserController _dbUserController;
        private readonly IDbGroupController _dbGroupController;
        private readonly IUsersUpdater _usersUpdater;

        private readonly IUserDtoConverter _userDtoConverter;
        private readonly IGroupDtoConverter _userGroupDtoConverter;

        public UserController( 
            IDbUserController dbUserController,
            IUserDtoConverter userDtoConverter, 
            IGroupDtoConverter userGroupDtoConverter, 
            IDbGroupController dbGroupController, 
            IUsersUpdater usersUpdater )
        {
            _dbUserController = dbUserController;
            _userDtoConverter = userDtoConverter;
            _userGroupDtoConverter = userGroupDtoConverter;
            _dbGroupController = dbGroupController;
            _usersUpdater = usersUpdater;
        }

        [HttpPost]
        public ContentResult GetUsers()
        {
            Dictionary<User, List<Group>> users = _dbUserController.GetUsersByGroups();

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
            List<Group> groups = _dbUserController.GetUserGroups( user.Id );

            UserDto userDto = _userDtoConverter.Convert( user );
            userDto.Groups = _userGroupDtoConverter.Convert( groups );


            return Json( userDto );
        }

        [HttpPost]
        public ContentResult GetGroups()
        {
            List<Group> userGroups = _dbUserController.GetUserGroups();
            List<GroupDto> userGroupsDto = userGroups.ConvertAll( _userGroupDtoConverter.Convert );

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
            List<Group> userGroups = _userGroupDtoConverter.Convert( userDto.Groups );

            _dbUserController.SaveUser( user );
            _dbUserController.SaveUserGroups( user.Id, userGroups.Select( item => item.Id ).ToList() );

            return Json( userDto );
        }

        [HttpPost]
        public ActionResult SaveGroup( string name )
        {
            if ( String.IsNullOrEmpty( name ) )
            {
                return HttpNotFound( "Invalid group name" );
            }

            GroupDto groupDto = new GroupDto
            {
                Name = name,
                Id = _dbGroupController.SaveGroup( name )
            };

            return Json( groupDto );
        }

        [HttpPost]
        public EmptyResult UpdateUsers()
        {
            _usersUpdater.Update();
            return new EmptyResult();
        }
    }
}