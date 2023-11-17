using ISRPO_LR1.Domain;
using ISRPO_LR1.Web.Repositories.Base;

namespace ISRPO_LR1.Web.Repositories;

public class GradesRepository : BaseRepository<Grade>
{
    public GradesRepository(HttpClient httpClient, ILogger<GradesRepository> logger) : base(httpClient, logger)
    {
    }
}