using System;
using System.Collections.Generic;

namespace teaMart.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Catename { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
