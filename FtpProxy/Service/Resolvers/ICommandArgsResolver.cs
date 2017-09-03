namespace FtpProxy.Service.Resolvers
{
    public interface ICommandArgsResolver
    {
        string ResolvePassword(string args);
        string ResolveServerIdentifier(string args);
        string ResolveUserLogin(string args);
    }
}