using BusinessObject.Models;
using Repository.ResponseModel;
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
    /// Interaction logic for CustomerFeedBackPage.xaml
    /// </summary>
    public partial class CustomerFeedBackPage : Window
    {
        private User CurrentUser { get; set; }
        private readonly int _customerId;
        private readonly IFeedbackService _feedbackService;

        public CustomerFeedBackPage(User user)
        {
            InitializeComponent();
            CurrentUser = user;
            _customerId = CurrentUser.UserId;
            _feedbackService = new FeedbackService();
        }

        private void SubmitFeedback_Click(object sender, RoutedEventArgs e)
        {
            if (RatingComboBox.SelectedItem is ComboBoxItem selectedItem &&
                int.TryParse(selectedItem.Content.ToString(), out int rating))
            {
                string comment = CommentTextBox.Text.Trim();

                var feedback = new CustomerFeedBack
                {
                    CustomerId = _customerId,
                    Rating = rating,
                    Comment = string.IsNullOrWhiteSpace(comment) ? null : comment,
                    CreatedDate = DateTime.Now
                };

                Feedback newFeedBack = new Feedback
                {
                    CustomerId = _customerId,
                    Rating = rating,
                    Comment = comment,
                    CreatedDate = DateTime.Now
                };

                _feedbackService.AddNewFeedBack(newFeedBack);
                // Save to DB or service here (you need to wire this part up)
                MessageBox.Show("Thank you for your feedback!", "Submitted", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please select a rating.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
