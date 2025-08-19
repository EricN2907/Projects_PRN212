using BusinessObject.Models;
using Service.Implements;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;      // SortDirection, SortDescription
using System.Windows.Data;        // CollectionViewSource


namespace InfertilityTreatment.Customer
{
    public partial class CustomerAppoimentPage : Window
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IScheduleService _scheduleService;
        private readonly ITreatmentServiceService _treatmentServiceService;
        private readonly IUserService _userService;
        private User currentUser;

        public CustomerAppoimentPage(User user)
        {
            InitializeComponent();
            currentUser = user;
            _appointmentService = new AppointmentService();
            _scheduleService = new ScheduleService();
            _treatmentServiceService = new TreatmentServiceService();
            _userService = new UserService();
            Loaded += CustomerAppoimentPage_Loaded;
        }

        private void CustomerAppoimentPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadMonthYearComboBoxes();
            LoadAppointmentsHaveRejeted();
            LoadAppointmentsHaveAccepted(); // Initial load with current month/year
            LoadAllAppointments();
        }

        private void LoadMonthYearComboBoxes()
        {
            var now = DateTime.Now;

            MonthComboBox.ItemsSource = Enumerable.Range(1, 12);
            MonthComboBox.SelectedItem = now.Month;

            YearComboBox.ItemsSource = Enumerable.Range(now.Year - 5, 10);
            YearComboBox.SelectedItem = now.Year;
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) LoadAppointmentsHaveAccepted();
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded) LoadAppointmentsHaveAccepted();
        }

        public void LoadAppointmentsHaveRejeted()
        {
            try
            {
                var appointments = _appointmentService.GetAllAppointmentRejetedByCustomerId(currentUser.UserId);

                var displayData = appointments.Select(a => new
                {
                    Date = a.AppointmentDate.ToString("MM/dd/yyyy hh:mm tt"),
                    Service = _treatmentServiceService.GetTreatmentServiceById(a.ServiceId).ServiceName,
                    Doctor = _userService.GetUserById(a.DoctorId)?.FullName ?? "Unknown",
                    Status = a.Status ?? "Unknown",
                    RejectReason = a.RejectReason ?? ""
                }).ToList();

                RejectedAppointmentListView.ItemsSource = displayData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading rejected appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //public void LoadAppointmentsHaveAccepted()
        //{
        //    try
        //    {
        //        if (MonthComboBox.SelectedItem == null || YearComboBox.SelectedItem == null)
        //            return;

        //        int selectedMonth = (int)MonthComboBox.SelectedItem;
        //        int selectedYear = (int)YearComboBox.SelectedItem;

        //        var schedules = _scheduleService
        //            .GetSchedulesByCustomerId(currentUser.UserId)
        //            .Where(s => s.ScheduleDate.HasValue &&
        //                        s.ScheduleDate.Value.Month == selectedMonth &&
        //                        s.ScheduleDate.Value.Year == selectedYear)
        //            .ToList();

        //        // Clear all week views
        //        Week1ListView.ItemsSource = null;
        //        Week2ListView.ItemsSource = null;
        //        Week3ListView.ItemsSource = null;
        //        Week4ListView.ItemsSource = null;

        //        // Group by week
        //        var weekGroups = schedules.GroupBy(s =>
        //        {
        //            int day = s.ScheduleDate!.Value.Day;
        //            if (day <= 7) return 1;
        //            if (day <= 14) return 2;
        //            if (day <= 21) return 3;
        //            return 4;
        //        });

        //        foreach (var group in weekGroups)
        //        {
        //            var items = group.Select(s => new
        //            {
        //                DayName = s.ScheduleDate.HasValue ? s.ScheduleDate.Value.DayOfWeek.ToString() : "", 
        //                Date = s.ScheduleDate.HasValue ? s.ScheduleDate.Value.ToString("dd/MM/yyyy") : "",
        //                Time = s.ScheduleDate.HasValue ? s.ScheduleDate.Value.ToString("hh:mm tt") : "",
        //                Notes = s.Note
        //            }).ToList();

        //            switch (group.Key)
        //            {
        //                case 1: Week1ListView.ItemsSource = items; break;
        //                case 2: Week2ListView.ItemsSource = items; break;
        //                case 3: Week3ListView.ItemsSource = items; break;
        //                case 4: Week4ListView.ItemsSource = items; break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred while loading schedules: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        public void LoadAppointmentsHaveAccepted()
        {
            try
            {
                if (MonthComboBox.SelectedItem == null || YearComboBox.SelectedItem == null)
                    return;

                int selectedMonth = (int)MonthComboBox.SelectedItem;
                int selectedYear = (int)YearComboBox.SelectedItem;

                var appts = QueryAppointments(currentUser.UserId, selectedMonth, selectedYear,
                                              "Scheduled", "Accepted");

                // Clear all week views
                Week1ListView.ItemsSource = null;
                Week2ListView.ItemsSource = null;
                Week3ListView.ItemsSource = null;
                Week4ListView.ItemsSource = null;

                // Group into weeks 1..4
                var weekGroups = appts.GroupBy(a =>
                {
                    int day = a.AppointmentDate.Day;
                    if (day <= 7) return 1;
                    if (day <= 14) return 2;
                    if (day <= 21) return 3;
                    return 4;
                });

                foreach (var g in weekGroups)
                {
                    var items = g.Select(a => new
                    {
                        DayName = a.AppointmentDate.DayOfWeek.ToString(),
                        Date = a.AppointmentDate.ToString("dd/MM/yyyy"),
                        Time = a.AppointmentDate.ToString("hh:mm tt"),
                        Notes = "" // bạn muốn show ghi chú gì thì thay ở đây
                    }).ToList();

                    switch (g.Key)
                    {
                        case 1: Week1ListView.ItemsSource = items; break;
                        case 2: Week2ListView.ItemsSource = items; break;
                        case 3: Week3ListView.ItemsSource = items; break;
                        case 4: Week4ListView.ItemsSource = items; break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading accepted appointments: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private List<Appointment> QueryAppointments(int customerId, int month, int year, params string[] statuses)
        {
            using var ctx = new InfertilityTreatmentContext();

            var q = ctx.Appointments
                       .Where(a => a.CustomerId == customerId
                                && a.AppointmentDate.Month == month
                                && a.AppointmentDate.Year == year);

            if (statuses != null && statuses.Length > 0)
            {
                var set = new HashSet<string>(statuses, StringComparer.OrdinalIgnoreCase);
                q = q.Where(a => a.Status != null && set.Contains(a.Status));
            }

            return q.ToList();
        }
        //private void LoadAllAppointments()
        //{
        //    try
        //    {
        //        using var ctx = new InfertilityTreatmentContext();
        //        var appts = ctx.Appointments
        //                       .Where(a => a.CustomerId == currentUser.UserId)
        //                       .OrderByDescending(a => a.AppointmentDate)
        //                       .ToList();

        //        var displayData = appts.Select(a => new
        //        {
        //            AppointmentId = a.AppointmentId,
        //            Date = a.AppointmentDate.ToString("MM/dd/yyyy hh:mm tt"),
        //            Service = _treatmentServiceService.GetTreatmentServiceById(a.ServiceId).ServiceName,
        //            Doctor = _userService.GetUserById(a.DoctorId)?.FullName ?? "Unknown",
        //            Status = a.Status ?? "Unknown",
        //            CanCancel = (a.AppointmentDate - DateTime.Now) > TimeSpan.FromHours(24)
        //                       && a.Status != "Cancelled"
        //                        && a.Status != "Rejected"
        //    }).ToList();

        //        AllAppointmentsListView.ItemsSource = displayData;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        public void LoadAllAppointments()
        {
            try
            {
                //using var ctx = new InfertilityTreatmentContext();
                //var appts = ctx.Appointments
                //               .Where(a => a.CustomerId == currentUser.UserId)
                //               .OrderByDescending(a => a.AppointmentDate)
                //               .ToList();

                //var displayData = appts.Select(a => new 
                //{
                //    AppointmentId = a.AppointmentId,
                //    Date = a.AppointmentDate.ToString("MM/dd/yyyy hh:mm tt"),
                //    Service = _treatmentServiceService.GetTreatmentServiceById(a.ServiceId).ServiceName,
                //    Doctor = _userService.GetUserById(a.DoctorId)?.FullName ?? "Unknown",
                //    Status = a.Status ?? "Unknown",
                //    CanCancel = (a.AppointmentDate - DateTime.Now) > TimeSpan.FromHours(24)
                //                && a.Status != "Cancelled"
                //                && a.Status != "Rejected"
                //}).ToList();

                //AllAppointmentsListView.ItemsSource = displayData;
                using var ctx = new InfertilityTreatmentContext();
                var appts = ctx.Appointments
                               .Where(a => a.CustomerId == currentUser.UserId)
                               .OrderByDescending(a => a.AppointmentDate)
                               .ToList();

                AllAppointmentsListView.ItemsSource = appts;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        //private void CancelAppointment_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button button && button.DataContext != null)
        //    {
        //        dynamic appointment = button.DataContext;

        //        // Kiểm tra CanCancel
        //        bool canCancel = appointment.CanCancel;
        //        if (!canCancel)
        //        {
        //            MessageBox.Show("This appointment cannot be cancelled.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return;
        //        }

        //        var result = MessageBox.Show(
        //            "Are you sure to cancel this Appointment?",
        //            "Confirm Cancel",
        //            MessageBoxButton.YesNo,
        //            MessageBoxImage.Warning);

        //        if (result == MessageBoxResult.Yes)
        //        {
        //            try
        //            {
        //                int apptId = appointment.AppointmentId;

        //                using var ctx = new InfertilityTreatmentContext();
        //                var app = ctx.Appointments.FirstOrDefault(a => a.AppointmentId == apptId);
        //                if (app != null)
        //                {
        //                    app.Status = "Cancelled";
        //                    ctx.SaveChanges();
        //                }

        //                // Cập nhật UI
        //                appointment.Status = "Cancelled";
        //                appointment.CanCancel = false; // disable nút
        //                AllAppointmentsListView.Items.Refresh();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show($"Error cancelling appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //            }
        //        }
        //    }
        //}

        private void CancelAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext != null)
            {
                var appointment = button.DataContext as Appointment;

                // Kiểm tra CanCancel
                if (appointment == null || !appointment.CanCancel)
                {
                    MessageBox.Show("This appointment cannot be cancelled.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Hỏi người dùng có chắc chắn muốn hủy không?
                var result = MessageBox.Show(
                    "Are you sure you want to cancel this appointment?",
                    "Confirm Cancel",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    // Hiển thị cửa sổ yêu cầu nhập lý do hủy
                    var cancelDialog = new CustomerConfirmCancelDialog();
                    if (cancelDialog.ShowDialog() == true)
                    {
                        try
                        {
                            // Lấy lý do hủy từ cửa sổ nhập lý do
                            string cancelReason = cancelDialog.CancelReason;

                            using var ctx = new InfertilityTreatmentContext();
                            var appt = ctx.Appointments.FirstOrDefault(a => a.AppointmentId == appointment.AppointmentId);
                            if (appt != null)
                            {
                                appt.Status = "Cancelled"; // Cập nhật trạng thái trong cơ sở dữ liệu
                                appt.CancelReason = cancelReason; // Cập nhật lý do hủy vào cơ sở dữ liệu
                                ctx.SaveChanges();

                                // Cập nhật UI
                                appointment.Status = "Cancelled";
                                appointment.CancelReason = cancelReason;
                                AllAppointmentsListView.Items.Refresh(); // Làm mới danh sách
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error cancelling appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}
