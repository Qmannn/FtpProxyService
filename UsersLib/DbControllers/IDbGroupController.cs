using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace UsersLib.DbControllers
{
    public interface IDbGroupController
    {
        int SaveGroup( string name );
    }
}