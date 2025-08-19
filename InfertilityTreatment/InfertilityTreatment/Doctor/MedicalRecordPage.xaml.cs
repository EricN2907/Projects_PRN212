using BusinessObject.Models;
using Service.Implements;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InfertilityTreatment.Doctor
{
    public partial class MedicalRecordPage : Window
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IUserService _userService;
        private readonly IAppointmentService _appointmentService;
        private readonly int _doctorId;

        public MedicalRecordPage(User currentDoctor)
        {
            InitializeComponent();
            _medicalRecordService = new MedicalRecordService();
            _userService = new UserService();
            _appointmentService = new AppointmentService();
            _doctorId = currentDoctor.UserId;

            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customers = _userService.GetAllCustomer();
            CustomerComboBox.ItemsSource = customers;
            CustomerComboBox.DisplayMemberPath = "FullName";
            CustomerComboBox.SelectedValuePath = "UserId";
        }

        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppointmentComboBox.ItemsSource = null;

            if (CustomerComboBox.SelectedValue is int customerId)
            {
                var appointments = _appointmentService
                    .GetAllAppointmentAcceptedByCustomerId(customerId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToList();

                AppointmentComboBox.ItemsSource = appointments;
                AppointmentComboBox.DisplayMemberPath = "AppointmentDate"; // Or a custom string like "AppointmentSummary"
                AppointmentComboBox.SelectedValuePath = "AppointmentId";
            }
        }

        private void SaveMedicalRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CustomerComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Please select a customer.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (AppointmentComboBox.SelectedValue == null)
                {
                    MessageBox.Show("Please select an appointment.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string testResults = TestResultsTextBox.Text.Trim();
                string prescription = PrescriptionTextBox.Text.Trim();
                string note = NoteTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(testResults) &&
                    string.IsNullOrWhiteSpace(prescription) &&
                    string.IsNullOrWhiteSpace(note))
                {
                    MessageBox.Show("Please enter at least one field before saving.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(testResults) && !double.TryParse(testResults, out _))
                {
                    MessageBox.Show("Test result must be a valid number.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                int customerId = (int)CustomerComboBox.SelectedValue;
                int appointmentId = (int)AppointmentComboBox.SelectedValue;

                var record = new MedicalRecord
                {
                    AppointmentId = appointmentId,
                    CustomerId = customerId,
                    DoctorId = _doctorId,
                    TestResults = !string.IsNullOrWhiteSpace(testResults) ? testResults : null,
                    Prescription = prescription,
                    Note = note,
                    CreatedDate = DateTime.Now
                };

                _medicalRecordService.AddMedicalRecord(record);

                MessageBox.Show("Medical record saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving medical record: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void WatchCustomerChart_Click(object sender, RoutedEventArgs e)
        {
           
            
                var chartPage = new DoctorChartPage(_doctorId);
                chartPage.ShowDialog();
          
        }
    }
}
