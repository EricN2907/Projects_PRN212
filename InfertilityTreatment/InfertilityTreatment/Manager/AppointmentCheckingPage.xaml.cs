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

namespace InfertilityTreatment.Manager
{
    /// <summary>
    /// Interaction logic for AppointmentCheckingPage.xaml
    /// </summary>
    public partial class AppointmentCheckingPage : Window
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientRequestService _patienRequestService;
        private readonly IScheduleService _scheduleService;
        private readonly IUserService _userService;
        private readonly ITreatmentServiceService _treatmentServiceService;
        private User _currentUser;

        public AppointmentCheckingPage()
        {
            InitializeComponent();
            _appointmentService = new AppointmentService();
            _patienRequestService = new PatientRequestService();
            _scheduleService = new ScheduleService();
            _userService = new UserService();
            _treatmentServiceService = new TreatmentServiceService();
            RefreshAppointments();
        }

        public AppointmentCheckingPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _appointmentService = new AppointmentService();
            _patienRequestService = new PatientRequestService();
            _scheduleService = new ScheduleService();
            _userService = new UserService();
            _treatmentServiceService = new TreatmentServiceService();
            RefreshAppointments();
        }
        private void RefreshAppointments()
        {
            AppointmentCheckingPage_Loaded(null, null);
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void AppointmentCheckingPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                var appointments = _appointmentService.GetAllAppointments()
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

                PendingPanel.Children.Clear();
                AcceptedPanel.Children.Clear();
                RejectedPanel.Children.Clear();

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
            stack.Children.Add(new TextBlock { Text = $" Service: {treatmentService.ServiceName}" });
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

            // nao code cheduler 
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
                        // Cancel was pressed, stop the process
                        return;
                    }
                }
                bool isAlreadyBooked = _scheduleService
               .CheckingisAlreadyBooked(appointment.DoctorId, appointment.AppointmentDate);

                if (isAlreadyBooked)
                {
                    MessageBox.Show("Doctor is already booked at this time!", "Schedule Conflict", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _appointmentService.UpdateAppointmentStatus(appointment.AppointmentId, "Accepted", note);

                // Create PatientRequest
                PatientRequest patientRequest = new PatientRequest
                {
                    CustomerId = appointment.CustomerId,
                    DoctorId = appointment.DoctorId,
                    ServiceId = appointment.ServiceId,
                    Note = note,
                    RequestedDate = appointment.AppointmentDate,
                    CreatedDate = DateTime.Now
                };
                _patienRequestService.AddNewPatientRequest(patientRequest);

                // Create Schedule
                Schedule schedule = new Schedule
                {
                    CustomerId = appointment.CustomerId,
                    DoctorId = appointment.DoctorId,
                    SerivceName = treatmentService.ServiceName,
                    ScheduleDate = appointment.AppointmentDate,
                    Note = note
                };
                IScheduleService scheduleService = new ScheduleService();
                _scheduleService.AddNewSchedule(schedule); // use this to showout schedule doctor's schedule and customer's schedule 

                // Reload UI
                AppointmentCheckingPage_Loaded(null, null);
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
                _appointmentService.UpdateAppointmentStatus(appointment.AppointmentId, "Rejected", reason); // send back to customer 30/6(use getAllAppointMentByCustomerId)
                AppointmentCheckingPage_Loaded(null, null);
            }
        }

        private void LoadDoctorSchedule(int doctorId)
        {
            try
            {
                List<SchedulesResponse> schedulesResponses = new List<SchedulesResponse>();
                // using cheduleService to get all schedules of the doctor
                //var schedules = _scheduleService.GetSchedulesByDoctorId(doctorId);
                var schedules = _scheduleService
                                .GetSchedulesByDoctorId(doctorId)
                                .OrderByDescending(s => s.ScheduleDate) // tăng dần
                                .ToList();

                foreach (var schedule in schedules)
                {
                    SchedulesResponse schedulesResponse = new SchedulesResponse
                    {
                        CustomerName = _userService.GetUserById(schedule.CustomerId)?.FullName,
                        ServiceName = schedule.SerivceName,
                        Date = schedule.ScheduleDate?.ToString("dd-MM-yyyy") ?? "N/A",
                        Time = schedule.ScheduleDate?.ToString("HH:mm") ?? "N/A"
                    };
                    schedulesResponses.Add(schedulesResponse);
                }
                DoctorScheduleList.ItemsSource = schedulesResponses;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctor's schedule: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}