using CathaybkHW.Application.Result;
using CathaybkHW.Application.Services;
using CathaybkHW.Domain.DTOs.Currency;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CathaybkHW.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrencyController : Controller
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 呼叫 coindesk 的 API 組合貨幣名稱
    /// </summary>
    /// <returns></returns>
    [HttpGet("CurrencyRateList")]
    public async Task<IEnumerable<CurrencyInfoResult>> GetCurrencyInfo()
    {
        var query = new CurrencyInfoListQuery();
        var result = await _mediator.Send(query);
        return result;
    }

    /// <summary>
    /// 新增貨幣名稱
    /// </summary>
    /// <param name="currencyName"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateCurrencyName([FromBody] CurrencyName currencyName)
    {
        await _mediator.Send(new CreateCurrencyNameCommand { CurrencyName = currencyName });
        return Ok();
    }

    /// <summary>
    /// 取得貨幣名稱
    /// </summary>
    /// <param name="code"></param>
    /// <param name="language"></param>
    /// <returns></returns>    
    [Authorize]
    [HttpGet("{code}/{language}")]
    public async Task<ActionResult<CurrencyName>> GetCurrencyName(string code, string language)
    {
        var currencyName = await _mediator.Send(new GetCurrencyNameQuery { Code = code, Language = language });
        if (currencyName == null)
            return NotFound();
        return currencyName;
    }

    /// <summary>
    /// 更新貨幣名稱
    /// </summary>
    /// <param name="currencyName"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateCurrencyName([FromBody] CurrencyName currencyName)
    {
        await _mediator.Send(new UpdateCurrencyNameCommand { CurrencyName = currencyName });
        return NoContent();
    }

    /// <summary>
    /// 刪除貨幣名稱
    /// </summary>
    /// <param name="code"></param>
    /// <param name="language"></param>
    /// <returns></returns>
    [Authorize]
    [HttpDelete("{code}/{language}")]
    public async Task<IActionResult> DeleteCurrencyName(string code, string language)
    {
        await _mediator.Send(new DeleteCurrencyNameCommand { Code = code, Language = language });
        return NoContent();
    }
}
