using ISRPO_LR1.Domain.Service;
using ISRPO_LR1.Web.Models.ViewModels;
using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers.Base;

public abstract class BaseController<T> : Controller
{
    protected readonly BaseRepository<T> BaseRepository;
    protected readonly Logger<BaseController<T>> Logger;

    public BaseController(BaseRepository<T> baseRepository, Logger<BaseController<T>> logger)
    {
        BaseRepository = baseRepository;
        Logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        var response = await BaseRepository.Get();
        if (!response.Success)
            return View(new ServiceResponseModel<BaseIndexViewModel<T>>()
            {
                Error = response.Error
            });
        return View(new ServiceResponseModel<BaseIndexViewModel<T>>()
        {
            Success = true,
            Item = new BaseIndexViewModel<T>()
            {
                Items = response.Item ?? new List<T>()
            }
        });
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var response = await BaseRepository.Get(id);
        return View(response);
    }
    
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(T model)
    {
        var response = await BaseRepository.Post(model);
        if (response.Success)
            return RedirectToAction("Index");
        return View();
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var response = await BaseRepository.Get(id);
        return View(response);
    }
    
    [HttpPut("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id, T model)
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
        return View("Get");
    }
}