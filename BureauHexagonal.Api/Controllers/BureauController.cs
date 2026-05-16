using BureauHexagonal.Application.Dtos.Inputs;
using BureauHexagonal.Application.UseCases.CEP;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BureauHexagonal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchCepRequest(ILogger<SearchCepRequest> _logger) : ControllerBase
    {
        [HttpGet("/v1/cep/{zipcode}/{providerType:int}")]
        public async Task<IActionResult> GetCep([FromRoute(Name = "zipcode")] string zipCode,
                                                [FromRoute(Name = "providerType")] ProviderType providerType,
                                                [FromServices] IBureauCepUseCase useCase)
        {
            var result = await useCase.ExecAsync(new BureauInputDto(zipCode, providerType, BureauType.CEP));
            
            if (result.IsFail())
                return BadRequest(result);

            return Ok(result);
        }
    }
}
