using CathaybkHW.Application.Result;
using CathaybkHW.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CathaybkHW.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : Controller
    {
        private readonly IMediator _mediator;

        public CurrencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("CurrencyRateList")]
        public async Task<IEnumerable<CurrencyInfoResult>> GetCurrencyInfo()
        {
            var query = new CurrencyInfoListQuery();
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
