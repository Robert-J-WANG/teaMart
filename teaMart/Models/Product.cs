using System;
using System.Collections.Generic;

namespace teaMart.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Cid { get; set; }

    public decimal Price { get; set; }

    public decimal SalePrice { get; set; }

    public int Number { get; set; }

    public string Detail { get; set; } = null!;

    public string Img { get; set; } = null!;

    public short State { get; set; }

    public DateTime Createtime { get; set; }

    public byte Score { get; set; }

    public decimal Postage { get; set; }

    public virtual ICollection<ApplyReturn> ApplyReturns { get; set; } = new List<ApplyReturn>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual Category CidNavigation { get; set; } = null!;

    public virtual ICollection<OrderComment> OrderComments { get; set; } = new List<OrderComment>();

    public virtual ICollection<OrdersDetail> OrdersDetails { get; set; } = new List<OrdersDetail>();

    public virtual ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();

    public virtual ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();

    public virtual ICollection<ProductSave> ProductSaves { get; set; } = new List<ProductSave>();
}
