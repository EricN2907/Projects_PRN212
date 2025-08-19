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

namespace InfertilityTreatment.Customer
{
    /// <summary>
    /// Interaction logic for BookAppointmentPage.xaml
    /// </summary>
    public partial class BookAppointmentPage : Window
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IUserService _userService;
        private readonly ITreatmentServiceService _treatmentServiceService;
        private User _currentUser { get; set; }
        public BookAppointmentPage()
        {
            InitializeComponent();
            _currentUser = null;
            _appointmentService = new AppointmentService();
            _userService = new UserService();
            _treatmentServiceService = new TreatmentServiceService();
            DoctorComboBox_Loaded(DoctorComboBox, null);
            ServiceComboBox_Loaded(ServiceComboBox, null);
            LoadTimeComboBoxes();

        }
        public BookAppointmentPage(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _appointmentService = new AppointmentService();
            _userService = new UserService();
            _treatmentServiceService = new TreatmentServiceService();
            DoctorComboBox_Loaded(DoctorComboBox, null);
            ServiceComboBox_Loaded(ServiceComboBox, null);
            LoadTimeComboBoxes();
        }

        private void DoctorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            try {
                var doctors = _userService.GetALlDoctorInfor();
                if (doctors != null) {
                    DoctorComboBox.ItemsSource = doctors;
                    DoctorComboBox.DisplayMemberPath = "FullName";
                    DoctorComboBox.SelectedValuePath = "UserId";// data binding (the user can not see the Id)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading doctors: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DoctorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                if (DoctorComboBox.SelectedItem is User selectedDoctor)
                {
                    DoctorNameTextBlock.Text = $"Name: {selectedDoctor.FullName}";
                    DoctorPhoneTextBlock.Text = $"Phone: {selectedDoctor.PhoneNumber}";
                    DoctorAgeTextBlock.Text = $"Age: {selectedDoctor.Age}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting doctor: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ServiceComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var services = _treatmentServiceService.GetAllTreatmentServices();
                if (services != null)
                {
                    // load all Services 
                    ServiceComboBox.ItemsSource = services;
                    ServiceComboBox.DisplayMemberPath = "ServiceName";
                    ServiceComboBox.SelectedValuePath = "ServiceId"; // data binding (the user can not see the Id)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading services: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ServiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ServiceComboBox.SelectedItem is TreatmentService selectedService)
                {
                    ServiceNameTextBlock.Text = $"Service: {selectedService.ServiceName}";
                    ServicePriceTextBlock.Text = $"Price: {selectedService.Price}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error selecting service: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try {
            int customerId = _currentUser.UserId;    
            int doctorId = (int)DoctorComboBox.SelectedValue;
            int serviceId = (int)ServiceComboBox.SelectedValue;
                DateTime selectedDate = AppointmentDatePicker.SelectedDate.Value;
                int selectedHour = int.Parse(HourComboBox.SelectedItem.ToString());
                int selectedMinute = int.Parse(MinuteComboBox.SelectedItem.ToString());

                // Combine Date + Time into full DateTime
                DateTime appointmentDateTime = new DateTime(
                    selectedDate.Year,
                    selectedDate.Month,
                    selectedDate.Day,
                    selectedHour,
                    selectedMinute,
                    0 // seconds
                );
                Appointment appointment = new Appointment
                {
                    CustomerId = customerId,
                    DoctorId = doctorId,
                    ServiceId = serviceId,
                    AppointmentDate = appointmentDateTime,
                    Status = "Pending", // manager will change this
                    RejectReason = null,// manager will change  this 
                };
                _appointmentService.AddAppointment(appointment);
                MessageBox.Show("Appointment submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting appointment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadTimeComboBoxes()
        {
            // Add hours from 08 to 22 (8AM to 10PM)
            for (int hour = 8; hour <= 22; hour++)
            {
                HourComboBox.Items.Add(hour.ToString("D2"));
            }

            // Add minutes in 15-minute steps
            for (int minute = 0; minute < 60; minute += 15)
            {
                MinuteComboBox.Items.Add(minute.ToString("D2"));
            }

            // Default selection
            HourComboBox.SelectedIndex = 0;
            MinuteComboBox.SelectedIndex = 0;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DoctorComboBox.SelectedIndex = -1;
            ServiceComboBox.SelectedIndex = -1;
            AppointmentDatePicker.SelectedDate = null;

            DoctorNameTextBlock.Text = "Name: ";
            DoctorPhoneTextBlock.Text = "Phone: ";
            DoctorAgeTextBlock.Text = "Age: ";

            ServiceNameTextBlock.Text = "Service: ";
            ServicePriceTextBlock.Text = "Price: ";
            ServiceDescriptionTextBlock.Text = "Description: ";

            this.Close();
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
