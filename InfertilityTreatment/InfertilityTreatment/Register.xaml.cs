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

namespace InfertilityTreatment
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private readonly IUserService _userService;
        public Register()
        {
            InitializeComponent();
            _userService = new UserService();
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string userName = txtUserName.Text;
                string passWord = txtPassword.Password;
                string confirmPassword = txtConfirmPassword.Password;
                string fullName = txtFullName.Text;
                int age = Int32.Parse(txtAge.Text);
                string phoneNumber = txtPhoneNumber.Text;

                User newUser = _userService.Register(userName, passWord, confirmPassword, fullName, age, phoneNumber);
                if (newUser != null)
                {
                    MessageBox.Show("Register successfully");
                    //vao main page (customer Page)
                }
                else
                {
                    MessageBox.Show("Register false");
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
