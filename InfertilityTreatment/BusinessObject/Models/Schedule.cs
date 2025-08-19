using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Schedule
{
    public int ScheduleId { get; set; }

    public int CustomerId { get; set; }

    public int DoctorId { get; set; }

    public string? SerivceName { get; set; }

    public DateTime? ScheduleDate { get; set; }

    public string? Note { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual User Doctor { get; set; } = null!;
}
