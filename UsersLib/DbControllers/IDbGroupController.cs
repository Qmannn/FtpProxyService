using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UsersLib.Entity;

namespace UsersLib.DbControllers
{
    public interface IDbGroupController
    {
        Group SaveGroup(Group group);
        List<Group> GetGroups();
        void DeleteGroup(int groupId);
    }
}