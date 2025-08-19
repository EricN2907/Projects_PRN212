using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IPatientRequestService
    {
        public List<PatientRequest> GetAllPatientRequests();
        public PatientRequest GetPatientRequestById(int requestId);
        public PatientRequest AddNewPatientRequest(PatientRequest patientRequest);
        public PatientRequest UpdatePantienRequest(int pantientId, PatientRequest newPatientRequest);
        public bool DeletePatientRequest(int requestId);

    }
}
