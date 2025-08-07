using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace teaMart.Models;



public partial class User
{
    public int Id { get; set; }

    public string Phone { get; set; } = null!;

    public string Pwd { get; set; } = null!;

    public string Nickname { get; set; } = null!;

    public string? Sex { get; set; }

    public string? Introduce { get; set; }

    public int? Age { get; set; }

    public string? Img { get; set; }

    public string? Mibao { get; set; }

    public short? Role { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<ApplyReturn> ApplyReturns { get; set; } = new List<ApplyReturn>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<GetCoupon> GetCoupons { get; set; } = new List<GetCoupon>();

    public virtual ICollection<OrderComment> OrderComments { get; set; } = new List<OrderComment>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<ProductSave> ProductSaves { get; set; } = new List<ProductSave>();
}
