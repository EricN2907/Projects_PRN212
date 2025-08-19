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

namespace InfertilityTreatment.Admin
{
    /// <summary>
    /// Interaction logic for UserManager.xaml
    /// </summary>
    public partial class UserManager : Window
    {
        private readonly IUserService _userService;
        public UserManager()
        {
            InitializeComponent();
            _userService = new UserService();
            LoadAllUser();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgUsers.SelectedItem is User selectedUser)
                {
                    User exitUser = _userService.GetUserById(selectedUser.UserId);
                    if (exitUser != null)
                    {
                        // udpate new User 
                        exitUser.UserName = txtUserName.Text;
                        exitUser.Password = txtPassword.Password;
                        exitUser.FullName = txtFullName.Text;
                        exitUser.Age = Int32.Parse(txtAge.Text);
                        exitUser.PhoneNumber = txtPhoneNumber.Text;
                        exitUser.RoleId = Int32.Parse(txtRoleId.Text);
                        exitUser.IsActive = chkIsActive.IsChecked == true;
                    }
                    _userService.UpdateUser(exitUser, exitUser.UserId);
                    MessageBox.Show("User updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Reload danh sách
                    LoadAllUser();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgUsers.SelectedItem is User selectedUser)
                {
                    User exitUser = _userService.GetUserById(selectedUser.UserId);
                    if (exitUser != null)
                    {
                        _userService.DeleteUser(selectedUser.UserId);
                        LoadAllUser();
                        ClearForm();
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void LoadAllUser()
        {
            try
            {
                List<User> listUser = _userService.GetAllUser();
                dgUsers.ItemsSource = listUser;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Gán dữ liệu dòng chọn vào form
            var selectedUser = dgUsers.SelectedItem as User;
            if (selectedUser != null)
            {
                txtUserName.Text = selectedUser.UserName;
                txtPassword.Password = selectedUser.Password;
                txtFullName.Text = selectedUser.FullName;
                txtAge.Text = selectedUser.Age?.ToString();
                txtPhoneNumber.Text = selectedUser.PhoneNumber;
                txtRoleId.Text = selectedUser.RoleId.ToString();
                chkIsActive.IsChecked = selectedUser.IsActive;
            }
        }
        private void ClearForm()
        {

            txtUserName.Text = "";
            txtPassword.Password = "";
            txtFullName.Text = "";
            txtAge.Text = "";
            txtPhoneNumber.Text = "";
            txtRoleId.Text = "";
            chkIsActive.IsChecked = false;
        }
    }
}
