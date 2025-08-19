using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ITreatmentServiceService
    {
        public List<TreatmentService> GetAllTreatmentServices();
        public TreatmentService GetTreatmentServiceById(int serviceId);
        public TreatmentService AddTreatMentService(TreatmentService newTreatmentService);
        public TreatmentService UpdateTreatMentService(int treatMentId, TreatmentService newTreatmentService);
        public bool DeleteTreatmentService(int serviceId);
    }
}
