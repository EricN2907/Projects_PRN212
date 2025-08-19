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
    public class PatientRequestService : IPatientRequestService
    {
        private readonly IPatientRequestRepository _patientRequestRepository;

        public PatientRequestService()
        {
            _patientRequestRepository = new PatientRequestRepository();
        }
        public PatientRequestService(IPatientRequestRepository patientRequestRepository)
        {
            _patientRequestRepository = patientRequestRepository;
        }

        public PatientRequest AddNewPatientRequest(PatientRequest patientRequest)
        {
         return _patientRequestRepository.AddNewPatientRequest(patientRequest);
        }

        public bool DeletePatientRequest(int requestId)
        {
            return _patientRequestRepository.DeletePatientRequest(requestId);
        }

        public List<PatientRequest> GetAllPatientRequests()
        {
          return _patientRequestRepository.GetAllPatientRequests();
        }

        public PatientRequest GetPatientRequestById(int requestId)
        {
         return _patientRequestRepository.GetPatientRequestById(requestId);
        }

        public PatientRequest UpdatePantienRequest(int pantientId, PatientRequest newPatientRequest)
        {
            return _patientRequestRepository.UpdatePantienRequest(pantientId, newPatientRequest);   
        }
    }
}
