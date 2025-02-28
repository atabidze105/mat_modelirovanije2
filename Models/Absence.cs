using System;
using System.Collections.Generic;

namespace mat_modelirovanije2.Models;

public partial class Absence
{
    public int Id { get; set; }

    public DateOnly DateStart { get; set; }

    public DateOnly DateEnd { get; set; }

    public int Type { get; set; }

    public int AbsentEployee { get; set; }

    public int SubstituteEmployee { get; set; }

    public virtual Employee AbsentEployeeNavigation { get; set; } = null!;

    public virtual Employee SubstituteEmployeeNavigation { get; set; } = null!;

    public virtual AbsenceType TypeNavigation { get; set; } = null!;
}
