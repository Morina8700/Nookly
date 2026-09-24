namespace BookMyHome.Client.Models
{
    public class UpdateAccommodationDto
    {
        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public decimal PricePerNight { get; set; }

        public string Description { get; set; } = string.Empty;

        [System.ComponentModel.DataAnnotations.Range(0.01, double.MaxValue)]
        public decimal FloorAreaSquareMeters { get; set; }
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        public int Bedrooms { get; set; }
        [System.ComponentModel.DataAnnotations.Range(0, int.MaxValue)]
        public int Bathrooms { get; set; }
        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
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
    }
}
