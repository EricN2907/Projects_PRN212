using BusinessObject.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly InfertilityTreatmentContext _context;

        public AppointmentRepository()
        {
            _context = new InfertilityTreatmentContext();
        }

        public AppointmentRepository(InfertilityTreatmentContext context)
        {
            _context = context;
        }

        public List<Appointment> GetAllAppointments()
        {
            return _context.Appointments
                .OrderByDescending(a => a.AppointmentDate) // Mới nhất trước
                .ToList();
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            return _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
        }

        public Appointment AddAppointment(Appointment newAppointment)
        {
            if (newAppointment == null)
            {
                return null;
            }
            _context.Appointments.Add(newAppointment);
            _context.SaveChanges();
            return newAppointment;
        }

        public Appointment UpdateAppointmentById(int appointmentId, Appointment newAppointment)
        {

            var existAppointment = GetAppointmentById(appointmentId);
            if (existAppointment == null || newAppointment == null)
            {
                return null;
            }
            existAppointment.CustomerId = newAppointment.CustomerId;
            existAppointment.DoctorId = newAppointment.DoctorId;
            existAppointment.ServiceId = newAppointment.ServiceId;
            existAppointment.AppointmentDate = newAppointment.AppointmentDate;
            existAppointment.Status = newAppointment.Status;
            existAppointment.RejectReason = newAppointment.RejectReason;
            _context.SaveChanges();
            return existAppointment;

        }

        public bool DeleteAppointmentById(int appointmentId)
        {
            var existAppointment = GetAppointmentById(appointmentId);
            if (existAppointment == null)
            {
                return false;
            }
            _context.Appointments.Remove(existAppointment);
            _context.SaveChanges();
            return true;
        }

        public Appointment UpdateAppointmentStatus(int appointmentId, string status, string reson)
        {
            var appointment = GetAppointmentById(appointmentId);
            if (appointment == null)
            {
                return null;
            }
            appointment.Status = status;
            appointment.RejectReason = reson;
            _context.SaveChanges();
            return appointment;
        }

        public List<Appointment> GetAllAppointmentByDoctorId(int doctorId)
        {
            return _context.Appointments.Where(a => a.DoctorId == doctorId && a.Status == "Accepted").ToList();
        }

        public List<Appointment> GetAllAppointmentRejetedByCustomerId(int customerId)
        {
            return _context.Appointments.Where(a => a.CustomerId == customerId && a.Status == "Rejected").ToList();
        }
        public List<Appointment> GetAllAppointmentAcceptedByCustomerId(int customerId)
        {
            return _context.Appointments.Where(a => a.CustomerId == customerId && a.Status == "Accepted").ToList();
        }

        public void AcceptAppointment(int appointmentId, string? note = null)
        {
            var appt = _context.Appointments.Find(appointmentId);
            if (appt == null) throw new Exception("Appointment not found");

            appt.Status = "Scheduled"; // hoặc "Accepted"
            appt.RejectReason = null;

            var serviceName = _context.TreatmentServices
                .Where(x => x.ServiceId == appt.ServiceId)
                .Select(x => x.ServiceName)
                .FirstOrDefault();

            var schedule = new Schedule
            {
                CustomerId = appt.CustomerId,
                DoctorId = appt.DoctorId,
                SerivceName = serviceName,
                ScheduleDate = appt.AppointmentDate,
                Note = note
            };

            _context.Schedules.Add(schedule);
            _context.SaveChanges();
        }
        public void CancelAppointment(int appointmentId)
        {
            var appt = _context.Appointments.FirstOrDefault(a => a.AppointmentId == appointmentId);
            if (appt != null && appt.Status != "Cancelled")
            {
                appt.Status = "Cancelled";
                _context.SaveChanges();
            }
        }

    }
}
