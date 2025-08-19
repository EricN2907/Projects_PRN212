using BusinessObject.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class PatientRequestRepository : IPatientRequestRepository
    {
        private readonly InfertilityTreatmentContext _context;

        public PatientRequestRepository()
        {
            _context = new InfertilityTreatmentContext();
        }

        public PatientRequestRepository(InfertilityTreatmentContext context)
        {
            _context = context;
        }

        public List<PatientRequest> GetAllPatientRequests()
        {
            return _context.PatientRequests.ToList();
        }

        public PatientRequest GetPatientRequestById(int requestId)
        {
            return _context.PatientRequests.FirstOrDefault(pr => pr.RequestId == requestId);
        }

        public PatientRequest AddNewPatientRequest(PatientRequest patientRequest)
        {
            if (patientRequest == null)
            {
                return null;
            }

            _context.PatientRequests.Add(patientRequest);
            _context.SaveChanges();
            return patientRequest;
        }

        public PatientRequest UpdatePantienRequest(int pantientId, PatientRequest newPatientRequest)
        {
            var exitPatientRequest = GetPatientRequestById(pantientId);

            if (exitPatientRequest == null)
            {
                return null;
            }
            exitPatientRequest.CustomerId = newPatientRequest.CustomerId;
            exitPatientRequest.DoctorId = newPatientRequest.DoctorId;
            exitPatientRequest.ServiceId = newPatientRequest.ServiceId;
            exitPatientRequest.Note = newPatientRequest.Note;
            exitPatientRequest.RequestedDate = newPatientRequest.RequestedDate;
            exitPatientRequest.CreatedDate = newPatientRequest.CreatedDate;
            _context.SaveChanges();
            return exitPatientRequest;
        }

        public bool DeletePatientRequest(int requestId)
        {
            var exitPatientRequest = GetPatientRequestById(requestId);
            if (exitPatientRequest == null)
            {
                return false;
            }

            _context.PatientRequests.Remove(exitPatientRequest);
            _context.SaveChanges();
            return true;
        }
    }
}
