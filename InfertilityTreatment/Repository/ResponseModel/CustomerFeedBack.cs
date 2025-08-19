using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ResponseModel
{
    public class CustomerFeedBack
    {
        public int CustomerId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
