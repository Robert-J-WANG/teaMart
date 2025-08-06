using System;
using System.Collections.Generic;

namespace teaMart.Models;

public partial class ProductImage
{
    public int Id { get; set; }

    public int? Pid { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Product? PidNavigation { get; set; }
}
