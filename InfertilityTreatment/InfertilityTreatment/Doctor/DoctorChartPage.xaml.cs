using BusinessObject.Models;
using LiveCharts;
using LiveCharts.Wpf;
using Service.Implements;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InfertilityTreatment.Doctor
{
    public partial class DoctorChartPage : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }

        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IUserService _userService;
        private readonly int _doctorId;

        public DoctorChartPage(int doctorId)
        {
            InitializeComponent();
            DataContext = this;

            _medicalRecordService = new MedicalRecordService();
            _userService = new UserService();
            _doctorId = doctorId;

            LoadCustomerComboBox();
        }

        private void LoadCustomerComboBox()
        {
            var customers = _medicalRecordService.GetALlMedicalReccord()
                .Where(r => r.DoctorId == _doctorId)
                .Select(r => r.Customer)
                .Distinct()
                .ToList();

            ChartCustomerComboBox.ItemsSource = customers;
            ChartCustomerComboBox.DisplayMemberPath = "FullName";
            ChartCustomerComboBox.SelectedValuePath = "UserId";
        }

        private void ChartCustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChartCustomerComboBox.SelectedValue is int customerId)
            {
                LoadChartData(customerId);
            }
        }

        private void LoadChartData(int customerId)
        {
            try
            {
                var records = _medicalRecordService.GetALlMedicalReccord()
                    .Where(r => r.DoctorId == _doctorId &&
                                r.CustomerId == customerId &&
                                !string.IsNullOrWhiteSpace(r.TestResults) &&
                                double.TryParse(r.TestResults, out _))
                    .OrderBy(r => r.CreatedDate)
                    .ToList();

                if (!records.Any())
                {
                    MessageBox.Show("No valid test results found for this customer.", "No Data", MessageBoxButton.OK, MessageBoxImage.Information);
                    SeriesCollection = new SeriesCollection();
                    Labels = new List<string>();
                    ChartControl.Series = SeriesCollection;
                    return;
                }

                var grouped = records
                    .GroupBy(r => new { r.CreatedDate.Year, r.CreatedDate.Month })
                    .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                    .ToDictionary(
                        g => $"{g.Key.Month:00}/{g.Key.Year}",
                        g => g.Average(r => double.Parse(r.TestResults))
                    );

                Labels = grouped.Keys.ToList();
                var values = new ChartValues<double>(grouped.Values);

                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Test Results",
                        Values = values,
                        PointGeometrySize = 12
                    }
                };

                ChartControl.Series = SeriesCollection;
                ChartControl.AxisX[0].Labels = Labels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading chart: {ex.Message}", "Chart Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
