namespace Proxynet.Service.Validators
{
    public interface IUserValidator
    {
        bool ValidateUserName(string userName, int userId);
    }
}