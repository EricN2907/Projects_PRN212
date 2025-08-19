using BusinessObject.Models;
using Repository.Implements;
using Repository.Interfaces;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implements
{
   public class FeedbackService : IFeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;
    
       public FeedbackService()
        {
            _feedbackRepository = new FeedbackRepository();
        }

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public Feedback AddNewFeedBack(Feedback newFeedBack)
        {
           return _feedbackRepository.AddNewFeedBack(newFeedBack);
        }

        public bool DeleteFeedbackById(int feedbackId)
        {
           return _feedbackRepository.DeleteFeedbackById(feedbackId);
        }

        public List<Feedback> GetAllFeedbacks()
        {
           return _feedbackRepository.GetAllFeedbacks();
        }

        public Feedback GetFeedbackById(int feedbackId)
        {
            return _feedbackRepository.GetFeedbackById(feedbackId);
        }

        public Feedback UpdateFeedbackById(int feedbackId, Feedback newFeedback)
        {
            return _feedbackRepository.UpdateFeedbackById(feedbackId, newFeedback);
        }
        public List<Feedback> GetAllFeeddBackOfUser(int UserId) { 
        return _feedbackRepository.GetAllFeeddBackOfUser(UserId);
        }
    }
}
