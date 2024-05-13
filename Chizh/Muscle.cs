using System;
using System.Collections.Generic;

namespace Chizh;

public partial class Muscle
{
    public int Id { get; set; }

    public string? MuTittle { get; set; }

    public virtual ICollection<Poze> Pozes { get; set; } = new List<Poze>();
}
