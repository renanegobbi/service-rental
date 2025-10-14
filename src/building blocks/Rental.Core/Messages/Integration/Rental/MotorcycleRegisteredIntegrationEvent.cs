using Rental.Core.Messages.Integration.Common;

namespace Rental.Core.Messages.Integration.Rental
{
    public class MotorcycleRegisteredIntegrationEvent : IntegrationEvent
    {
        public Guid Id { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }

        public MotorcycleRegisteredIntegrationEvent(Guid id, int year, string model, string plate)
        {
            AggregateId = id;
            Id = id;
            Year = year;
            Model = model;
            Plate = plate;
        }
    }
}
