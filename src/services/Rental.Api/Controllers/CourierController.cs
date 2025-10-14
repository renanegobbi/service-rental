using Microsoft.AspNetCore.Mvc;
using Rental.Api.Application.Commands.CourierCommands;
using Rental.Api.Application.DTOs.Courier;
using Rental.Core.Mediator;
using Rental.Services.Controllers;

namespace Rental.Api.Controllers
{
    public class CourierController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;
        public CourierController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpPost("new-courier")]
        public async Task<IActionResult> RegisterCourier([FromBody] RegisterCourierRequest request)
        {
            var courierCommand = new RegisterCourierCommand(request);
            var result = await _mediatorHandler.SendCommand(courierCommand);
;
            return CustomResponse();
        }
    }
}
