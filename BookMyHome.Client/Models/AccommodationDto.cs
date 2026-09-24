namespace BookMyHome.Client.Models
{
    public class AccommodationDto
    {
        public Guid AccommodationId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public decimal PricePerNight { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal FloorAreaSquareMeters { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int MaxGuests { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasHeatedFloors { get; set; }
        public bool HasWifi { get; set; }
        public bool HasKitchen { get; set; }
        public bool HasParking { get; set; }
        public bool HasWasher { get; set; }
        public bool HasTv { get; set; }
        public bool HasBalcony { get; set; }
        public bool HasPool { get; set; }
        public bool PetsAllowed { get; set; }
        public bool SmokingAllowed { get; set; }

        public Guid HostId { get; set; }

        public List<AccommodationImageDto> Images { get; set; }
     = new();
    }
}
