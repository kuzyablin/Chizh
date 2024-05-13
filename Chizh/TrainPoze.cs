using System;
using System.Collections.Generic;

namespace Chizh;

public partial class TrainPoze
{
    public int? IdTrain { get; set; }

    public int? IdPoze { get; set; }

    public virtual Poze? IdPozeNavigation { get; set; }

    public virtual Train? IdTrainNavigation { get; set; }
}
