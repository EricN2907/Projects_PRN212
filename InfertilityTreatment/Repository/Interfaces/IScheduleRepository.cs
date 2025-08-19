using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IScheduleRepository
    {
        public List<Schedule> GetAllSchedule();
        public Schedule GetScheduleById(int scheduleId);
        public Schedule AddNewSchedule(Schedule schedule);
        public Schedule UpdateSchedule(Schedule newSchedule, int scheduleId);
        public bool DeleteSchedule(int scheduleId);
        public List<Schedule> GetSchedulesByDoctorId(int doctorId);
        public bool CheckingisAlreadyBooked(int doctorId, DateTime dateTimeBooking);
        public List<Schedule> GetSchedulesByCustomerId(int customerId);
    }
}
