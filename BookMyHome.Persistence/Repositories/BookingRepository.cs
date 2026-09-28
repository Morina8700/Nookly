using BookMyHome.Domain.Exceptions;
using BookMyHome.Domain.Models;
using BookMyHome.Persistence.Data;
using Microsoft.EntityFrameworkCore;
namespace BookMyHome.Persistence.Repositories
{
    public class BookingRepository
    {
        private readonly BookMyHomeDbContext _context;

        public BookingRepository(BookMyHomeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.Accommodation)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .Include(b => b.Accommodation)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task AddAsync(Booking booking)
        {
            bool hasOverlap = await _context.Bookings
                .AnyAsync(b =>
                    b.AccommodationId == booking.AccommodationId &&
                    booking.StartDate <= b.EndDate &&
                    booking.EndDate >= b.StartDate);

            if (hasOverlap)
            {
                throw new OverlapingBookingException(
                    "Accommodation is already booked for these dates.");
            }

            _context.Bookings.Add(booking);

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Guid id)
        {
            var booking = await _context.Bookings
                .FindAsync(id);

            if (booking == null)
            {
                return;
            }

            _context.Bookings.Remove(booking);

            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(
     Guid id,
     DateOnly startDate,
     DateOnly endDate,
     Guid accommodationId,
     byte[] rowVersion)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
                throw new KeyNotFoundException();

            bool hasOverlap = await _context.Bookings.AnyAsync(b =>
                b.BookingId != id &&
                b.AccommodationId == accommodationId &&
                startDate <= b.EndDate &&
                endDate >= b.StartDate);

            if (hasOverlap)
            {
                throw new OverlapingBookingException(
                    "Accommodation is already booked for these dates.");
            }

            booking.Update(
                startDate,
                endDate,
                accommodationId);

            _context.Entry(booking)
                .Property(b => b.RowVersion)
                .OriginalValue = rowVersion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BookingConcurrencyException(
                    "The booking has been changed by another user.");
            }
        }


        public async Task<List<Booking>> SearchAsync(
            Guid? accommodationId,
            DateOnly? from,
            DateOnly? to)
        {
            

            var query = _context.Bookings
            .Include(b => b.Accommodation)
            .AsQueryable();

            if(accommodationId.HasValue)
            {
                query = query.Where(b => b.AccommodationId == accommodationId.Value);
            }

            if(from.HasValue)
            {
                query = query.Where(b => b.EndDate >= from.Value);
            }

            if(to.HasValue)
            {
                query = query.Where(b => b.StartDate <= to.Value);
            }

            return await query
            .OrderBy(b => b.StartDate)
            .ToListAsync();
        }

    }
}
