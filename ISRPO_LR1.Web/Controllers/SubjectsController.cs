using ISRPO_LR1.Domain;
using ISRPO_LR1.Web.Controllers.Base;
using ISRPO_LR1.Web.Repositories;
using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers;

[Route("subjects")]
public class SubjectsController : BaseController<Subject>
{
    public SubjectsController(BaseRepository<Subject> baseRepository, ILogger<SubjectsController> logger) : base(baseRepository, logger)
    {
    }
}