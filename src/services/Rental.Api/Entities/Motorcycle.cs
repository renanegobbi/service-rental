using Rental.Core.DomainObjects;
using System;

namespace Rental.Api.Entities
{
    public class Motorcycle : Entity, IAggregateRoot
    {
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected Motorcycle() { }

        public Motorcycle(Guid id, int year, string model, string plate)
        {
            Id = id;
            Year = year;
            Model = model;
            Plate = plate;
        }

        public void UpdateInfo(int year, string model)
        {
            Year = year;
            Model = model;
        }
    }
}
