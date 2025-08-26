using BusinessObject.Models;
using Repository.ResponseModel;
using Service.Interface;
using Service.Implements;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using InfertilityTreatment.Manager;

namespace InfertilityTreatment.Doctor
{
    public partial class DoctorAppointmentPage : Window
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientRequestService _patientRequestService;
        private readonly IScheduleService _scheduleService;
        private readonly IUserService _userService;
        private readonly ITreatmentServiceService _treatmentServiceService;
        private User _currentUser;

        public DoctorAppointmentPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _appointmentService = new AppointmentService();
            _patientRequestService = new PatientRequestService();
            _scheduleService = new ScheduleService();
            _userService = new UserService();
            _treatmentServiceService = new TreatmentServiceService();
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            try
            {
                // Lấy tất cả các cuộc hẹn của bác sĩ dựa trên DoctorId
                var appointments = _appointmentService.GetAllAppointments()
                    .Where(a => a.DoctorId == _currentUser.UserId) // Lọc theo DoctorId
                    .OrderByDescending(u => u.AppointmentDate)
                    .ToList();

                var pending = appointments
                    .Where(a => a.Status == "Pending")
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                var accepted = appointments
                    .Where(a => a.Status == "Accepted")
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                var rejected = appointments
                    .Where(a => a.Status == "Rejected")
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                // Clear các panel hiện tại
                PendingPanel.Children.Clear();
                AcceptedPanel.Children.Clear();
                RejectedPanel.Children.Clear();

                // Thêm các cuộc hẹn vào các panel
                foreach (var a in pending)
                    PendingPanel.Children.Add(CreateAppointmentCard(a));

                foreach (var a in accepted)
                    AcceptedPanel.Children.Add(CreateAppointmentCard(a));

                foreach (var a in rejected)
                    RejectedPanel.Children.Add(CreateAppointmentCard(a));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Border CreateAppointmentCard(Appointment appointment)
        {
            var border = new Border
            {
                Background = Brushes.White,
                Width = 250,
                Margin = new Thickness(5),
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(8),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1)
            };

            var stack = new StackPanel();

            User customer = _userService.GetUserById(appointment.CustomerId);
            TreatmentService treatmentService = _treatmentServiceService.GetTreatmentServiceById(appointment.ServiceId);
            User doctor = _userService.GetUserById(appointment.DoctorId);

            stack.Children.Add(new TextBlock { Text = $"👤 Customer: {customer.FullName}", FontWeight = FontWeights.Bold });
            stack.Children.Add(new TextBlock { Text = $"🩺 Service: {treatmentService.ServiceName}" });
            stack.Children.Add(new TextBlock { Text = $"🧑‍⚕️ Doctor: {doctor.FullName}" });
            stack.Children.Add(new TextBlock { Text = $"📅 Date Appointment: {appointment.AppointmentDate:dd-MM-yyyy HH:mm}" });

            border.MouseLeftButtonDown += (s, e) => LoadDoctorSchedule(appointment.DoctorId);

            if (appointment.Status == "Pending")
            {
                var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 10, 0, 0) };

                var acceptBtn = new Button { Content = "Accept", Background = Brushes.LightGreen, Margin = new Thickness(5), Tag = appointment };
                acceptBtn.Click += AcceptBtn_Click;

                var rejectBtn = new Button { Content = "Reject", Background = Brushes.IndianRed, Margin = new Thickness(5), Tag = appointment };
                rejectBtn.Click += RejectBtn_Click;

                btnPanel.Children.Add(acceptBtn);
                btnPanel.Children.Add(rejectBtn);
                stack.Children.Add(btnPanel);
            }

            border.Child = stack;
            return border;
        }

        private void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {

            var appointment = (sender as Button)?.Tag as Appointment;
            if (appointment == null) return;

            // Xác nhận và tạo lịch trình cho bác sĩ
            LoadDoctorSchedule(appointment.DoctorId);
            User customer = _userService.GetUserById(appointment.CustomerId);
            TreatmentService treatmentService = _treatmentServiceService.GetTreatmentServiceById(appointment.ServiceId);
            var confirm = MessageBox.Show($"Confirm accepting appointment for {customer.FullName} on {appointment.AppointmentDate}?",
                                          "Confirm Accept", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                string? note = null;

                var result = MessageBox.Show("Do you have any note?", "Add Note", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var noteDialog = new NoteInputDialog();
                    var dialogResult = noteDialog.ShowDialog();

                    if (dialogResult == true)
                    {
                        note = noteDialog.NoteText;
                    }
                    else
                    {
                        return; // Cancel, stop process
                    }
                }

                bool isAlreadyBooked = _scheduleService.CheckingisAlreadyBooked(appointment.DoctorId, appointment.AppointmentDate);
                if (isAlreadyBooked)
                {
                    MessageBox.Show("Doctor is already booked at this time!", "Schedule Conflict", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _appointmentService.UpdateAppointmentStatus(appointment.AppointmentId, "Accepted", note);

                // Tạo Schedule
                Schedule schedule = new Schedule
                {
                    CustomerId = appointment.CustomerId,
                    DoctorId = appointment.DoctorId,
                    SerivceName = treatmentService.ServiceName,
                    ScheduleDate = appointment.AppointmentDate,
                    Note = note
                };
                _scheduleService.AddNewSchedule(schedule);

                // Reload lại UI
                LoadAppointments();
            }
        }

        private void RejectBtn_Click(object sender, RoutedEventArgs e)
        {
            var appointment = (sender as Button)?.Tag as Appointment;
            if (appointment == null) return;

            var inputDialog = new RejectReasonDialog();
            if (inputDialog.ShowDialog() == true)
            {
                string reason = inputDialog.RejectReason;
                _appointmentService.UpdateAppointmentStatus(appointment.AppointmentId, "Rejected", reason);
                LoadAppointments();
            }
        }

        private void LoadDoctorSchedule(int doctorId)
        {
            try
            {
                var schedules = _scheduleService.GetSchedulesByDoctorId(doctorId).OrderByDescending(s => s.ScheduleDate).ToList();
                var schedulesResponse = schedules.Select(schedule => new SchedulesResponse
                {
                    CustomerName = _userService.GetUserById(schedule.CustomerId)?.FullName,
                    ServiceName = schedule.SerivceName,
                    Date = schedule.ScheduleDate?.ToString("dd-MM-yyyy") ?? "N/A",
                    Time = schedule.ScheduleDate?.ToString("HH:mm") ?? "N/A"
                }).ToList();

                DoctorScheduleList.ItemsSource = schedulesResponse;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctor's schedule: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        // Phương thức xử lý sự kiện click của nút "Accept"
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nút Accept đã được nhấn!");
        }

        // Phương thức xử lý sự kiện click của nút "Reject"
        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nút Reject đã được nhấn!");
        }

        // Phương thức xử lý sự kiện click của nút "Back to Home"
        private void BackToHome_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
;       }

    }
}
