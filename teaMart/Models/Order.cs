using System;
using System.Collections.Generic;

namespace teaMart.Models;

public partial class Order
{
    public int Id { get; set; }

    public int Uid { get; set; }

    public string OrderNum { get; set; } = null!;

    public decimal SumPrice { get; set; }

    public string Mark { get; set; } = null!;

    public DateTime Createtime { get; set; }

    public short IsPay { get; set; }

    public short State { get; set; }

    public string PayWay { get; set; } = null!;

    public int AddressId { get; set; }

    public decimal DecMoney { get; set; }

    public int CouponId { get; set; }

    public string? ExpressName { get; set; }

    public string? ExpressNumber { get; set; }

    public virtual ICollection<ApplyReturn> ApplyReturns { get; set; } = new List<ApplyReturn>();

    public virtual ICollection<OrderComment> OrderComments { get; set; } = new List<OrderComment>();

    public virtual ICollection<OrdersDetail> OrdersDetails { get; set; } = new List<OrdersDetail>();

    public virtual User UidNavigation { get; set; } = null!;
}
