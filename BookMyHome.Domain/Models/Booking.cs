using System;
using System.Collections.Generic;
using System.Text;

namespace BookMyHome.Domain.Models
{
    public class Booking
    {
        public Guid BookingId { get; private set;  }
        public DateOnly StartDate { get; private set; }

        public DateOnly EndDate { get; private set; }

        public Guid AccommodationId {  get; private set; }

        public Accommodation Accommodation { get; private set; }

        public Guid? GuestId { get; private set; }

        public Guest? Guest { get; private set; }

        public byte[] RowVersion { get; private set; } =Array.Empty<byte>();

        public Booking () { }
             public Booking(
        DateOnly startDate,
        DateOnly endDate,
        Guid accommodationId)
        {
            if (startDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException(
                    "Start date cannot be in the past");
            }

            if (endDate <= startDate)
            {
                throw new ArgumentException(
                    "End date must be after start date");
            }

            StartDate = startDate;
            EndDate = endDate;
            AccommodationId = accommodationId;
        }

        public override string ToString()
        {
            return $"Booking {BookingId}: {StartDate} - {EndDate}";
        }

        public void Update(
    DateOnly startDate,
    DateOnly endDate,
    Guid accommodationId)
        {
            if (startDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException(
                    "Booking cannot start in the past.");
            }

            if (endDate <= startDate)
            {
                throw new ArgumentException(
                    "End date must be after start date.");
            }

            StartDate = startDate;
            EndDate = endDate;
            AccommodationId = accommodationId;
        }
    }
}
