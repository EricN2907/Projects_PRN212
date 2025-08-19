using BusinessObject.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class UserRepository : IUserRepository
    {
        private readonly InfertilityTreatmentContext _context;

        public UserRepository()
        {
            _context = new InfertilityTreatmentContext();
        }

        public UserRepository(InfertilityTreatmentContext context)
        {
            _context = context;
        }

        public List<User> GetAllUser()
        {

            List<User> listUser = _context.Users.Where(u => u.IsActive == true).ToList();

            if (listUser.Count() == 0)
            {
                return null;
            }
            return listUser;



        }

        public User AddUsere(User newUser)
        {
            if (newUser == null)
            {
                return null;
            }
            _context.Users.Add(newUser);
            _context.SaveChanges();
            return newUser;

        }
        public User GetUserById(int userId)
        {

            User exitUser = _context.Users.FirstOrDefault(u => u.UserId == userId && u.IsActive == true);

            if (exitUser == null)
            {
                return null;
            }
            return exitUser;

        }

        public User UpdateUser(User newUser, int userId)
        {
            User exitUser = GetUserById(userId);
            if (exitUser == null)
            {
                return null;
            }
            exitUser.UserName = newUser.UserName;
            exitUser.Password = newUser.Password;
            exitUser.FullName = newUser.FullName;
            exitUser.PhoneNumber = newUser.PhoneNumber;
            exitUser.Age = newUser.Age;
            exitUser.RoleId = newUser.RoleId;
            exitUser.IsActive = newUser.IsActive;

            _context.SaveChanges();
            return exitUser;

        }

        public bool DeleteUser(int userId)
        {

            User exitUser = GetUserById(userId);
            if (exitUser == null)
            {
                return false;
            }
            exitUser.IsActive = false;
            _context.SaveChanges();
            return true;


        }

        public User Login(string userName, string password)
        {

            return _context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password && u.IsActive == true);

        }

        public User Register(string userName, string password, string confirmPassword, string fullName, int age, string phoneNumber)
        {
            User exitUSer = _context.Users.FirstOrDefault(u => u.UserName == userName && u.IsActive);

            if (exitUSer != null)
            {
                return null; // if user exit 
            }
            if (password != confirmPassword)
            {
                return null; // password and confirm password are not the same
            }
            //create new user 
            User newUser = new User
            {
                UserName = userName,
                Password = password,
                FullName = fullName,
                Age = age,
                PhoneNumber = phoneNumber,
                IsActive = true,
                StartActiveDate = DateTime.UtcNow,
                RoleId = 4 // customer
            };
            AddUsere(newUser);
            return newUser;
        }

        public List<User> GetALlDoctorInfor()
        {
            return _context.Users
                .Where(u => u.IsActive == true && u.RoleId == 3) // Assuming RoleId 3 is for doctors
                .ToList();
        }

        public List<User> GetAllCustomer()
        {
            return _context.Users
                    .Where(u => u.IsActive == true && u.RoleId == 4) // Assuming RoleId 4 is for customers
                    .ToList();
        }

        public bool updatePassword(int userId, string newPassword, string confirmPassword, string OldPassowrd)
        {
            User user = GetUserById(userId);
            if (user == null)
            {
                return false; // User not found
            }
            if (user.Password != OldPassowrd)
            {
                return false; // Old password does not match
            }
            if (OldPassowrd == newPassword)
            {
                return false;
            }
            if (newPassword != confirmPassword)
            {
                return false;
            }
            user.Password = newPassword;
            _context.SaveChanges();
            return true; // Password updated successfully
        }
    }
}
