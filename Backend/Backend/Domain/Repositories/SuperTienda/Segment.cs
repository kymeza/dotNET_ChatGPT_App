using System;
using System.Collections.Generic;

namespace Backend.Domain.Repositories.SuperTienda;

public partial class Segment
{
    public string IdSegmento { get; set; } = null!;

    public string? Segmento { get; set; }

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
}
