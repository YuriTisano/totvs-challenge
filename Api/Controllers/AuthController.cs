using Application.Command;
using Application.Handler;
using Domain.Exception;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "Auth")]

    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("RegisterUser")]
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserCommand command)
        {
            try
            {
                await _mediator.Send(command);
                return Ok();
            }
            catch (DefaultException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Domain.Dto.AuthResponseDto), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Auth([FromBody] AuthCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (DefaultException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
