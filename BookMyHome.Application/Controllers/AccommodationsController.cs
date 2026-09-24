using BookMyHome.Application.DTO.Accommodation;
using BookMyHome.Domain.Models;
using BookMyHome.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace BookMyHome.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccommodationsController : ControllerBase
    {
        private readonly AccommodationRepository _repository;

        public AccommodationsController(AccommodationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var accommodation = await _repository.GetAllAsync();

            return Ok(accommodation);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var accommodation = await _repository.GetByIdAsync(id);

            if (accommodation == null)
                return NotFound();

            return Ok(accommodation);
        }

        [HttpPost]
        public async Task<ActionResult> Create(
       CreateAccommodationDto dto)
        {
            try
            {
                var accommodation = new Accommodation(
                    dto.Name,
                    dto.Address,
                    dto.Description,
                    dto.PricePerNight,
                    dto.HostId,
                    dto.FloorAreaSquareMeters,
                    dto.Bedrooms,
                    dto.Bathrooms,
                    dto.MaxGuests,
                    dto.HasAirConditioning,
                    dto.HasHeatedFloors,
                    dto.HasWifi,
                    dto.HasKitchen,
                    dto.HasParking,
                    dto.HasWasher,
                    dto.HasTv,
                    dto.HasBalcony,
                    dto.HasPool,
                    dto.PetsAllowed,
                    dto.SmokingAllowed);

                await _repository.AddAsync(accommodation);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = accommodation.AccommodationId },
                    accommodation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
    Guid id,
    UpdateAccommodationDto dto)
        {
            try
            {
                await _repository.UpdateAsync(
                    id,
                    dto.Name,
                    dto.Address,
                    dto.Description,
                    dto.PricePerNight,
                    dto.FloorAreaSquareMeters,
                    dto.Bedrooms,
                    dto.Bathrooms,
                    dto.MaxGuests,
                    dto.HasAirConditioning,
                    dto.HasHeatedFloors,
                    dto.HasWifi,
                    dto.HasKitchen,
                    dto.HasParking,
                    dto.HasWasher,
                    dto.HasTv,
                    dto.HasBalcony,
                    dto.HasPool,
                    dto.PetsAllowed,
                    dto.SmokingAllowed);

                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var accommodation = await _repository.GetByIdAsync(id);

            if (accommodation == null)
                return NotFound();

            await _repository.DeleteAsync(id);

            return NoContent();
        }

        [HttpGet("{id:guid}/bookings")]
        public async Task<ActionResult> GetBookings(Guid id)
        {
            var accommodation = await _repository.GetByIdAsync(id);

            if (accommodation == null)
                return NotFound();

            var bookings = await _repository.GetBookingsAsync(id);

            return Ok(bookings);
        }

        [HttpPost("{id:guid}/images")]
        public async Task<ActionResult> AddImage(
    Guid id,
    AddAccommodationImageDto dto)
        {
            try
            {
                var image = await _repository.AddImageAsync(
                    id,
                    dto.ImageUrl);

                return Ok(image);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:guid}/images/{imageId:guid}")]
        public async Task<IActionResult> DeleteImage(
            Guid id,
            Guid imageId)
            {
            var accommodation =
                await _repository.GetByIdAsync(id);

            if (accommodation == null)
                return NotFound();

            await _repository.DeleteImageAsync(imageId);

            return NoContent();
            }

            [HttpPost("{id:guid}/images/upload")]
[RequestSizeLimit(100 * 1024 * 1024)]
public async Task<ActionResult> UploadImages(
    Guid id,
    [FromForm] UploadAccommodationImagesRequest request,
    IWebHostEnvironment environment)
{
    var accommodation = await _repository.GetByIdAsync(id);

    if (accommodation == null)
        return NotFound();

    if (request.Files.Count == 0)
        return BadRequest("Select at least one image.");

    if (request.Files.Count > 10)
        return BadRequest("You can upload a maximum of 10 images at a time.");

    var allowedTypes = new HashSet<string>(
        StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    var webRootPath = environment.WebRootPath
        ?? Path.Combine(environment.ContentRootPath, "wwwroot");

    var uploadDirectory = Path.Combine(
        webRootPath,
        "uploads",
        "accommodations",
        id.ToString("N"));

    Directory.CreateDirectory(uploadDirectory);

    foreach (var file in request.Files)
    {
        if (file.Length == 0)
            return BadRequest("Empty files cannot be uploaded.");

        if (file.Length > 10 * 1024 * 1024)
            return BadRequest(
                $"'{file.FileName}' is larger than 10 MB.");

        if (!allowedTypes.Contains(file.ContentType))
            return BadRequest(
                $"'{file.FileName}' is not a supported image type.");

        var extension = Path.GetExtension(file.FileName)
            .ToLowerInvariant();

        if (extension != ".jpg"
            && extension != ".jpeg"
            && extension != ".png"
            && extension != ".webp")
        {
            return BadRequest(
                $"'{file.FileName}' is not a supported image type.");
        }

        var storedFileName =
            $"{Guid.NewGuid():N}{extension}";

        var filePath = Path.Combine(
            uploadDirectory,
            storedFileName);

        await using var stream =
            System.IO.File.Create(filePath);

        await file.CopyToAsync(stream);

        var imageUrl =
            $"/uploads/accommodations/{id:N}/{storedFileName}";

        await _repository.AddImageAsync(
            id,
            imageUrl);
    }

    var updatedAccommodation =
        await _repository.GetByIdAsync(id);

    return Ok(updatedAccommodation);
}

       
    }
}
