using CathaybkHW.Infrastructure.Databases;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CathaybkHW.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly CathaybkHWDBContext _dbContext;

        public CurrencyController(CathaybkHWDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        async public Task<IActionResult> List()
        {
            var list = await _dbContext.Currencies
                .Include(e => e.Names)
                .Select(e => new
                {
                    Code = e.Code,
                    Names = e.Names.Select(n => new
                    {
                        Language = n.Language,
                        Name = n.Name
                    })
                })
                .ToArrayAsync();

            return Ok(list);
        }
    }
}
