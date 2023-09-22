using System;
using System.Collections.Generic;

namespace ISRPO_LR1.Domain;

public partial class Student
{
    public int s_id { get; set; }

    public string s_full_name { get; set; } = null!;

    public DateTime s_birth_date { get; set; }

    public string? s_email { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
