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
    public class TreatmentServiceService : ITreatmentServiceService
    {
        private readonly ITreatmentServiceRepository _treatmentServiceRepository;

        public TreatmentServiceService()
        {
            _treatmentServiceRepository = new TreatmentServiceRepository();
        }

        public TreatmentServiceService(ITreatmentServiceRepository treatmentServiceRepository)
        {
            _treatmentServiceRepository = treatmentServiceRepository;
        }

        public TreatmentService AddTreatMentService(TreatmentService newTreatmentService)
        {
           return _treatmentServiceRepository.AddTreatMentService(newTreatmentService);
        }

        public bool DeleteTreatmentService(int serviceId)
        {
            return _treatmentServiceRepository.DeleteTreatmentService(serviceId);
        }

        public List<TreatmentService> GetAllTreatmentServices()
        {
           return _treatmentServiceRepository.GetAllTreatmentServices();
        }

        public TreatmentService GetTreatmentServiceById(int serviceId)
        {
            return _treatmentServiceRepository.GetTreatmentServiceById(serviceId);
        }

        public TreatmentService UpdateTreatMentService(int treatMentId, TreatmentService newTreatmentService)
        {
            return _treatmentServiceRepository.UpdateTreatMentService(treatMentId, newTreatmentService);
        }
    }
}
