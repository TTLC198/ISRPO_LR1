using ISRPO_LR1.Domain;
using ISRPO_LR1.Web.Controllers.Base;
using ISRPO_LR1.Web.Repositories;
using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers;

[Route("grades")]
public class GradesController : BaseController<Grade>
{
    private readonly GradesRepository _gradesRepository;
    public GradesController(BaseRepository<Grade> baseRepository, ILogger<GradesController> logger, GradesRepository gradesRepository) : base(baseRepository, logger)
    {
        _gradesRepository = gradesRepository;
    }
    
    [HttpGet("create")]
    public virtual IActionResult Create()
    {
        ViewBag.Students
        return View();
    }
    
    [HttpGet("edit/{id:int}")]
    public override async Task<IActionResult> Edit(int id)
    {
        var response = await BaseRepository.Get(id);
        return View(response);
    }
}