using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IUserService
    {
        public List<User> GetAllUser();
        public User AddUsere(User newUser);
        public User GetUserById(int userId);
        public User UpdateUser(User newUser, int userId);
        public bool DeleteUser(int userId);
        public User Login(string userName, string password);
        public User Register(string userName, string password, string confirmPassword, string fullName, int age, string phoneNumber);
        public List<User> GetALlDoctorInfor();
        public List<User> GetAllCustomer();
        public bool updatePassword(int userId, string newPassword, string confirmPassword, string OldPassowrd);
    }
}
