using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ResponseModel
{
    public class ScheduleDisplay
    {
        public int ScheduleId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public string DayName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Notes { get; set; }
    }
}
