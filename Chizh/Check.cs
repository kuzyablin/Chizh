using System;
using System.Collections.Generic;

namespace Chizh;

public partial class Check
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public int? IdUser { get; set; }

    public decimal? Weight1 { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
