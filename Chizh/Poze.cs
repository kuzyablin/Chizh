using System;
using System.Collections.Generic;

namespace Chizh;

public partial class Poze
{
    public int Id { get; set; }

    public string? Tittle { get; set; }

    public byte[]? Image { get; set; }

    public decimal? Time { get; set; }

    public string? Description { get; set; }

    public int? IdMuscle { get; set; }

    public virtual Muscle? IdMuscleNavigation { get; set; }

    public virtual ICollection<Train> IdTrains { get; set; } = new List<Train>();
}
