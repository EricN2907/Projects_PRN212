using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ResponseModel
{
    public class UserProfile
    {
        public string UserName { get; set; } = null!;
        public string? FullName { get; set; }
        public int? Age { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
