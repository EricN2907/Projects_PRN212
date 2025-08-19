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
    /// Interaction logic for AddNewBlog.xaml
    /// </summary>
    public partial class AddNewBlog : Window
    {
        public User _currentUser { get; set; }
        private readonly IBlogService _blogService;

        public AddNewBlog()
        {
            InitializeComponent();
            _currentUser = null;
            _blogService = new BlogService();
        }
        public AddNewBlog(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _blogService = new BlogService();
        }

        public void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string title = txtTitle.Text;
                string content = txtContent.Text;
                DateTime createDate = DateTime.Now;

                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(content))
                {
                    MessageBox.Show("Title and Content cannot be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Blog newBlog = new Blog
                {
                    Title = title,
                    Content = content,
                    CreatedDate = createDate,
                    UserId = _currentUser.UserId
                };

                _blogService.AddNewBlog(newBlog);
                MessageBox.Show("Blog added successfully!", "Success", MessageBoxButton.OK);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the blog: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtTitle.Clear();
                txtContent.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while cancelling: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Close();
            }
        }
    }
}
