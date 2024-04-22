using CathaybkHW.Application.Result;
using CathaybkHW.Application.Services;
using CathaybkHW.Domain.DTOs.Currency;
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

        [HttpPost]
        public async Task<ActionResult> CreateCurrencyName([FromBody] CurrencyName currencyName)
        {
            await _mediator.Send(new CreateCurrencyNameCommand { CurrencyName = currencyName });
            return Ok();
        }

        [HttpGet("{code}/{language}")]
        public async Task<ActionResult<CurrencyName>> GetCurrencyName(string code, string language)
        {
            var currencyName = await _mediator.Send(new GetCurrencyNameQuery { Code = code, Language = language });
            if (currencyName == null)
                return NotFound();
            return currencyName;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCurrencyName([FromBody] CurrencyName currencyName)
        {
            await _mediator.Send(new UpdateCurrencyNameCommand { CurrencyName = currencyName });
            return NoContent();
        }

        [HttpDelete("{code}/{language}")]
        public async Task<IActionResult> DeleteCurrencyName(string code, string language)
        {
            await _mediator.Send(new DeleteCurrencyNameCommand { Code = code, Language = language });
            return NoContent();
        }
    }
}
