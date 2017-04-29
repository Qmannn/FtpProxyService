using UsersLib.DbEntity;

namespace UsersLib.Entity
{
    public class UserGroup
    {
        public UserGroup()
        {
        }
        public UserGroup( DbUserGroup dbUserGroup )
        {
            Id = dbUserGroup.UserGroupId;
            Name = dbUserGroup.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DbUserGroup ConvertToDbUserGroup()
        {
            return new DbUserGroup
            {
                Name = Name,
                UserGroupId = Id
            };
        }
    }
}