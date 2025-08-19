using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class RoleType
{
    public int Role { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
