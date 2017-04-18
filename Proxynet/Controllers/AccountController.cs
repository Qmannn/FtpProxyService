using Proxynet.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Web.Mvc;
using UsersLib.Secure.Auth;

namespace Proxynet.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager _auth;

        public AccountController( IAuthenticationManager auth )
        {
            _auth = auth;
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login( string returnUrl )
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login( LoginViewModel model )
        {
            if( !ModelState.IsValid )
                return View();

            LdapAuthorizer authorizer = new LdapAuthorizer();

            if ( true || authorizer.ValidateCredentials( model.UserName, model.Password ) )
            {
                var identity = new ClaimsIdentity( new[] { new Claim( ClaimTypes.Name, model.UserName ), },
                    DefaultAuthenticationTypes.ApplicationCookie );

                _auth.SignIn( new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                }, identity );

                return RedirectToAction( "Index", "User" );
            }
            ModelState.AddModelError( "", "Invalid login attempt." );
            return View( model );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _auth.SignOut();
            return RedirectToAction( "Login", "Account" );
        }

    }
}