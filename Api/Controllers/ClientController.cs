using Application.Command;
using Application.Handler;
using Domain.Dto.Response;
using Domain.Exception;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "Client")]
    [Authorize]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("RegisterUser")]
        [Authorize]
        [ProducesResponseType(typeof(RegisterClientResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        [ProducesResponseType(typeof(ErrorResponseDto), 500)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterClientCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (DefaultException ex)
            {
                return StatusCode((int)ex.StatusCode, new ErrorResponseDto { DetalheErro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDto { DetalheErro = "Internal server error: " + ex.Message });
            }
        }

        [HttpPost("SimulationCredit")]
        [Authorize]
        [ProducesResponseType(typeof(RegisterClientResponseDto), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        [ProducesResponseType(typeof(ErrorResponseDto), 500)]
        public async Task<IActionResult> SimulationCredit([FromBody] CreditSimulationCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (DefaultException ex)
            {
                return StatusCode((int)ex.StatusCode, new ErrorResponseDto { DetalheErro = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponseDto { DetalheErro = "Internal server error: " + ex.Message });
            }
        }
    }
}
