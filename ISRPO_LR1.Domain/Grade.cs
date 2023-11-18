using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ISRPO_LR1.Domain;

public partial class Grade
{
    [Display(Name = "Идентификатор")]
    public int g_id { get; set; }
    [Display(Name = "Оценка")]
    public int g_value { get; set; }
    [Display(Name = "Идентификатор студента")]
    public int g_s_id { get; set; }
    [Display(Name = "Идентификатор предмета")]
    public int g_sj_id { get; set; }

    public virtual Student? g_s { get; set; }

    public virtual Subject? g_sj { get; set; }
}
