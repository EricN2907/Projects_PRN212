using BusinessObject.Models;
using Repository.ResponseModel;
using Service.Implements;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InfertilityTreatment.Manager
{
    public partial class DoctorManagerPage : Window
    {
        private User CurrentUser { get; set; }
        private readonly IUserService _userService;
        private readonly IScheduleService _scheduleService;
        private readonly IAppointmentService _appointmentService;
        private readonly ITreatmentServiceService _treatmentService;
        private int _DoctorId;
        private int _ScheduleId;

        public DoctorManagerPage()
        {
            InitializeComponent();
            CurrentUser = null;
            _userService = new UserService();
            _scheduleService = new ScheduleService();
            _appointmentService = new AppointmentService();
            _treatmentService = new TreatmentServiceService();
        }

        public DoctorManagerPage(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            _userService = new UserService();
            _scheduleService = new ScheduleService();
            _appointmentService = new AppointmentService();
            _treatmentService = new TreatmentServiceService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeMonthYearDropdowns();
        }

        private void InitializeMonthYearDropdowns()
        {
            MonthComboBox.ItemsSource = Enumerable.Range(1, 12)
                .Select(m => new DateTime(2000, m, 1).ToString("MMMM"))
                .ToList();
            MonthComboBox.SelectedIndex = DateTime.Now.Month - 1;

            int currentYear = DateTime.Now.Year;
            YearComboBox.ItemsSource = Enumerable.Range(currentYear - 2, 5).ToList();
            YearComboBox.SelectedItem = currentYear;
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var doctors = _userService.GetALlDoctorInfor();
                DoctorComboBox.ItemsSource = doctors;
                DoctorComboBox.DisplayMemberPath = "FullName";
                DoctorComboBox.SelectedValuePath = "DoctorId";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DoctorComboBox.SelectedItem is User selectedDoctor)
                {
                    DoctorNameTextBlock.Text = $"Name: {selectedDoctor.UserName}";
                    DoctorFullNameTextBlock.Text = $"Full Name: {selectedDoctor.FullName}";
                    DoctorPhoneTextBlock.Text = $"Phone: {selectedDoctor.PhoneNumber}";
                    DoctorAgeTextBlock.Text = $"Age: {selectedDoctor.Age}";
                    LoadCustomerBookingWithDoctor(selectedDoctor.UserId);
                    _DoctorId = selectedDoctor.UserId;
                    LoadScheduleForSelectedDoctor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting doctor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LoadCustomerBookingWithDoctor(int doctorId)
        {
            var Appointments = _appointmentService.GetAllAppointmentByDoctorId(doctorId);
            var userHadBooking = new List<DoctorCustomerInfor>();
            foreach (var appointment in Appointments)
            {
                User customer = _userService.GetUserById(appointment.CustomerId);
                TreatmentService service = _treatmentService.GetTreatmentServiceById(appointment.ServiceId);
                if (customer != null)
                {
                    userHadBooking.Add(new DoctorCustomerInfor
                    {
                        Name = customer.UserName,
                        Phone = customer.PhoneNumber,
                        Age = (int)customer.Age,
                        TreatmentService = service?.ServiceName ?? "N/A"
                    });
                }
            }
            DoctorCustomerListView.ItemsSource = userHadBooking;
            DoctorCustomerCountTextBlock.Text = $"Customers: {userHadBooking.Count}";
        }

        private void AddDoctor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new AddUpdateDoctorInformation();
                if (window.ShowDialog() == true)
                {
                    DoctorComboBox.ItemsSource = _userService.GetALlDoctorInfor();
                    DoctorComboBox.SelectedIndex = -1;
                    ClearDoctorInfoDisplay();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding doctor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateDoctor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DoctorComboBox.SelectedItem is User selectedDoctor)
                {
                    var updateWindow = new AddUpdateDoctorInformation(selectedDoctor);
                    if (updateWindow.ShowDialog() == true)
                    {
                        DoctorComboBox.ItemsSource = _userService.GetALlDoctorInfor();
                        DoctorComboBox.SelectedIndex = -1;
                        ClearDoctorInfoDisplay();
                    }
                }
                else
                {
                    MessageBox.Show("Please select a doctor first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening update window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteDoctor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int userId = _DoctorId;
                if (userId <= 0)
                {
                    MessageBox.Show("Please select a doctor to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool isDeleted = _userService.DeleteUser(userId);
                DoctorComboBox.SelectedIndex = -1;
                DoctorComboBox.ItemsSource = _userService.GetALlDoctorInfor();
                if (isDeleted)
                {
                    MessageBox.Show("Doctor deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearDoctorInfoDisplay();
                    DoctorCustomerListView.ItemsSource = null;
                }
                else
                {
                    MessageBox.Show("Failed to delete doctor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting doctor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearDoctorInfoDisplay()
        {
            DoctorNameTextBlock.Text = string.Empty;
            DoctorFullNameTextBlock.Text = string.Empty;
            DoctorPhoneTextBlock.Text = string.Empty;
            DoctorAgeTextBlock.Text = string.Empty;
            DoctorCustomerCountTextBlock.Text = string.Empty;
            DoctorCustomerListView.ItemsSource = null;
        }

        private ScheduleDisplay GetSelectedScheduleFromWeekTabs()
        {
            return Week1ListView.SelectedItem as ScheduleDisplay ??
                   Week2ListView.SelectedItem as ScheduleDisplay ??
                   Week3ListView.SelectedItem as ScheduleDisplay ??
                   Week4ListView.SelectedItem as ScheduleDisplay;
        }

        private void AddSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var window = new AddUpdateScheduleInfor(_DoctorId);
                if (window.ShowDialog() == true)
                {
                    LoadScheduleForSelectedDoctor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding schedule: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selected = GetSelectedScheduleFromWeekTabs();
                if (selected == null)
                {
                    MessageBox.Show("Please select a schedule first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int scheduleId = selected.ScheduleId;
                var window = new AddUpdateScheduleInfor(_DoctorId, scheduleId);
                if (window.ShowDialog() == true)
                {
                    LoadScheduleForSelectedDoctor();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating schedule: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_ScheduleId == 0)
                {
                    MessageBox.Show("Please select a schedule to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool isDeleted = _scheduleService.DeleteSchedule(_ScheduleId);
                if (isDeleted)
                {
                    MessageBox.Show("Schedule deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadScheduleForSelectedDoctor();
                }
                else
                {
                    MessageBox.Show("Failed to delete schedule.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting schedule: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ScheduleDay_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView listView && listView.SelectedItem != null)
            {
                dynamic selectedSchedule = listView.SelectedItem;
                _ScheduleId = selectedSchedule.ScheduleId;

                string message = $"Day: {selectedSchedule.DayName}\n" +
                                 $"Start Time: {selectedSchedule.StartTime}\n" +
                                 $"End Time: {selectedSchedule.EndTime}\n" +
                                 $"Notes: {selectedSchedule.Notes}";

                MessageBox.Show(message, "Schedule Detail", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadScheduleForSelectedDoctor()
        {
            if (_DoctorId <= 0 || MonthComboBox.SelectedIndex == -1 || YearComboBox.SelectedItem == null)
                return;

            int month = MonthComboBox.SelectedIndex + 1;
            int year = (int)YearComboBox.SelectedItem;

            var schedules = _scheduleService.GetSchedulesByDoctorId(_DoctorId)
                .Where(s => s.ScheduleDate.HasValue &&
                            s.ScheduleDate.Value.Month == month &&
                            s.ScheduleDate.Value.Year == year)
                .Select(s => new ScheduleDisplay
                {
                    ScheduleId = s.ScheduleId,
                    ScheduleDate = s.ScheduleDate.Value,
                    DayName = s.ScheduleDate.Value.ToString("dddd"),
                    StartTime = s.ScheduleDate.Value.ToString("HH:mm"),
                    EndTime = s.ScheduleDate.Value.AddHours(4).ToString("HH:mm"),
                    Notes = s.Note ?? ""
                })
                .ToList();

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
            if (MonthComboBox.SelectedIndex != -1 && YearComboBox.SelectedItem != null)
            {
                LoadScheduleForSelectedDoctor();
            }
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MonthComboBox.SelectedIndex != -1 && YearComboBox.SelectedItem != null)
            {
                LoadScheduleForSelectedDoctor();
            }
        }
    }
}
