using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ISRPO_LR1.Domain;

public partial class Subject
{
    [Display(Name = "Идентификатор")]
    public int sj_id { get; set; }
    [Display(Name = "Название")]
    public string sj_name { get; set; } = null!;
    [Display(Name = "Часы в семестре")]
    public int? sj_hours { get; set; }

    [JsonIgnore]
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
