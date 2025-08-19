using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int CustomerId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User Customer { get; set; } = null!;
}
