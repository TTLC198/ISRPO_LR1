using ISRPO_LR1.Domain;
using ISRPO_LR1.Web.Controllers.Base;
using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers;

[Route("/grades")]
public class GradesController : BaseController<Grade>
{
    public GradesController(BaseRepository<Grade> baseRepository, Logger<BaseController<Grade>> logger) : base(baseRepository, logger)
    {
    }

    public async Task<IActionResult> Index()
    {
        var response = await BaseRepository.Get();
        if (response.Success)
            ViewBag.Grades = response.Item ?? new List<Grade>();
        return View();
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await BaseRepository.Get();
        if (response.Success)
            ViewBag.Grades = response.Item ?? new List<Grade>();
        return View("Grade");
    }
    
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(Grade model)
    {
        var response = await BaseRepository.Post(model);
        if (response.Success)
            return RedirectToAction("Index");
        return View();
    }
    
    [HttpPut("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, Grade model)
    {
        var response = await BaseRepository.Put(id, model);
        if (response.Success)
            return RedirectToAction("Index");
        return View();
    }
    
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await BaseRepository.Delete(id);
        if (response)
            return RedirectToAction("Index");
        return View();
    }
}