namespace SharedTrip.Services
{
    using SharedTrip.Models;
    using SIS.MvcFramework;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void ChangePassword(string username, string newPassword)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return;
            }

            user.Password = this.Hash(newPassword);
            db.SaveChanges();
        }

        public void CreateUser(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = this.Hash(password),
                Role = IdentityRole.User
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            var passwordHashed = this.Hash(password);

            return this.db.Users
                .Where(u => u.Username == username && u.Password == passwordHashed)
                .Select(u => u.Id)
                .FirstOrDefault();
        }

        public bool IsEmailUsed(string email)
        {
            return this.db.Users.Any(u => u.Email == email);
        }

        public bool IsUsernameUsed(string username)
        {
            return this.db.Users.Any(u => u.Username == username);
        }

        private string Hash(string input)
        {
            if (input == null)
            {
                return null;
            }

            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
