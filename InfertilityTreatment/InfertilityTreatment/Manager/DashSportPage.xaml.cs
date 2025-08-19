using BusinessObject.Models;
using LiveCharts;
using LiveCharts.Wpf;
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
    /// Interaction logic for DashSportPage.xaml
    /// </summary>
    public partial class DashSportPage : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }
        private readonly IAppointmentService _appointmentService;
        public DashSportPage()
        {
            InitializeComponent();
            _appointmentService = new AppointmentService();
            LoadAppointmentData();
        }

        private void LoadAppointmentData()
        {
            var appointments = _appointmentService.GetAllAppointments();

            var grouped = appointments
                .GroupBy(a => new { a.AppointmentDate.Year, a.AppointmentDate.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Month = $"{g.Key.Month:00}/{g.Key.Year}",
                    AcceptedCount = g.Count(a => a.Status == "Accepted"),
                    RejectedCount = g.Count(a => a.Status == "Rejected")
                })
                .ToList();


                        Labels = grouped.Select(g => g.Month).ToList();

                        SeriesCollection = new SeriesCollection
                {
                new ColumnSeries
                {
                    Title = "Accepted",
                    Values = new ChartValues<int>(grouped.Select(g => g.AcceptedCount)),
                    Fill = Brushes.Green
                },
                new ColumnSeries
                {
                    Title = "Rejected",
                    Values = new ChartValues<int>(grouped.Select(g => g.RejectedCount)),
                    Fill = Brushes.Red
                }
                         };

                 ChartControl.Series = SeriesCollection;
                 ChartControl.AxisX[0].Labels = Labels;

        }

    }
}
