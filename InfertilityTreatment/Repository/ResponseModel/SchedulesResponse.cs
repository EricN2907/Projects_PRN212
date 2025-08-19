using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ResponseModel
{
    public class SchedulesResponse
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
    }
}
