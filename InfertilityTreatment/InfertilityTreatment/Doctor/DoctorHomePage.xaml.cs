using BusinessObject.Models;
using InfertilityTreatment.HomePage;
using InfertilityTreatment.InfertilityHomePage;
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

namespace InfertilityTreatment.Doctor
{
    /// <summary>
    /// Interaction logic for DoctorHomePage.xaml
    /// </summary>
    public partial class DoctorHomePage : Window
    {
        private readonly IUserService _userService;
        private readonly IScheduleService _scheduleService;
        private User CurrentUser { get; set; }
        public DoctorHomePage(User user)
        {
            InitializeComponent();
            _userService = new UserService(); 
            _scheduleService = new ScheduleService();
            CurrentUser = user;
            Loaded += Window_Loaded;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadYearMonthComboBoxes();
            LoadDoctorSchedules();
        }

        public void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ContactUs_Click(object sender, RoutedEventArgs e)
        {
            ContactUsPage contactUsPage = new ContactUsPage();
            contactUsPage.ShowDialog();
        }
        public void btProfile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProfilePage profilePage = new ProfilePage(CurrentUser,CurrentUser.RoleId);
                profilePage.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void Logout_Click(object sender, RoutedEventArgs e)
        {
            try
            {   
                MainWindow mainWindow = new MainWindow();
                this.Close();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btMedicalRecordManagerment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MedicalRecordPage medicalRecordPage = new MedicalRecordPage(CurrentUser);
                medicalRecordPage.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btSendFeedBackToManager_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoctorFeedBackPage doctorFeedBackPage = new DoctorFeedBackPage(CurrentUser);
                doctorFeedBackPage.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadYearMonthComboBoxes()
        {
            var currentYear = DateTime.Now.Year;
            for (int i = currentYear - 2; i <= currentYear + 2; i++)
            {
                YearComboBox.Items.Add(i);
            }
            YearComboBox.SelectedItem = currentYear;

            for (int i = 1; i <= 12; i++)
            {
                MonthComboBox.Items.Add(new DateTime(1, i, 1).ToString("MMMM"));
            }
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void LoadDoctorSchedules()
        {
            if (MonthComboBox.SelectedIndex == -1 || YearComboBox.SelectedItem == null) return;

            int month = MonthComboBox.SelectedIndex + 1;
            int year = (int)YearComboBox.SelectedItem;

            var schedules = _scheduleService.GetSchedulesByDoctorId(CurrentUser.UserId)
                .Where(s => s.ScheduleDate.HasValue &&
                            s.ScheduleDate.Value.Month == month &&
                            s.ScheduleDate.Value.Year == year)
                .Select(s => new
                {
                    ScheduleId = s.ScheduleId,
                    DayName = s.ScheduleDate.Value.ToString("dddd"),
                    StartTime = s.ScheduleDate.Value.ToString("HH:mm"),
                    EndTime = s.ScheduleDate.Value.AddHours(4).ToString("HH:mm"),
                    CustomerName = _userService.GetUserById(s.CustomerId).FullName,
                    ServiceName = s.SerivceName ?? "N/A",
                    Notes = s.Note ?? "",
                    ScheduleDate = s.ScheduleDate.Value
                }).ToList();

            Week1ListView.ItemsSource = schedules.Where(s => GetWeekOfMonth(s.ScheduleDate) == 1).ToList();
            Week2ListView.ItemsSource = schedules.Where(s => GetWeekOfMonth(s.ScheduleDate) == 2).ToList();
            Week3ListView.ItemsSource = schedules.Where(s => GetWeekOfMonth(s.ScheduleDate) == 3).ToList();
            Week4ListView.ItemsSource = schedules.Where(s => GetWeekOfMonth(s.ScheduleDate) == 4).ToList();
        }

        private int GetWeekOfMonth(DateTime date)
        {
            int day = date.Day;
            if (day <= 7) return 1;
            if (day <= 14) return 2;
            if (day <= 21) return 3;
            return 4;
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (YearComboBox.SelectedItem != null)
            {
                LoadDoctorSchedules();
            }
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedIndex != -1)
            {
                LoadDoctorSchedules();
            }
        }

        private void ScheduleDay_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem != null)
            {
                dynamic item = listView.SelectedItem;
                string message = $"Day: {item.DayName}\n" +
                                 $"Customer: {item.CustomerName}\n" +
                                 $"Service: {item.ServiceName}\n" +
                                 $"Start Time: {item.StartTime}\n" +
                                 $"End Time: {item.EndTime}\n" +
                                 $"Notes: {item.Notes}";

                MessageBox.Show(message, "Schedule Detail", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btCheckAppointments_Click(object sender, RoutedEventArgs e)
        {
            DoctorAppointmentPage appointmentPage = new DoctorAppointmentPage(CurrentUser);
            appointmentPage.Show();
        }
    }
}
