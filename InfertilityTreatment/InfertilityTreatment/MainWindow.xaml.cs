using BusinessObject.Models;
using InfertilityTreatment.Admin;
using InfertilityTreatment.Customer;
using InfertilityTreatment.Doctor;
using InfertilityTreatment.InfertilityHomePage;
using InfertilityTreatment.Manager;
using Service.Implements;
using Service.Interface;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InfertilityTreatment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IUserService _userService;
        public MainWindow()
        {
            InitializeComponent();
            _userService = new UserService(); 
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            try {

                string userName = txtUserName.Text;
                string password = txtPassword.Password;

                if (String.IsNullOrWhiteSpace(userName) || String.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Please enter both username and password.");
                    return;
                }

                User exitUser = _userService.Login(userName, password);
                if (exitUser != null)
                {
                    //Admin
                    if (exitUser.RoleId == 1)
                    {
                        AdminManageHomePage adminManageHomePage = new AdminManageHomePage();
                        adminManageHomePage.Show();
                        this.Close();
                    }
                    //Manager
                    else if (exitUser.RoleId == 2)
                    {
                        ManagerHomePage managerHomePage = new ManagerHomePage(exitUser);
                        managerHomePage.Show();
                        this.Close();
                    }
                    //doctor
                    else if (exitUser.RoleId == 3)
                    {
                        DoctorHomePage doctorHomePage = new DoctorHomePage(exitUser);
                        doctorHomePage.Show();
                        this.Close();
                    }
                    //Customer
                    else if (exitUser.RoleId == 4)
                    {
                        WebHomePage webHomePage = new WebHomePage(exitUser);
                        CustomerPage customerPage = new CustomerPage(exitUser);
                        customerPage.Show();
                        this.Close();
                      
                    }
                }
                else
                {
                    MessageBox.Show("login fails");
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btRegister_Click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
        }
    }
}