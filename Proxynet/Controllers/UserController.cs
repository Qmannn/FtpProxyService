using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proxynet.Models;
using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Controllers
{
    public class UserController : BaseController
    {
        private readonly IDbUserController _dbUserController;
        private readonly IDbSiteController _dbSiteController;

        public UserController( IDbUserController dbUserController, IDbSiteController dbSiteController )
        {
            _dbUserController = dbUserController;
            _dbSiteController = dbSiteController;
        }

        public ActionResult Index()
        {
            ViewBag.Users = _dbUserController.GetUsers();
            return View();
        }

        public ActionResult Edit( int userId )
        {
            User user = _dbUserController.GetUser( userId );

            return View( new EditUserViewModel
            {
                User = user,
                UserGroups = _dbUserController.GetUserGroups( userId ),
                Groups = _dbUserController.GetUserGroups()
            } );
        }

        /// <summary>
        /// Изменение видимого понятного имени пользователя
        /// </summary>
        public ActionResult EditName( EditUserViewModel model )
        {
            if( model == null || model.User == null || model.User.Id == 0 )
            {
                return HttpNotFound( "User not found" );
            }
            _dbUserController.SaveUser( model.User );
            return View( "Edit", model );
        }

        public ActionResult TestVIew()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetUsers()
        {
            return Json( new List<User>
            {
                new User
                {
                    Id = 10,
                    Login = "Max",
                    DisplayName = "MMMMAAA"
                },
                new User
                {
                    Id = 1,
                    Login = "LOL",
                    DisplayName = "МАКС"
                }
            } );
        }
    }
}