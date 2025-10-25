//using FluentValidation.Results;
//using MediatR;
//using Rental.Api.Data.Repositories.Interfaces;
//using Rental.Api.Entities;
//using Rental.Core.Messages;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Rental.Api.Application.Commands.CourierCommands
//{
//    public class CourierCommandHandler : CommandHandler,
//        IRequestHandler<RegisterCourierCommand, ValidationResult>
//    {
//        private readonly ICourierRepository _courierRepository;

//        public CourierCommandHandler(ICourierRepository courierRepository)
//        {
//            _courierRepository = courierRepository;
//        }

//        public async Task<ValidationResult> Handle(RegisterCourierCommand message, CancellationToken cancellationToken)
//        {
//            if (!message.IsValid()) return message.ValidationResult;

//            var courier = new Courier(
//                message.FullName,
//                message.Cnpj,
//                message.BirthDate,
//                message.DriverLicenseNumber,
//                message.DriverLicenseType,
//                message.DriverLicenseImageUrl
//            );

//            _courierRepository.Add(courier);

//            return await PersistData(_courierRepository.UnitOfWork);
//        }
//    }
//}
