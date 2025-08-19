using BusinessObject.Models;
using InfertilityTreatment.Customer;
using InfertilityTreatment.HomePage;
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

namespace InfertilityTreatment.InfertilityHomePage
{
    /// <summary>
    /// Interaction logic for WebHomePage.xaml
    /// </summary>
    public partial class WebHomePage : Window
    {
        public User currentUser { get; set; }
        private readonly IBlogService _blogService;

        public WebHomePage()
        {
            InitializeComponent();
            currentUser = null;
            _blogService = new BlogService();
            Loaded += CustomerPage_Loaded;
        }

        public WebHomePage(User user)
        {
            InitializeComponent();
            currentUser = user;
            _blogService = new BlogService();
            Loaded += CustomerPage_Loaded;
        }

        private void btContactUs_Click(object sender, RoutedEventArgs e)
        {
            ContactUsPage contactUsPage = new ContactUsPage();
            contactUsPage.Show();
        }

        private void btAboutUs_Click(object sender, RoutedEventArgs e)
        {
            AboutUsPage aboutUsPage = new AboutUsPage();
            aboutUsPage.Show();
        }

        private void btBookAppointment_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser == null || currentUser.RoleId != 4) // only customer
            {
                MessageBox.Show("Please log in or register to book an appointment.",
                                "Login Required",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);

                // Redirect to Login Page
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            

        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
        private void CustomerPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadBlogs(); // Automatically load blogs when the page opens
        }
        public void LoadBlogs()
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
            // "Read More" button
            var readMoreButton = new Button
            {
                Content = "Read More",
                Background = new SolidColorBrush(Color.FromRgb(58, 110, 165)), // same blue as header
                Foreground = Brushes.White,
                FontSize = 12,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 10, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(10, 4, 10, 4),
                Cursor = Cursors.Hand
            };

            // Show full blog in a MessageBox (you can replace this with a custom BlogDetailsWindow)
            readMoreButton.Click += (s, e) =>
            {
                MessageBox.Show(blog.Content, blog.Title, MessageBoxButton.OK, MessageBoxImage.Information);
            };

            stackPanel.Children.Add(readMoreButton);

            border.Child = stackPanel;
            return border;
        }
    }
}
