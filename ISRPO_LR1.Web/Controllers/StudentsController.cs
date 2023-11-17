using ISRPO_LR1.Domain;
using ISRPO_LR1.Domain.Service;
using ISRPO_LR1.Web.Controllers.Base;
using ISRPO_LR1.Web.Models.ViewModels;
using ISRPO_LR1.Web.Repositories;
using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers;

[Route("students")]
public class StudentsController : BaseController<Student>
{
    public StudentsController(BaseRepository<Student> baseRepository, ILogger<StudentsController> logger) : base(baseRepository, logger)
    {
    }
}