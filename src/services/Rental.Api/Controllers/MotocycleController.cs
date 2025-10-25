//using Microsoft.AspNetCore.Mvc;
//using Rental.Api.Application.Commands.MotocycleCommands;
//using Rental.Core.Mediator;
//using Rental.Services.Controllers;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Rental.Api.Controllers
//{
//    public class MotocycleController : MainController
//    {
//        private readonly IMediatorHandler _mediatorHandler;

//        public MotocycleController(IMediatorHandler mediatorHandler)
//        {
//            _mediatorHandler = mediatorHandler;
//        }

//        [HttpGet("motocycles")]
//        public async Task<IActionResult> Index()
//        {
//            var randomCode = GenerateRandomCode(7);

//            var resultado = await _mediatorHandler.SendCommand(
//                new RegisterMotorcycleCommand(Guid.NewGuid(), 2024, "Yamaha XTZ 150 Crosser", randomCode));

//            return ApiResponse(resultado);
//        }

//        public static string GenerateRandomCode(int length)
//        {
//            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
//            var random = new Random();
//            return new string(Enumerable.Repeat(characters, length)
//                .Select(s => s[random.Next(s.Length)]).ToArray());
//        }
//    }
//}
