using BusinessObject.Models;
using InfertilityTreatment.InfertilityHomePage;
using Microsoft.Identity.Client;
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

namespace InfertilityTreatment.Manager
{
    /// <summary>
    /// Interaction logic for ManagerHomePage.xaml
    /// </summary>
    public partial class ManagerHomePage : Window
    {

        public User CurrentUser { get; set; }
        public ManagerHomePage()
        {
            InitializeComponent();
            CurrentUser = null;

        }
        public ManagerHomePage(User user)
        {
            InitializeComponent();
            CurrentUser = user;

        }
        private void btProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfilePage profilePage = new ProfilePage(CurrentUser, CurrentUser.RoleId);
            profilePage.ShowDialog();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            WebHomePage homePage = new WebHomePage(null);
            this.Close();
        }

        private void ManagerBlog_Click(object sender, RoutedEventArgs e)
        {
            BlogManagerPage blogManagerPage = new BlogManagerPage(CurrentUser);
            blogManagerPage.Show();
        }

        private void ManagerAppointment_Click(object sender, RoutedEventArgs e)
        {
            // get patientRequest from user + reate appointment send back to user and doctor(add apointment into user and doctor schedule)
            AppointmentCheckingPage appointmentCheckingPage = new AppointmentCheckingPage(CurrentUser);
            appointmentCheckingPage.Show();
        }

        private void ManagerDoctor_Click(object sender, RoutedEventArgs e)
        {
            DoctorManagerPage doctorManagerPage = new DoctorManagerPage(CurrentUser);
            doctorManagerPage.Show();
        }

        private void ManagerService_Click(object sender, RoutedEventArgs e)
        {
            ServiceManagerPage serviceManagerPage = new ServiceManagerPage(CurrentUser);
            serviceManagerPage.Show();
        }

        private void FeedBack_Click(object sender, RoutedEventArgs e)
        {
            FeedBackManagerPage feedBackManagerPage = new FeedBackManagerPage(CurrentUser);
            feedBackManagerPage.Show();
        }

        private void DashPort_Click(object sender, RoutedEventArgs e)
        {
            DashSportPage dashSportPage = new DashSportPage();
            dashSportPage.Show();
        }
        
    }
}
