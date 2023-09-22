using System;
using System.Collections.Generic;

namespace ISRPO_LR1.Domain;

public partial class Subject
{
    public int sj_id { get; set; }

    public string sj_name { get; set; } = null!;

    public int? sj_hours { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
