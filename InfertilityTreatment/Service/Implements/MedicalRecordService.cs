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
    public class MedicalRecordService : IMedicalRecordService
    {
        private readonly IMedicalRecordRepository _medicalRecordRepository;

        public MedicalRecordService() {
            _medicalRecordRepository = new MedicalRecordRepository();
        }
        public MedicalRecordService(IMedicalRecordRepository medicalRecordRepository)
        {
            _medicalRecordRepository = medicalRecordRepository;
        }

        public MedicalRecord AddMedicalRecord(MedicalRecord newMedicalRecord)
        {
            return _medicalRecordRepository.AddMedicalRecord(newMedicalRecord);
        }

        public bool DeleteMedicalRecordById(int medicalRecordId)
        {
            return _medicalRecordRepository.DeleteMedicalRecordById(medicalRecordId);
        }

        public List<MedicalRecord> GetALlMedicalReccord()
        {
            return _medicalRecordRepository.GetALlMedicalReccord();
        }

        public MedicalRecord GetMedicalRecordById(int medicalRecordId)
        {
            return _medicalRecordRepository.GetMedicalRecordById(medicalRecordId);
        }

        public MedicalRecord UpdateMedicalRecordById(int medialRecordId, MedicalRecord newMedicalRecord)
        {
            return _medicalRecordRepository.UpdateMedicalRecordById(medialRecordId, newMedicalRecord);
        }
        public List<MedicalRecord> GetAllMedicalOfCustomerId(int customerId) {
            return _medicalRecordRepository.GetAllMedicalOfCustomerId(customerId);
        }
    }
}
