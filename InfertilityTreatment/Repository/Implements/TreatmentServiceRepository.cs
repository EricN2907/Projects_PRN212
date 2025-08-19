using BusinessObject.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implements
{
    public class TreatmentServiceRepository : ITreatmentServiceRepository
    {
        private readonly InfertilityTreatmentContext _context;

        public TreatmentServiceRepository()
        {
            _context = new InfertilityTreatmentContext();
        }   

        public TreatmentServiceRepository(InfertilityTreatmentContext context)
        {
            _context = context;
        }

        public List<TreatmentService> GetAllTreatmentServices()
        {
          List<TreatmentService> treatmentServices  = _context.TreatmentServices.ToList();

            if (treatmentServices.Count() == 0)
            {
                return null;
            }
            return treatmentServices;
        }

        public TreatmentService GetTreatmentServiceById(int serviceId) { 
           TreatmentService treatmentService = _context.TreatmentServices
                .FirstOrDefault(ts => ts.ServiceId == serviceId);

            if (treatmentService == null)
            {
                return null;
            }
            return treatmentService;
        }

        public TreatmentService AddTreatMentService(TreatmentService newTreatmentService) {
            if (newTreatmentService == null) {
                return null;
            }
            _context.TreatmentServices.Add(newTreatmentService);
            _context.SaveChanges();
            return newTreatmentService;
        }

        public TreatmentService UpdateTreatMentService(int treatMentId , TreatmentService newTreatmentService) { 
            var exitTreatmentService = _context.TreatmentServices.FirstOrDefault(ts => ts.ServiceId == treatMentId);
            if (exitTreatmentService == null)
            {
                return null;
            }
            exitTreatmentService.ServiceName = newTreatmentService.ServiceName;
            exitTreatmentService.Description = newTreatmentService.Description;
            exitTreatmentService.Price = newTreatmentService.Price;
            _context.SaveChanges();
            return exitTreatmentService;
        }

        public bool DeleteTreatmentService(int serviceId)
        {
            var treatmentService = _context.TreatmentServices.FirstOrDefault(ts => ts.ServiceId == serviceId);
            if (treatmentService == null)
            {
                return false;
            }
            _context.TreatmentServices.Remove(treatmentService);
            _context.SaveChanges();
            return true;
        }
    }
}
