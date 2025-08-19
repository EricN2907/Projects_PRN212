using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Blog
{
    public int BlogId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
