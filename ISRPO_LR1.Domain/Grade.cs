using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ISRPO_LR1.Domain;

public partial class Grade
{
    public int g_id { get; set; }

    public int g_value { get; set; }

    public int g_s_id { get; set; }

    public int g_sj_id { get; set; }

    [JsonIgnore]
    public virtual Student? g_s { get; set; }

    [JsonIgnore]
    public virtual Subject? g_sj { get; set; }
}
