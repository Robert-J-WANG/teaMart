﻿using System;
using System.Collections.Generic;

namespace teaMart.Models;

public partial class ProductSave
{
    public int Id { get; set; }

    public int? Uid { get; set; }

    public int? Pid { get; set; }

    public DateTime? Createtime { get; set; }

    public virtual Product? PidNavigation { get; set; }

    public virtual User? UidNavigation { get; set; }
}
