using BookMyHome.Domain.Models;
using BookMyHome.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookMyHome.Persistence.Repositories
{
    public class AccommodationRepository
    {
        private readonly BookMyHomeDbContext _context;

        public AccommodationRepository(BookMyHomeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Accommodation>> GetAllAsync()
        {
            return await _context.Accommodations
        .Include(a => a.Images)
        .Include(a => a.Host)
        .ToListAsync();
        }

        public async Task<Accommodation?> GetByIdAsync(Guid id)
        {
            return await _context.Accommodations
     .Include(a => a.Images)
    .Include(a => a.Host)
    .FirstOrDefaultAsync(
        a => a.AccommodationId == id);
        }

        public async Task AddAsync(Accommodation newAccommodation)
        {
            _context.Accommodations.Add(newAccommodation);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(
      Guid id,
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
            var accommodation = await _context.Accommodations
                .FirstOrDefaultAsync(
                    a => a.AccommodationId == id);

            if (accommodation == null)
                throw new KeyNotFoundException();

            accommodation.Update(
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

            await _context.SaveChangesAsync();
        }


        public async Task DeleteAsync(Guid id)
        {
            var accommodation = await _context.Accommodations
                .FirstOrDefaultAsync(a => a.AccommodationId ==id);

            if (accommodation == null)
                return;

            _context.Accommodations.Remove(accommodation);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetBookingsAsync(Guid accommodationId)
        {
            return await _context.Bookings
                .Where(b => b.AccommodationId == accommodationId)
                .ToListAsync();
        }


        public async Task<AccommodationImage> AddImageAsync(
    Guid accommodationId,
    string imageUrl)
        {
            var accommodation = await _context.Accommodations
                .FirstOrDefaultAsync(
                    a => a.AccommodationId == accommodationId);

            if (accommodation == null)
                throw new KeyNotFoundException();

            var image = new AccommodationImage(
                imageUrl,
                accommodationId);

            _context.AccommodationImages.Add(image);

            await _context.SaveChangesAsync();

            return image;
        }

        public async Task DeleteImageAsync(Guid imageId)
        {
            var image = await _context.AccommodationImages
                .FirstOrDefaultAsync(
                    i => i.AccommodationImageId == imageId);

            if (image == null)
                return;

            _context.AccommodationImages.Remove(image);

            await _context.SaveChangesAsync();
        }


    }
}
