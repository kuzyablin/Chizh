using System;
using System.Collections.Generic;

namespace Chizh;

public partial class Train
{
    public int Id { get; set; }

    public string? TrTittle { get; set; }

    public string? TrDescription { get; set; }

    public decimal? TrTime { get; set; }

    public virtual ICollection<Poze> IdPozes { get; set; } = new List<Poze>();
}
