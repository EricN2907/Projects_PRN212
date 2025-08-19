using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class PatientRequest
{
    public int RequestId { get; set; }

    public int CustomerId { get; set; }

    public int DoctorId { get; set; }

    public int ServiceId { get; set; }

    public string? Note { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual User Doctor { get; set; } = null!;

    public virtual TreatmentService Service { get; set; } = null!;
}
