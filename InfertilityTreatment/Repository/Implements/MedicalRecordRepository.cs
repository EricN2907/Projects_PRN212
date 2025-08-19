using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class MedicalRecordRepository : IMedicalRecordRepository
    {
        private readonly InfertilityTreatmentContext _context;

        public MedicalRecordRepository()
        {
            _context = new InfertilityTreatmentContext();
        }

        public MedicalRecordRepository(InfertilityTreatmentContext context)
        {
            _context = context;
        }

        public List<MedicalRecord> GetALlMedicalReccord()
        {
            return _context.MedicalRecords.Include(m => m.Appointment)
                .Include(m => m.Doctor)
                .Include(m => m.Customer)
                .ToList();
        }

        public MedicalRecord GetMedicalRecordById(int medicalRecordId)
        {
            return _context.MedicalRecords
                .Include(m => m.Appointment)
                .Include(m => m.Doctor)
                .Include(m => m.Customer)
                .FirstOrDefault(m => m.RecordId == medicalRecordId);
        }

        public MedicalRecord AddMedicalRecord(MedicalRecord newMedicalRecord)
        {
            if (newMedicalRecord == null)
            {
                return null;
            }

            _context.MedicalRecords.Add(newMedicalRecord);
            _context.SaveChanges();
            return newMedicalRecord;
        }

        public MedicalRecord UpdateMedicalRecordById(int medialRecordId, MedicalRecord newMedicalRecord)
        {

            var existingMedicalRecord = _context.MedicalRecords
                .FirstOrDefault(m => m.RecordId == medialRecordId);
            if (existingMedicalRecord == null || newMedicalRecord == null)
            {
                return null;
            }
            existingMedicalRecord.AppointmentId = newMedicalRecord.AppointmentId;
            existingMedicalRecord.DoctorId = newMedicalRecord.DoctorId;
            existingMedicalRecord.CustomerId = newMedicalRecord.CustomerId;
            existingMedicalRecord.Prescription = newMedicalRecord.Prescription;
            existingMedicalRecord.TestResults = newMedicalRecord.TestResults;
            existingMedicalRecord.Note = newMedicalRecord.Note;
            existingMedicalRecord.CreatedDate = newMedicalRecord.CreatedDate;
            _context.MedicalRecords.Update(existingMedicalRecord);
            _context.SaveChanges();
            return existingMedicalRecord;
        }

        public bool DeleteMedicalRecordById(int medicalRecordId)
        {
            var existingMedicalRecord = _context.MedicalRecords
                .FirstOrDefault(m => m.RecordId == medicalRecordId);
            if (existingMedicalRecord == null)
            {
                return false;
            }
            _context.MedicalRecords.Remove(existingMedicalRecord);
            _context.SaveChanges();
            return true;
        }

        public List<MedicalRecord> GetAllMedicalOfCustomerId(int customerId)
        {
            return _context.MedicalRecords.Where(m => m.CustomerId == customerId).ToList();
        }
    }
}
