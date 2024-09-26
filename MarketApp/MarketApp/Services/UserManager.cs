using MarketLibrary;
using MarketLibrary.Data;
using System.Linq;

namespace UserPanel.Services
{
    public class UserManager
    {
        private readonly AppDbContext _context;

        public UserManager(AppDbContext context)
        {
            _context = context;
        }

        public void Register(string name, string surname, string email, string pass, DateTime dateOfBirth)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == email);
            if (existingUser == null)
            {
                var user = new User
                {
                    Name = name,
                    Surname = surname,
                    Email = email.ToLower().Trim(),
                    Password = pass,
                    DateOfBirth = dateOfBirth
                };

                _context.Users.Add(user);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Bu email artig qeydiyyatdan kechib.");
            }
        }

        public User Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email.ToLower().Trim() && u.Password == password);
            if (user == null) throw new Exception("Email ve ya shifre yanlishdir.");
            return user;
        }

        public void UpdateProfile(User user, string name, string surname, DateTime dateOfBirth)
        {
            user.Name = name;
            user.Surname = surname;
            user.DateOfBirth = dateOfBirth;
            _context.SaveChanges();
        }

        public void ChangePassword(User user, string newPassword)
        {
            user.Password = newPassword;
            _context.SaveChanges();
        }

        public void Logout(ref User user)
        {
            user = null;
        }
    }
}
