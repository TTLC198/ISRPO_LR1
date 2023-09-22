using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ISRPO_LR1.Domain;

public partial class Subject
{
    public int sj_id { get; set; }

    public string sj_name { get; set; } = null!;

    public int? sj_hours { get; set; }

    [JsonIgnore]
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
