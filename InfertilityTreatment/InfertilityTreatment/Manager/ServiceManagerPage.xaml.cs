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

namespace InfertilityTreatment.Manager
{
    /// <summary>
    /// Interaction logic for ServiceManagerPage.xaml
    /// </summary>
    public partial class ServiceManagerPage : Window
    {
        private readonly ITreatmentServiceService _treatmentServiceService;
        private  TreatmentService? selectedTreatmentService;
        private readonly IAppointmentService _appointmentService; // use to view list user and list Appointment have booking

        private User CurrentUser { get; set; }
        public ServiceManagerPage()
        {
            InitializeComponent();
            CurrentUser = null;
            _treatmentServiceService = new TreatmentServiceService();
            _appointmentService = new AppointmentService();
            LoadTreatmentServices();
        }

        public ServiceManagerPage(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            _treatmentServiceService = new TreatmentServiceService();
            _appointmentService = new AppointmentService();
            LoadTreatmentServices();
        }

        public void LoadTreatmentServices()
        {
            try
            {
                ServiceCardPanel.Children.Clear();
                var services = _treatmentServiceService.GetAllTreatmentServices();
                foreach (var service in services)
                {
                    var card = CreateServiceCard(service);
                    ServiceCardPanel.Children.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading treatment services: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string serviceName = ServiceNameTextBox.Text.Trim();
                string description = DescriptionTextBox.Text.Trim();
                decimal price = PriceTextBox.Text.Trim() != string.Empty ? decimal.Parse(PriceTextBox.Text.Trim()) : 0;
                int serId = CurrentUser?.UserId ?? 0;
                if (string.IsNullOrEmpty(serviceName) || string.IsNullOrEmpty(description) || price <= 0)
                {
                    MessageBox.Show("Please fill in all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                TreatmentService newService = new TreatmentService
                {
                    ServiceName = serviceName,
                    Description = description,
                    Price = price,
                    UserId = serId
                };
                _treatmentServiceService.AddTreatMentService(newService);
                LoadTreatmentServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding service: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedTreatmentService== null) {
                    MessageBox.Show("Please select a service to update.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                string serviceName = ServiceNameTextBox.Text.Trim();
                string description = DescriptionTextBox.Text.Trim();
                decimal price = PriceTextBox.Text.Trim() != string.Empty ? decimal.Parse(PriceTextBox.Text.Trim()) : 0;
                int serId = CurrentUser?.UserId ?? 0;

                if (string.IsNullOrEmpty(serviceName) || string.IsNullOrEmpty(description) || price <= 0)
                {
                    MessageBox.Show("Please fill in all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                TreatmentService newTreatmentSerivce = new TreatmentService
                {
                    ServiceName = serviceName,
                    Description = description,
                    Price = price,
                    UserId = serId
                };
                int TreatMentServiceId = selectedTreatmentService.ServiceId;
                _treatmentServiceService.UpdateTreatMentService(TreatMentServiceId, newTreatmentSerivce);
                LoadTreatmentServices();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating service: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedTreatmentService == null) {
                    MessageBox.Show("Please select a blog to delete.");
                    return;
                }
                var result = MessageBox.Show($"Are you sure you want to delete '{selectedTreatmentService.ServiceName}'?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes) { 
               bool deleR =  _treatmentServiceService.DeleteTreatmentService(selectedTreatmentService.ServiceId);
                    if (deleR == true)
                    {
                        MessageBox.Show("Service deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    LoadTreatmentServices(); // Reload the service list after deletion

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting service: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating back: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private Border CreateServiceCard(TreatmentService service)
        {
            var border = new Border
            {
                Background = Brushes.White,
                Width = 280,
                Height = 180,
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(8),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Cursor = Cursors.Hand
            };

            var stackPanel = new StackPanel();
            stackPanel.Children.Add(new TextBlock
            {
                Text = "💊 " + service.ServiceName,
                FontWeight = FontWeights.Bold,
                FontSize = 14
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = service.Description?.Length > 100 ? service.Description.Substring(0, 100) + "..." : service.Description,
                FontSize = 12,
                Margin = new Thickness(0, 5, 0, 0),
                TextWrapping = TextWrapping.Wrap
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"💰 Price: {service.Price:C}",
                FontSize = 12,
                Margin = new Thickness(0, 5, 0, 0)
            });

            border.MouseLeftButtonDown += (s, e) =>
            {
                selectedTreatmentService = service;
                HighlightSelectedServiceCard(border);
                // Load selected service into the form for update
                ServiceNameTextBox.Text = service.ServiceName;
                DescriptionTextBox.Text = service.Description;
                PriceTextBox.Text = service.Price.ToString();
            };

            border.Child = stackPanel;

            return border;
        }

        private void HighlightSelectedServiceCard(Border selectedBorder)
        {
            foreach (Border border in ServiceCardPanel.Children)
            {
                border.BorderBrush = Brushes.LightGray;
                border.BorderThickness = new Thickness(1);
            }
            selectedBorder.BorderBrush = Brushes.Blue;
            selectedBorder.BorderThickness = new Thickness(2);
        }

        // List User booking (Appointment) the service 

        // list Appointment booking the serivce
    }
}
