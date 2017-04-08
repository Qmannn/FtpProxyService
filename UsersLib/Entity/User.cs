using UsersLib.DbEntity;

namespace UsersLib.Entity
{
    public class User
    {
        public User()
        {

        }

        public User( DbUser dbUser )
        {
            Id = dbUser.UserId;
            Login = dbUser.Login;
            DisplayName = dbUser.DisplayName;
        }

        public int Id { get; set; }

        public string Login { get; set; }

        public string DisplayName { get; set; }

        public DbUser ConvertToDbUser()
        {
            return new DbUser
            {
                UserId = Id,
                Login = Login,
                DisplayName = DisplayName
            };
        }
    }
}