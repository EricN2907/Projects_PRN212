using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int CustomerId { get; set; }

    public int DoctorId { get; set; }

    public int ServiceId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public string? Status { get; set; }

    public string? RejectReason { get; set; }

    [NotMapped] // Đánh dấu đây là thuộc tính không cần ánh xạ tới cơ sở dữ liệu
    public bool CanCancel
    {
        get
        {
            // Kiểm tra thời gian cuộc hẹn còn hơn 24 giờ và trạng thái không phải là Cancelled hoặc Rejected
            if (AppointmentDate > DateTime.Now.AddHours(24) && Status != "Cancelled" && Status != "Rejected")
            {
                return true;
            }
            return false;
        }
    }

    public string? CancelReason { get; set; }

    public virtual User Customer { get; set; } = null!;

    public virtual User Doctor { get; set; } = null!;

    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; } = new List<MedicalRecord>();

    public virtual TreatmentService Service { get; set; } = null!;
}
