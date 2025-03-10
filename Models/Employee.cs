using System;
using System.Collections.Generic;

namespace mat_modelirovanije2.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Lastname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly Birthday { get; set; }

    public string Phone { get; set; } = null!;

    public string Room { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int JobId { get; set; }

    public int DepartmentId { get; set; }

    public string? PhonePrivate { get; set; }

    public int? Supervisor { get; set; }

    public int? Assistant { get; set; }

    public virtual ICollection<Absence> AbsenceAbsentEployeeNavigations { get; set; } = new List<Absence>();

    public virtual ICollection<Absence> AbsenceSubstituteEmployeeNavigations { get; set; } = new List<Absence>();

    public virtual Employee? AssistantNavigation { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Employee> InverseAssistantNavigation { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> InverseSupervisorNavigation { get; set; } = new List<Employee>();

    public virtual Job Job { get; set; } = null!;

    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    public virtual Employee? SupervisorNavigation { get; set; }

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();

    public virtual ICollection<Workingcalendar> IdWorkingcalendars { get; set; } = new List<Workingcalendar>();

    public string BirthdayDate => Birthday.Month switch
    {
        1 => $"{Birthday.Day} января",
        2 => $"{Birthday.Day} февраля",
        3 => $"{Birthday.Day} марта",
        4 => $"{Birthday.Day} апреля",
        5 => $"{Birthday.Day} мая",
        6 => $"{Birthday.Day} июня",
        7 => $"{Birthday.Day} июля",
        8 => $"{Birthday.Day} августа",
        9 => $"{Birthday.Day} сентября",
        10 => $"{Birthday.Day} октября",
        11 => $"{Birthday.Day} ноября",
        12 => $"{Birthday.Day} декабря",
        _ => "Исключение"
    };


    public string VCard => 
        $"BEGIN:VCARD\n" +
        $"VERSION:3.0\n" +
        $"N:{Name}\n" +
        $"FN:{Lastname}\n" +
        $"ORG:ГК Дороги России\n" +
        $"TITLE:{Job.Name}\n" +
        $"TEL;WORK;VOICE:{Phone.Replace("(", "").Replace(")", "").Replace("-", "")}\n" +
        $"TEL;CELL:{  (PhonePrivate == null ? string.Empty : PhonePrivate.Replace("(", "").Replace(")", "").Replace("-", ""))  }\n" +
        $"EMAIL;WORK;INTERNET:{Email}\n" +
        $"END:VCARD";

}
