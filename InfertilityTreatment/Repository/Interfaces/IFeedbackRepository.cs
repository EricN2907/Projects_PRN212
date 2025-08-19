using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IFeedbackRepository
    {
        public List<Feedback> GetAllFeedbacks();
        public Feedback GetFeedbackById(int feedbackId);
        public Feedback AddNewFeedBack(Feedback newFeedBack);
        public Feedback UpdateFeedbackById(int feedbackId, Feedback newFeedback);
        public bool DeleteFeedbackById(int feedbackId);

        public List<Feedback> GetAllFeeddBackOfUser(int UserId);

    }
}
