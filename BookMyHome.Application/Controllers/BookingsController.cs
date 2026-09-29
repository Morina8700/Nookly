using BookMyHome.Domain.Exceptions;
using BookMyHome.Domain.Models;
using BookMyHome.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using BookMyHome.Application.DTO.Booking;
using Microsoft.AspNetCore.Authorization;
using BookMyHome.Application.Extensions;

namespace BookMyHome.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly BookingRepository _repository;

        public BookingsController(BookingRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Booking>>> GetAll(
            [FromQuery] Guid? accommodationId,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to

        )
        {
            if(from.HasValue && to.HasValue && from > to)
            {
                return BadRequest("The 'from' date cannot be later than the 'to' date.");
            }

            var userId = User.GetUserId();

            var isHost = User.IsInRole("Host");

            var bookings =
            await _repository.SearchForUserAsync(
                userId,
                isHost,
                accommodationId,
                from,
                to
            );

            return Ok(bookings);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Booking>> GetById(Guid id)
        {
            var booking = await _repository.GetByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            var currentUserId = User.GetUserId();

            var isGuestOwner = booking.GuestId == currentUserId;

            var isAccommodationHost = booking.Accommodation?.HostId == currentUserId;

            if(!isGuestOwner && !isAccommodationHost)
            {
                return Forbid();
            }

            return Ok(booking);
        }

        [Authorize(Roles = "Guest")]
        [HttpPost]
        public async Task<ActionResult<Booking>> Create(BookingDto dto)
        {
            var booking = new Booking(
                dto.StartDate,
                dto.EndDate,
                dto.AccommodationId);

        booking.AssignGuest(User.GetUserId());

            try
            {
                await _repository.AddAsync(booking);
            }
            catch (OverlapingBookingException ex)
            {
                return Conflict(ex.Message);
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = booking.BookingId },
                booking);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var booking = await _repository.GetByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            var currentUserId = User.GetUserId();

            var isGuestOwner = booking.GuestId == currentUserId;

            var isAccommodationHost = booking.Accommodation?.HostId == currentUserId;
    
            if(!isGuestOwner && !isAccommodationHost)
            {
                return Forbid();
            }

            await _repository.DeleteAsync(id);

            return NoContent();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
    Guid id,
    UpdateBookingDto dto)
        {

            var booking = await _repository.GetByIdAsync(id);

            if(booking == null)
            {
                return NotFound();
            }

            if(booking.GuestId != User.GetUserId())
            {
                return Forbid();
            }

            try
            {
                await _repository.UpdateAsync(
                    id,
                    dto.StartDate,
                    dto.EndDate,
                    dto.AccommodationId,
                    dto.RowVersion);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Accommodation was not found.");
            }
            catch (OverlapingBookingException ex)
            {
                return Conflict(ex.Message);
            }
            catch (BookingConcurrencyException ex)
            {
                return Conflict(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }





    }
}
