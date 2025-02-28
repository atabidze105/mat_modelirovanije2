using System;
using System.Collections.Generic;

namespace mat_modelirovanije2.Models;

public partial class Material
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly DateApproval { get; set; }

    public DateOnly DateEdited { get; set; }

    public string? Domain { get; set; }

    public int Author { get; set; }

    public int TypeId { get; set; }

    public int StatusId { get; set; }

    public virtual Employee AuthorNavigation { get; set; } = null!;

    public virtual MaterialStatus Status { get; set; } = null!;

    public virtual MaterialType Type { get; set; } = null!;

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
}
