using ISRPO_LR1.Web.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace ISRPO_LR1.Web.Controllers.Base;

public class BaseController<T> : Controller
{
    protected readonly BaseRepository<T> BaseRepository;
    protected readonly Logger<BaseController<T>> Logger;

    public BaseController(BaseRepository<T> baseRepository, Logger<BaseController<T>> logger)
    {
        BaseRepository = baseRepository;
        Logger = logger;
    }
}