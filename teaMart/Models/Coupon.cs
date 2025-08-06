using System;
using System.Collections.Generic;

namespace teaMart.Models;

public partial class Coupon
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public decimal DiscountAmount { get; set; }

    public string DiscountType { get; set; } = null!;

    public int SumCount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int UsedCount { get; set; }

    public decimal ShouldMoney { get; set; }

    public virtual ICollection<GetCoupon> GetCoupons { get; set; } = new List<GetCoupon>();
}
