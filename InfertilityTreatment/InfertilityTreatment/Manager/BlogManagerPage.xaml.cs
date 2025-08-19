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
    /// Interaction logic for BlogManagerPage.xaml
    /// </summary>
    public partial class BlogManagerPage : Window
    {
        private readonly IBlogService _blogService;
        private Blog selectedBlog;
        private Blog editingBlog;
        public User CurrentUser { get; set; }

        public BlogManagerPage()
        {
            InitializeComponent();
            CurrentUser = null;
            _blogService = new BlogService();
            LoadBlogs();
        }
        public BlogManagerPage(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            _blogService = new BlogService();
            LoadBlogs();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            AddNewBlog addNewBlog = new AddNewBlog(CurrentUser);
            addNewBlog.ShowDialog();

            LoadBlogs();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (selectedBlog == null)
                {
                    MessageBox.Show("Please select a blog to edit.");
                    return;
                }

                editingBlog = selectedBlog;
                LoadBlogs();
             
            }
            catch (Exception ex) { 
            MessageBox.Show($"An error occurred while updating the blog: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (selectedBlog == null)
                {
                    MessageBox.Show("Please select a blog to delete.");
                    return;
                }

                var result = MessageBox.Show($"Are you sure you want to delete '{selectedBlog.Title}'?",
                                             "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _blogService.DeleteBlog(selectedBlog.BlogId);

                    LoadBlogs(); // reload blog list after deletion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting the blog: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string searchTerm = SearchTextBox.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    MessageBox.Show("Please enter a search term.");
                    return;
                }

                List<Blog> filteredBlogs = _blogService.GetAllBlogs()
                    .Where(b => (!string.IsNullOrEmpty(b.Title) && b.Title.ToLower().Contains(searchTerm)) ||
                                (!string.IsNullOrEmpty(b.Content) && b.Content.ToLower().Contains(searchTerm)))
                    .ToList();

                BlogWrapPanel.Children.Clear();

                if (!filteredBlogs.Any())
                {
                    MessageBox.Show("No blogs matched your search.", "Not Found", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                foreach (var blog in filteredBlogs)
                {
                    BlogWrapPanel.Children.Add(CreateBlogCard(blog));
                }

                selectedBlog = null;
                editingBlog = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while searching blogs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchPlaceholder.Visibility = string.IsNullOrEmpty(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search_Click(sender, e);
            }
        }

        private void BackToManager_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                Height = 200,
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                CornerRadius = new CornerRadius(8),
                BorderBrush = Brushes.LightGray,
                BorderThickness = new Thickness(1),
                Cursor = Cursors.Hand
            };

            if (editingBlog != null && editingBlog.BlogId == blog.BlogId)
            {
                // EDITING MODE
                var editPanel = new StackPanel();

                var titleBox = new TextBox
                {
                    Text = blog.Title,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    Margin = new Thickness(0, 0, 0, 5)
                };

                var contentBox = new TextBox
                {
                    Text = blog.Content,
                    AcceptsReturn = true,
                    TextWrapping = TextWrapping.Wrap,
                    Height = 60
                };

                var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 10, 0, 0) };

                var saveBtn = new Button { Content = "💾 Save", Width = 80, Margin = new Thickness(5, 0, 5, 0) };
                var cancelBtn = new Button { Content = "❌ Cancel", Width = 80 };

                saveBtn.Click += (s, e) =>
                {
                    blog.Title = titleBox.Text;
                    blog.Content = contentBox.Text;
                    _blogService.UpdateBlog(blog.BlogId, blog);
                    editingBlog = null;
                    LoadBlogs();
                    MessageBox.Show("Blog updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                };

                cancelBtn.Click += (s, e) =>
                {
                    editingBlog = null;
                    LoadBlogs();
                };

                buttonPanel.Children.Add(saveBtn);
                buttonPanel.Children.Add(cancelBtn);

                editPanel.Children.Add(titleBox);
                editPanel.Children.Add(contentBox);
                editPanel.Children.Add(buttonPanel);

                border.Child = editPanel;
            }
            else
            {
                // NORMAL DISPLAY MODE
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

                border.MouseLeftButtonDown += (s, e) =>
                {
                    selectedBlog = blog;
                    HighlightSelectedCard(border);
                };

                border.Child = stackPanel;
            }

            return border;
        }
        private void HighlightSelectedCard(Border selectedBorder)
        {
            foreach (Border border in BlogWrapPanel.Children)
            {
                border.BorderBrush = Brushes.LightGray;
            }
            selectedBorder.BorderBrush = Brushes.Blue;
            selectedBorder.BorderThickness = new Thickness(2);
        }
    }
}
