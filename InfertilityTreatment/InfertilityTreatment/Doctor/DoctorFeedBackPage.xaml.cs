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

namespace InfertilityTreatment.Doctor
{
    /// <summary>
    /// Interaction logic for DoctorFeedBackPage.xaml
    /// </summary>
    public partial class DoctorFeedBackPage : Window
    {
        private User currentUser { get; set; }
        private readonly int _doctorId;
        private readonly IFeedbackService _feedbackService;

        public DoctorFeedBackPage(User user )
        {
            InitializeComponent();
            _feedbackService =new FeedbackService();
            currentUser = user;
            _doctorId = currentUser.UserId; // Assuming UserId is the doctor's ID
        }

        private void SubmitFeedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string comment = CommentTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(comment))
                {
                    MessageBox.Show("Please enter a comment before submitting.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var feedback = new Feedback
                {
                    CustomerId = _doctorId,
                    Comment = comment,
                    CreatedDate = DateTime.Now
                };

                _feedbackService.AddNewFeedBack(feedback);
                MessageBox.Show("Feedback submitted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting feedback: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
