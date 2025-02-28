using System;
using System.Collections.Generic;

namespace mat_modelirovanije2.Models;

public partial class Education
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TypeId { get; set; }

    public virtual EducationType Type { get; set; } = null!;

    public virtual ICollection<EducationCalendar> Dates { get; set; } = new List<EducationCalendar>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();
}
