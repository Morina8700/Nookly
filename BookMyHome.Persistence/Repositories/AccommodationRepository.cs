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

  public async Task<List<Accommodation>> SearchAsync(
    string? location,
    decimal? minPrice,
    decimal? maxPrice,
    bool? hasWifi,
    int? maxGuests)
{
    var query = _context.Accommodations
        .Include(a => a.Images)
        .Include(a => a.Host)
        .AsQueryable();

    if (!string.IsNullOrWhiteSpace(location))
    {
        query = query.Where(a =>
            a.Name.Contains(location)
            || a.Address.Contains(location));
    }

    if (minPrice.HasValue)
    {
        query = query.Where(a =>
            a.PricePerNight >= minPrice.Value);
    }

    if (maxPrice.HasValue)
    {
        query = query.Where(a =>
            a.PricePerNight <= maxPrice.Value);
    }

    if (hasWifi.HasValue)
    {
        query = query.Where(a =>
            a.HasWifi == hasWifi.Value);
    }

    if (maxGuests.HasValue)
    {
        query = query.Where(a =>
            a.MaxGuests >= maxGuests.Value);
    }

    return await query
        .OrderBy(a => a.PricePerNight)
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

public async Task<bool> DeleteImageAsync(
    Guid accommodationId,
    Guid imageId)
{
    var image = await _context.AccommodationImages
        .FirstOrDefaultAsync(i =>
            i.AccommodationImageId == imageId
            && i.AccommodationId == accommodationId);

    if (image == null)
    {
        return false;
    }

    _context.AccommodationImages.Remove(image);
    await _context.SaveChangesAsync();

    return true;
}

    }
}
