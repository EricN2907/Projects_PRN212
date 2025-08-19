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
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;


        public AppointmentService()
        {
            _appointmentRepository = new AppointmentRepository();
        }
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public Appointment AddAppointment(Appointment newAppointment)
        {
            return _appointmentRepository.AddAppointment(newAppointment);
        }

        public bool DeleteAppointmentById(int appointmentId)
        {
            return _appointmentRepository.DeleteAppointmentById(appointmentId);
        }

        public List<Appointment> GetAllAppointments()
        {
            return _appointmentRepository.GetAllAppointments();
        }

        public Appointment GetAppointmentById(int appointmentId)
        {
            return _appointmentRepository.GetAppointmentById(appointmentId);
        }

        public Appointment UpdateAppointmentById(int appointmentId, Appointment newAppointment)
        {
            return _appointmentRepository.UpdateAppointmentById(appointmentId, newAppointment);
        }

        public Appointment UpdateAppointmentStatus(int appointmentId, string status, string reson)
        {
            return _appointmentRepository.UpdateAppointmentStatus(appointmentId, status, reson);
        }
        public List<Appointment> GetAllAppointmentByDoctorId(int doctorId)
        {
            return _appointmentRepository.GetAllAppointmentByDoctorId(doctorId);
        }
        public List<Appointment> GetAllAppointmentRejetedByCustomerId(int customerId)
        {
            return _appointmentRepository.GetAllAppointmentRejetedByCustomerId(customerId);
        }
        public List<Appointment> GetAllAppointmentAcceptedByCustomerId(int customerId)
        {
            return _appointmentRepository.GetAllAppointmentAcceptedByCustomerId(customerId);
        }
        public void AcceptAppointment(int appointmentId, string? note = null)
        {
             _appointmentRepository.AcceptAppointment(appointmentId, note);   
        }
        public void CancelAppointment(int appointmentId)
        {
            _appointmentRepository.CancelAppointment(appointmentId);
        }

    }
}
