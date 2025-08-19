using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class MedicalRecord
{
    public int RecordId { get; set; }

    public int AppointmentId { get; set; }

    public int DoctorId { get; set; }

    public int CustomerId { get; set; }

    public string? Prescription { get; set; }

    public string? TestResults { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual Appointment Appointment { get; set; } = null!;

    public virtual User Customer { get; set; } = null!;

    public virtual User Doctor { get; set; } = null!;
}
