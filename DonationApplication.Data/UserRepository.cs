using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationApplication.Data
{
    public class UserRepository
    {
        private string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(string firstName, string lastName, string emailAddress, string password, bool isAdmin)
        {
            string salt = PasswordHelper.GenerateSalt();
            string hash = PasswordHelper.HashPassword(password, salt);

            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                User user = new User
                {
                    Email = emailAddress,
                    FirstName = firstName,
                    LastName = lastName,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    IsAdmin = isAdmin
                };
                context.Users.InsertOnSubmit(user);
                context.SubmitChanges();
            }
        }

        public User Login(string emailAddress, string password)
        {
            User user = GetUser(emailAddress);
            if (user == null)
            {
                return null;
            }

            bool isMatch = PasswordHelper.IsMatch(password, user.PasswordHash, user.PasswordSalt);
            if (isMatch)
            {
                return user;
            }

            return null;
        }

        public User GetUser(string emailAddress)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Email == emailAddress);
            }
        }

        public User GetUser(int id)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                return context.Users.FirstOrDefault(u => u.Id == id);
            }
        }

        public bool EmailExists(string email)
        {
            using (var context = new DonationApplicationsDataContext(_connectionString))
            {
                return context.Users.Any(u => u.Email == email);
            }
        }

    }
}
