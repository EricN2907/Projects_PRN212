using BusinessObject.Models;
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
    /// Interaction logic for UpdatePasswordPage.xaml
    /// </summary>
    public partial class UpdatePasswordPage : Window
    {
        private User _currentUser { get; set; }
        private readonly IUserService _userService;
        public UpdatePasswordPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _userService = new UserService();
        }

        private void UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string oldPassword = OldPasswordBox.Password;
                string newPassword = NewPasswordBox.Password;
                string confirmPassword = ConfirmPasswordBox.Password;
                bool UpdateSuccess = _userService.updatePassword(_currentUser.UserId, newPassword, confirmPassword, oldPassword);

                if (!UpdateSuccess)
                {
                  MessageBox.Show("Password update failed. Please check your old password and ensure the new passwords match.", "Update Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show("Password updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close(); // Close the window after successful update
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the password: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
