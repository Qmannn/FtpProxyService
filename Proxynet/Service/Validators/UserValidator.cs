using UsersLib.DbControllers;
using UsersLib.Entity;

namespace Proxynet.Service.Validators
{
    public class UserValidator : IUserValidator
    {
        private readonly IDbAuthController _dbAuthController;

        public UserValidator(IDbAuthController dbAuthController)
        {
            _dbAuthController = dbAuthController;
        }

        public bool ValidateUserName(string userName, int userId)
        {
            int existingUserId = _dbAuthController.GetUserId(userName);
            return existingUserId == userId || existingUserId == 0;
        }
    }
}