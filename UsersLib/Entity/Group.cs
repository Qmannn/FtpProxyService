using UsersLib.DbEntity;

namespace UsersLib.Entity
{
    public class Group
    {
        public Group()
        {
        }
        public Group( DbGroup dbGroup )
        {
            Id = dbGroup.Id;
            Name = dbGroup.Name;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DbGroup ConvertToDbGroup()
        {
            return new DbGroup
            {
                Name = Name,
                Id = Id
            };
        }
    }
}