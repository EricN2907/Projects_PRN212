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
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService()
        {
            _scheduleRepository = new ScheduleRepository();
        }

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public Schedule AddNewSchedule(Schedule schedule)
        {
            return _scheduleRepository.AddNewSchedule(schedule);
        }

        public bool DeleteSchedule(int scheduleId)
        {
            return _scheduleRepository.DeleteSchedule(scheduleId);
        }

        public List<Schedule> GetAllSchedule()
        {
            return _scheduleRepository.GetAllSchedule();
        }

        public Schedule GetScheduleById(int scheduleId)
        {
            return _scheduleRepository.GetScheduleById(scheduleId);
        }

        public Schedule UpdateSchedule(Schedule newSchedule, int scheduleId)
        {
            return _scheduleRepository.UpdateSchedule(newSchedule, scheduleId);
        }
        public List<Schedule> GetSchedulesByDoctorId(int doctorId)
        {
            return _scheduleRepository.GetSchedulesByDoctorId(doctorId);
        }
        public bool CheckingisAlreadyBooked(int doctorId, DateTime dateTimeBooking)
        {
            return _scheduleRepository.CheckingisAlreadyBooked(doctorId, dateTimeBooking);
        }
        public List<Schedule> GetSchedulesByCustomerId(int customerId) 
        { 
        return _scheduleRepository.GetSchedulesByCustomerId(customerId);
        }
    }
}
