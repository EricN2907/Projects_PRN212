using BusinessObject.Models;
using Repository.Implements;
using Repository.Interfaces;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implements
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User AddUsere(User newUser)
        {
            return _userRepository.AddUsere(newUser);
        }

        public bool DeleteUser(int userId)
        {
            return _userRepository.DeleteUser(userId);
        }

        public List<User> GetAllUser()
        {
            return _userRepository.GetAllUser();
        }

        public User GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public User Login(string userName, string password)
        {
            return _userRepository.Login(userName, password);
        }

        public User UpdateUser(User newUser, int userId)
        {
            return _userRepository.UpdateUser(newUser, userId);
        }
        public User Register(string userName, string password, string confirmPassword, string fullName, int age, string phoneNumber)
        {
            return _userRepository.Register(userName, password, confirmPassword, fullName, age, phoneNumber);
        }
        public List<User> GetALlDoctorInfor()
        {
            return _userRepository.GetALlDoctorInfor();
        }
        public List<User> GetAllCustomer()
        {
            return _userRepository.GetAllCustomer();
        }
        public bool updatePassword(int userId, string newPassword, string confirmPassword, string OldPassowrd)
        {
            return _userRepository.updatePassword(userId, newPassword, confirmPassword, OldPassowrd);
        }
    }
}
