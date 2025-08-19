using BusinessObject.Models;
using LiveCharts;
using LiveCharts.Wpf;
using Service.Implements;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace InfertilityTreatment.Admin
{
    public partial class UserAccessHistoryDashPort : Window
    {
        private readonly IUserService _userService;

        // Binding cho XAML
        public List<string> Labels { get; set; }
        public SeriesCollection SeriesCollection { get; set; }

        public UserAccessHistoryDashPort()
        {
            InitializeComponent();
            _userService = new UserService();

            LoadChartData();

            // Binding dữ liệu ra XAML
            DataContext = this;
        }

        private void LoadChartData()
        {
            try
            {
                var users = _userService.GetAllUser();

                // Lọc & nhóm theo tháng-năm
                var groupedData = users
                    .Where(u => u.StartActiveDate.HasValue)
                    .GroupBy(u => u.StartActiveDate.Value.ToString("yyyy-MM"))
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Tạo nhãn trục X dạng "MMM yyyy"
                Labels = groupedData.Keys
                    .Select(k =>
                    {
                        var date = DateTime.ParseExact(k + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        return date.ToString("MMM yyyy", CultureInfo.InvariantCulture);
                    })
                    .ToList();

                // Series dữ liệu
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Users",
                        Values = new ChartValues<int>(groupedData.Values), // giữ nguyên int
                        Fill = System.Windows.Media.Brushes.SteelBlue,
                        DataLabels = true,
                        LabelPoint = point => point.Y.ToString("N0") // format label trên cột
                    }
                };

                // Cập nhật chart
                UserChart.Series = SeriesCollection;

                // Gán nhãn trục X
                if (UserChart.AxisX.Count > 0)
                    UserChart.AxisX[0].Labels = Labels;

                // Format trục Y để không có số thập phân
                if (UserChart.AxisY.Count > 0)
                    UserChart.AxisY[0].LabelFormatter = value => value.ToString("N0");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading chart: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
