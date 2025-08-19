using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IMedicalRecordRepository
    {
        public List<MedicalRecord> GetALlMedicalReccord();
        public MedicalRecord GetMedicalRecordById(int medicalRecordId);
        public MedicalRecord AddMedicalRecord(MedicalRecord newMedicalRecord);
        public MedicalRecord UpdateMedicalRecordById(int medialRecordId, MedicalRecord newMedicalRecord);
        public bool DeleteMedicalRecordById(int medicalRecordId);
        public List<MedicalRecord> GetAllMedicalOfCustomerId(int customerId);

    }
}
