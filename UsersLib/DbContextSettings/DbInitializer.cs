using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Cryptography;
using System.Text;
using UsersLib.Entity;

namespace UsersLib.DbContextSettings
{
    public class DbInitializer: CreateDatabaseIfNotExists<UsersLibDbContext>
    {
        protected override void Seed(UsersLibDbContext db)
        {
            User user = new User
            {
                DisplayName = "Administrator",
                UserAccount = new UserAccount
                {
                    Login = "admin",
                    Password = GetPasswordHashString("admin")
                },
                UserRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        Role = UserRoleKind.Admin
                    }
                }
            };

            db.Users.Add(user);
            db.SaveChanges();
        }

        private string GetPasswordHashString(string password)
        {
            SHA256 hasher = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            string hash = Encoding.UTF8.GetString(hasher.ComputeHash(bytes));
            return hash;
        }
    }
}