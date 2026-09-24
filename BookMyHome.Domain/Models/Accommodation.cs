using System;
using System.Collections.Generic;
using System.Text;

namespace BookMyHome.Domain.Models
{

    public class Accommodation
    {
        public Guid AccommodationId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string Address { get; private set; } = string.Empty;


        public decimal PricePerNight { get; private set; }

        public string Description { get; private set; } = string.Empty;

        public decimal FloorAreaSquareMeters { get; private set; }
        public int Bedrooms { get; private set; }
        public int Bathrooms { get; private set; }
        public int MaxGuests { get; private set; }
        public bool HasAirConditioning { get; private set; }
        public bool HasHeatedFloors { get; private set; }
        public bool HasWifi { get; private set; }
        public bool HasKitchen { get; private set; }
        public bool HasParking { get; private set; }
        public bool HasWasher { get; private set; }
        public bool HasTv { get; private set; }
        public bool HasBalcony { get; private set; }
        public bool HasPool { get; private set; }
        public bool PetsAllowed { get; private set; }
        public bool SmokingAllowed { get; private set; }

        public ICollection<AccommodationImage> Images { get; private set; }
       = new List<AccommodationImage>();

        public Guid HostId { get; private set; }

        public Host? Host { get; private set; }

        public ICollection<Booking> Bookings { get; private set; }
            = new List<Booking>();

        private Accommodation()
        {
        }

        public Accommodation(
     string name,
     string address,
     string description,
     decimal pricePerNight,
    Guid hostId,
    decimal floorAreaSquareMeters,
    int bedrooms,
    int bathrooms,
    int maxGuests,
    bool hasAirConditioning,
    bool hasHeatedFloors,
    bool hasWifi,
    bool hasKitchen,
    bool hasParking,
    bool hasWasher,
    bool hasTv,
    bool hasBalcony,
    bool hasPool,
    bool petsAllowed,
    bool smokingAllowed)
        {
            AccommodationId = Guid.NewGuid();

            Update(
                name,
                address,
                description,
                pricePerNight,
                floorAreaSquareMeters,
                bedrooms,
                bathrooms,
                maxGuests,
                hasAirConditioning,
                hasHeatedFloors,
                hasWifi,
                hasKitchen,
                hasParking,
                hasWasher,
                hasTv,
                hasBalcony,
                hasPool,
                petsAllowed,
                smokingAllowed);

            HostId = hostId;
        }

        public void Update(
    string name,
    string address,
    string description,
    decimal pricePerNight,
    decimal floorAreaSquareMeters,
    int bedrooms,
    int bathrooms,
    int maxGuests,
    bool hasAirConditioning,
    bool hasHeatedFloors,
    bool hasWifi,
    bool hasKitchen,
    bool hasParking,
    bool hasWasher,
    bool hasTv,
    bool hasBalcony,
    bool hasPool,
    bool petsAllowed,
    bool smokingAllowed)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.");

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address is required.");

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description is required.");

            if (pricePerNight <= 0)
                throw new ArgumentException(
                    "Price per night must be greater than 0.");

            if (floorAreaSquareMeters <= 0)
                throw new ArgumentException("Floor area must be greater than 0.");

            if (bedrooms < 0 || bathrooms < 0 || maxGuests <= 0)
                throw new ArgumentException("Bedrooms and bathrooms cannot be negative, and maximum guests must be greater than 0.");

            Name = name;
            Address = address;
            Description = description;
            PricePerNight = pricePerNight;
            FloorAreaSquareMeters = floorAreaSquareMeters;
            Bedrooms = bedrooms;
            Bathrooms = bathrooms;
            MaxGuests = maxGuests;
            HasAirConditioning = hasAirConditioning;
            HasHeatedFloors = hasHeatedFloors;
            HasWifi = hasWifi;
            HasKitchen = hasKitchen;
            HasParking = hasParking;
            HasWasher = hasWasher;
            HasTv = hasTv;
            HasBalcony = hasBalcony;
            HasPool = hasPool;
            PetsAllowed = petsAllowed;
            SmokingAllowed = smokingAllowed;
        }
    }
}
