using BureauHexagonal.Application.Dtos.Inputs;
using BureauHexagonal.Application.UseCases.CEP;
using BureauHexagonal.Core.Common.Operation;
using BureauHexagonal.Core.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BureauHexagonal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BureauController(ILogger<BureauController> _logger) : ControllerBase
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

        [HttpGet("/v1/cnpj/{zipcode}/{providerType:int}")]
        public async Task<IActionResult> GetCnpj([FromRoute(Name = "zipcode")] string zipCode,
                                                [FromRoute(Name = "providerType")] ProviderType providerType,
                                                [FromServices] IBureauCepUseCase useCase)
        {
            throw new NotImplementedException();
        }

        [HttpGet("/v1/cpf/{zipcode}/{providerType:int}")]
        public async Task<IActionResult> GetCpf([FromRoute(Name = "zipcode")] string zipCode,
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
