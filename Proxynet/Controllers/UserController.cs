using System.Collections.Generic;
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

        public UserController( IDbUserController dbUserController )
        {
            _dbUserController = dbUserController;
        }

        [HttpPost]
        public JsonResult GetUsers()
        {
            List<User> users = _dbUserController.GetUsers();

            IUserDtoConverter converter = new UserDtoConverter();

            return Json( converter.Convert( users ) );
        }

        [HttpPost]
        public JsonResult GetUser( int userId )
        {
            User user = _dbUserController.GetUser( userId );

            IUserDtoConverter converter = new UserDtoConverter();
            
            return Json(converter.Convert(user));
        }
    }
}