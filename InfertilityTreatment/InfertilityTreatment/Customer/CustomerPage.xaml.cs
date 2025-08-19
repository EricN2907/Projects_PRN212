using BusinessObject.Models;
using InfertilityTreatment.HomePage;
using InfertilityTreatment.InfertilityHomePage;
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
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Window
    {
        public User CurrentUser { get; set; }
        private readonly IBlogService _blogService;
        private readonly IScheduleService _scheduleService;

        public CustomerPage(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            _blogService = new BlogService();
            _scheduleService = new ScheduleService();
            Loaded += CustomerPage_Loaded;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CustomerPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBlogs();                  // Load blogs on page load
            CheckForCurrentSchedule();    // Check schedule and show notification if needed
        }

        private void btProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfilePage profilePage = new ProfilePage(CurrentUser, CurrentUser.RoleId);
            profilePage.ShowDialog();
        }

        private void btBookAppointment_Click(object sender, RoutedEventArgs e)
        {
            BookAppointmentPage bookAppointmentPage = new BookAppointmentPage(CurrentUser);
            bookAppointmentPage.Show();
        }

        private void ContactUs_Click(object sender, RoutedEventArgs e)
        {
            ContactUsPage contactUsPage = new ContactUsPage();
            contactUsPage.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            WebHomePage homePage = new WebHomePage(null);
            homePage.Show();
            this.Close();
        }

        private void btViewAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CustomerAppoimentPage customerAppoimentPage = new CustomerAppoimentPage(CurrentUser);
                customerAppoimentPage.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while viewing appointments: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadBlogs()
        {
            try
            {
                BlogWrapPanel.Children.Clear();
                List<Blog> blogs = _blogService.GetAllBlogs();
                if (blogs.Count > 0)
                {
                    foreach (var blog in blogs)
                    {
                        BlogWrapPanel.Children.Add(CreateBlogCard(blog));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading blogs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Border CreateBlogCard(Blog blog)
        {
            var border = new Border
            {
                Background = Brushes.White,
                Width = 280,
                Height = 160,
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(8),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1)
            };

            var stackPanel = new StackPanel();

            stackPanel.Children.Add(new TextBlock
            {
                Text = "📖 " + blog.Title,
                FontWeight = FontWeights.Bold,
                FontSize = 14
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = blog.Content?.Length > 100 ? blog.Content.Substring(0, 100) + "..." : blog.Content,
                FontSize = 12,
                Margin = new Thickness(0, 5, 0, 0),
                TextWrapping = TextWrapping.Wrap
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = blog.CreatedDate?.ToString("dd-MM-yyyy HH:mm"),
                FontSize = 10,
                Foreground = Brushes.Gray,
                Margin = new Thickness(0, 10, 0, 0)
            });

            border.Child = stackPanel;
            return border;
        }

      

        public void btFeedBack_Click(object sender, RoutedEventArgs e) { 
            CustomerFeedBackPage customerFeedBackPage = new CustomerFeedBackPage(CurrentUser);
            customerFeedBackPage.Show();
        }

        private void CheckForCurrentSchedule()
        {
            var scheduleService = new ScheduleService();
            var schedules = scheduleService.GetAllSchedule()
                .Where(s => s.CustomerId == CurrentUser.UserId && s.ScheduleDate.HasValue)
                .ToList();

            DateTime now = DateTime.Now;

            bool hasTodaySchedule = schedules.Any(s =>
            {
                DateTime scheduleTime = s.ScheduleDate.Value;

                // Notify if the schedule is today, or starting in the next 1 hour
                return scheduleTime.Date == now.Date ||
                       (scheduleTime > now && scheduleTime <= now.AddHours(1));
            });

            if (hasTodaySchedule)
            {
                NotificationDot.Visibility = Visibility.Visible;
                ToolTipService.SetToolTip(NotificationDot, "You have a schedule today or within the next hour.");
            }
            else
            {
                NotificationDot.Visibility = Visibility.Collapsed;
            }
        }

        private void NotificationBell_Click(object sender, RoutedEventArgs e)
        {
            var schedules = _scheduleService.GetAllSchedule()
                .Where(s => s.CustomerId == CurrentUser.UserId &&
                            s.ScheduleDate.HasValue &&
                            s.ScheduleDate.Value.Date == DateTime.Today)
                .ToList();

            if (schedules.Any())
            {
                var msg = string.Join("\n\n", schedules.Select(s =>
                 $"- 📅 Date: {s.ScheduleDate:dd/MM/yyyy hh:mm tt}\n- 🧪 Service: {s.SerivceName ?? "N/A"}"));


                MessageBox.Show($"🔔 You have schedule(s) today:\n\n{msg}", "Today's Schedule", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("✅ You have no schedules today.", "No Schedule", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


    }
}
