using ISRPO_LR1.Domain;
using ISRPO_LR1.Web.Controllers.Base;
using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers;

[Route("grades")]
public class GradesController : BaseController<Grade>
{
    public GradesController(BaseRepository<Grade> baseRepository, Logger<BaseController<Grade>> logger) : base(baseRepository, logger)
    {
    }
}