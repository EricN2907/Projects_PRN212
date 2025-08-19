using BusinessObject.Models;
using Service.Implements;
using Service.Interface;
using System;
using System.Windows;

namespace InfertilityTreatment.Manager
{
    public partial class AddUpdateScheduleInfor : Window
    {
        private int _doctorId;
        private int? _scheduleId;
        private readonly IUserService _userService;
        private readonly IScheduleService _scheduleService;
        private readonly ITreatmentServiceService _treatmentServiceService;

        public AddUpdateScheduleInfor(int doctorId, int? scheduleId = null)
        {
            InitializeComponent();
            _userService = new UserService();
            _scheduleService = new ScheduleService();
            _treatmentServiceService = new TreatmentServiceService();
            _doctorId = doctorId;
            _scheduleId = scheduleId;

            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllCustomers();
            LoadAllServices();

            if (_scheduleId != null)
            {
                LoadScheduleForEditing();
            }
        }

        private void LoadAllCustomers()
        {
            try
            {
                var customers = _userService.GetAllCustomer();
                CustomerComboBox.ItemsSource = customers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadAllServices()
        {
            try
            {
                var services = _treatmentServiceService.GetAllTreatmentServices();
                ServiceComboBox.ItemsSource = services;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load services: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadScheduleForEditing()
        {
            var schedule = _scheduleService.GetScheduleById(_scheduleId.Value);
            if (schedule != null)
            {
                DatePicker.SelectedDate = schedule.ScheduleDate;
                NoteTextBox.Text = schedule.Note;
                if (schedule.CustomerId != 0)
                {
                    CustomerComboBox.SelectedValue = schedule.CustomerId;
                }

                // Set service name if it's matched
                var service = _treatmentServiceService.GetAllTreatmentServices()
                    .FirstOrDefault(s => s.ServiceName == schedule.SerivceName);

                if (service != null)
                {
                    ServiceComboBox.SelectedValue = service.ServiceId;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CustomerComboBox.SelectedValue == null || ServiceComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Please select both a customer and service.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int selectedCustomerId = (int)CustomerComboBox.SelectedValue;
                DateTime? selectedDate = DatePicker.SelectedDate;
                int selectedServiceId = (int)ServiceComboBox.SelectedValue;
                var selectedService = _treatmentServiceService.GetTreatmentServiceById(selectedServiceId);
                string serviceName = selectedService.ServiceName;
                string note = NoteTextBox.Text;

                if (_scheduleId == null)
                {
                    var newSchedule = new Schedule
                    {
                        CustomerId = selectedCustomerId,
                        DoctorId = _doctorId,
                        ScheduleDate = selectedDate,
                        SerivceName = serviceName,
                        Note = note
                    };

                    _scheduleService.AddNewSchedule(newSchedule);
                    MessageBox.Show("Schedule added.");
                }
                else
                {
                    var updatedSchedule = _scheduleService.GetScheduleById(_scheduleId.Value);
                    updatedSchedule.ScheduleDate = selectedDate;
                    updatedSchedule.Note = note;
                    updatedSchedule.SerivceName = serviceName;
                    updatedSchedule.CustomerId = selectedCustomerId;

                    _scheduleService.UpdateSchedule(updatedSchedule, _scheduleId.Value);
                    MessageBox.Show("Schedule updated.");
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save schedule: {ex.Message}\n{ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
