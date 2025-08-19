using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IAppointmentRepository
    {
         List<Appointment> GetAllAppointments();
         Appointment GetAppointmentById(int appointmentId);
         Appointment AddAppointment(Appointment newAppointment);
         Appointment UpdateAppointmentById(int appointmentId, Appointment newAppointment);
         bool DeleteAppointmentById(int appointmentId);
         Appointment UpdateAppointmentStatus(int appointmentId, string status, string reson);
         List<Appointment> GetAllAppointmentByDoctorId(int doctorId);
         List<Appointment> GetAllAppointmentRejetedByCustomerId(int customerId);
         List<Appointment> GetAllAppointmentAcceptedByCustomerId(int customerId);
         void AcceptAppointment(int appointmentId, string? note = null);
        void CancelAppointment(int appointmentId); 

    }
}
