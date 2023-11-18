using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ISRPO_LR1.Domain;

public partial class Student
{
    [Display(Name = "Идентификатор")]
    public int s_id { get; set; }
    [Display(Name = "ФИО")]
    public string s_full_name { get; set; } = null!;
    [Display(Name = "Дата рождения")]
    public DateTime s_birth_date { get; set; }
    [Display(Name = "Электронная почта")]
    public string? s_email { get; set; }
    [JsonPropertyName("grades")]
    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
