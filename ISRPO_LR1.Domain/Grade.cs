using System;
using System.Collections.Generic;

namespace ISRPO_LR1.Domain;

public partial class Grade
{
    public int g_id { get; set; }

    public int g_value { get; set; }

    public int g_s_id { get; set; }

    public int g_sj_id { get; set; }

    public virtual Student? g_s { get; set; }

    public virtual Subject? g_sj { get; set; }
}
