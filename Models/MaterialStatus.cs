using System;
using System.Collections.Generic;

namespace mat_modelirovanije2.Models;

public partial class MaterialStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
