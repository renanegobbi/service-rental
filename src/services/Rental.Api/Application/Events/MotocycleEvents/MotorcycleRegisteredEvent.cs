using Rental.Core.Messages;

namespace Rental.Api.Application.Events
{
    public class MotorcycleRegisteredEvent : Event
    {
        public Guid Id { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public MotorcycleRegisteredEvent(Guid id, int year, string model, string plate)
        {
            AggregateId = id;
            Id = id;
            Year = year;
            Model = model;
            Plate = plate;
            CreatedAt = Timestamp;
        }
    }
}