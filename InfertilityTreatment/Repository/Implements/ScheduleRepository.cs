using BusinessObject.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly InfertilityTreatmentContext _context;

        public ScheduleRepository()
        {
            _context = new InfertilityTreatmentContext();
        }

        public ScheduleRepository(InfertilityTreatmentContext context)
        {
            _context = context;
        }

        public List<Schedule> GetAllSchedule()
        {
            return _context.Schedules.ToList();
        }

        public Schedule GetScheduleById(int scheduleId)
        {
            return _context.Schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);
        }

        public Schedule AddNewSchedule(Schedule schedule)
        {
            if (schedule == null)
            {
                return null;
            }

            _context.Schedules.Add(schedule);
            _context.SaveChanges();
            return schedule;
        }

        public Schedule UpdateSchedule(Schedule newSchedule, int scheduleId)
        {
            var exitSchedule = GetScheduleById(scheduleId);

            if (exitSchedule == null)
            {
                return null;
            }
            exitSchedule.CustomerId = newSchedule.CustomerId;
            exitSchedule.DoctorId = newSchedule.DoctorId;
            exitSchedule.SerivceName = newSchedule.SerivceName;
            exitSchedule.ScheduleDate = newSchedule.ScheduleDate;
            exitSchedule.Note = newSchedule.Note;

            _context.SaveChanges();
            return exitSchedule;
        }

        public bool DeleteSchedule(int scheduleId)
        {
            var exitSchedule = GetScheduleById(scheduleId);
            if (exitSchedule == null)
            {
                return false;
            }

            _context.Schedules.Remove(exitSchedule);
            _context.SaveChanges();
            return true;
        }

        public List<Schedule> GetSchedulesByDoctorId(int doctorId)
        {
            return _context.Schedules.Where(s => s.DoctorId == doctorId).ToList();
        }

        public bool CheckingisAlreadyBooked(int doctorId, DateTime dateTimeBooking)
        {
            return _context.Schedules.Any(s => s.DoctorId == doctorId && s.ScheduleDate == dateTimeBooking);
        }

        public List<Schedule> GetSchedulesByCustomerId(int customerId)
        {
            return _context.Schedules.Where(s => s.CustomerId == customerId).ToList();
        }
    }
}
