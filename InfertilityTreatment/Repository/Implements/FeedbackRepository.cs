using BusinessObject.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly InfertilityTreatmentContext _context;

        public FeedbackRepository()
        {
            _context = new InfertilityTreatmentContext();
        }

        public FeedbackRepository(InfertilityTreatmentContext context)
        {
            _context = context;
        }

        public List<Feedback> GetAllFeedbacks()
        {
            return _context.Feedbacks.ToList();
        }

        public Feedback GetFeedbackById(int feedbackId)
        {
            return _context.Feedbacks.FirstOrDefault(f => f.FeedbackId == feedbackId);
        }
        public Feedback AddNewFeedBack(Feedback newFeedBack)
        {
            if (newFeedBack == null)
            {
                return null;
            }
            _context.Feedbacks.Add(newFeedBack);
            _context.SaveChanges();
            return newFeedBack;
        }
        public Feedback UpdateFeedbackById(int feedbackId, Feedback newFeedback)
        {
            var exitFeedback = GetFeedbackById(feedbackId);
            if (exitFeedback == null || newFeedback == null)
            {
                return null;
            }
            exitFeedback.Rating = newFeedback.Rating;
            exitFeedback.Comment = newFeedback.Comment;
            exitFeedback.CreatedDate = DateTime.Now;
            _context.Feedbacks.Update(exitFeedback);
            _context.SaveChanges();
            return exitFeedback;

        }

        public bool DeleteFeedbackById(int feedbackId)
        {
            var exitFeedback = GetFeedbackById(feedbackId);
            if (exitFeedback == null)
            {
                return false;
            }
            _context.Feedbacks.Remove(exitFeedback);
            _context.SaveChanges();
            return true;
        }

        public List<Feedback> GetAllFeeddBackOfUser(int UserId)
        {
            return _context.Feedbacks
                    .Where(f => f.CustomerId == UserId)
                    .ToList();
        }

    }
}
