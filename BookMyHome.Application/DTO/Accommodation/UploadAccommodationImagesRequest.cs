using Microsoft.AspNetCore.Http;

namespace BookMyHome.Application.DTO.Accommodation;

public sealed class UploadAccommodationImagesRequest
{
    public List<IFormFile> Files { get; set; } = new();
}