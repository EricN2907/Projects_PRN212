using BusinessObject.Models;
using Repository.ResponseModel;
using Service.Implements;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace InfertilityTreatment.InfertilityHomePage
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Window
    {
        private readonly IUserService _userService;
        private readonly int _role;
        private int userId;
        private User CurrentUser;
        public ProfilePage(User user, int role)
        {
            InitializeComponent();
            _userService = new UserService();
            _role = role;
            userId = user.UserId;
            CurrentUser = user;
            UserProfile userProfile = new UserProfile();
            userProfile.UserName = user.UserName;
            userProfile.FullName = user.FullName;
            userProfile.Age = user.Age;
            userProfile.PhoneNumber = user.PhoneNumber;


            UsernameText.Text = userProfile.UserName;
            FullNameText.Text = userProfile.FullName ?? "N/A";
            AgeText.Text = userProfile.Age.HasValue ? userProfile.Age.ToString() : "N/A";
            PhoneText.Text = userProfile.PhoneNumber ?? "N/A";

            switch (_role)
            {
                case 3:
                    RoleInfoText.Text = "🩺 Logged in as Doctor";
                    break;
                case 4:
                    RoleInfoText.Text = "🙍‍♂️ Logged in as Customer";
                    break;
                case 2:
                    RoleInfoText.Text = "🛠️ Logged in as Manager";
                    break;
                default:
                    RoleInfoText.Text = "👤 Unknown Role";
                    break;
            }
        }

        private void UpdateInformation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                string userName = UsernameText.Text.Trim();
                string fullName = FullNameText.Text.Trim();
                int? age = string.IsNullOrEmpty(AgeText.Text) ? (int?)null : int.Parse(AgeText.Text);
                string phoneNumber = PhoneText.Text.Trim();
                User newUser = new User
                {
                    UserId = userId,
                    UserName = userName,
                    FullName = fullName,
                    Age = age,
                    PhoneNumber = phoneNumber,
                    Password = CurrentUser.Password, // Assuming you don't want to change the password here
                    RoleId = CurrentUser.RoleId,
                    IsActive = CurrentUser.IsActive
                };
                _userService.UpdateUser(newUser, userId);
                MessageBox.Show("Information updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UpdatePasswordPage updatePasswordPage = new UpdatePasswordPage(CurrentUser);
                updatePasswordPage.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating password: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
