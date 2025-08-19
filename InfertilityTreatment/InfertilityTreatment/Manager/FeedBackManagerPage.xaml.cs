using BusinessObject.Models;
using Repository.ResponseModel;
using Service.Implements;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace InfertilityTreatment.Manager
{
    public partial class FeedBackManagerPage : Window
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IUserService _userService;

        public FeedBackManagerPage(User currentUser)
        {
            InitializeComponent();
            _feedbackService = new FeedbackService();
            _userService = new UserService();

            LoadDoctorFeedback();
            LoadCustomerFeedback();
        }

        private void LoadDoctorFeedback()
        {
            var feedbacks = _feedbackService.GetAllFeedbacks();

            var doctorFeedbacks = feedbacks
                .Where(f => f.Rating == null)
                .Select(f => new
                {
                    DoctorName = _userService.GetUserById(f.CustomerId)?.FullName ?? "Unknown",
                    Comment = f.Comment ?? "",
                    CreatedDate = f.CreatedDate ?? DateTime.MinValue
                })
                .ToList();

            DoctorFeedbackGrid.ItemsSource = doctorFeedbacks;
        }

        private void LoadCustomerFeedback()
        {
            var feedbacks = _feedbackService.GetAllFeedbacks();

            var customerFeedbacks = feedbacks
                .Where(f => f.Rating != null)
                .Select(f => new
                {
                    CustomerName = _userService.GetUserById(f.CustomerId)?.FullName ?? "Unknown",
                    Rating = f.Rating ?? 0,
                    Comment = f.Comment ?? "",
                    CreatedDate = f.CreatedDate ?? DateTime.MinValue
                })
                .ToList();

            CustomerFeedbackGrid.ItemsSource = customerFeedbacks;
        }
    }
}
