using System;
using System.Collections.Generic;

namespace mat_modelirovanije2.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime DatetimeStart { get; set; }

    public DateTime DatetimeEnd { get; set; }

    public string? Description { get; set; }

    public int IdType { get; set; }

    public int IdStatus { get; set; }

    public int IdOrganisator { get; set; }

    public virtual Employee IdOrganisatorNavigation { get; set; } = null!;

    public virtual EventStatus IdStatusNavigation { get; set; } = null!;

    public virtual EventType IdTypeNavigation { get; set; } = null!;
}
