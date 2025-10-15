using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rental.Api.Application.Commands.CourierCommands;
using Rental.Api.Application.DTOs.Courier;
using Rental.Api.Data.Repositories.Interfaces;
using Rental.Api.Services.Storage;
using Rental.Core.Mediator;
using Rental.Services.Controllers;
using Rental.Services.Storage;

namespace Rental.Api.Controllers
{
    public class CourierController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ICourierRepository _repo;
        private readonly StorageOptions _opt;
        private readonly IStorageService _storage;
        public CourierController(IMediatorHandler mediatorHandler,
            ICourierRepository repo,
            IOptions<StorageOptions> opt,
            IStorageService storage)
        {
            _mediatorHandler = mediatorHandler;
            _repo = repo;
            _opt = opt.Value;
            _storage = storage;
        }

        [HttpPost("new-courier")]
        public async Task<IActionResult> RegisterCourier([FromBody] RegisterCourierRequest request)
        {
            var courierCommand = new RegisterCourierCommand(request);
            var result = await _mediatorHandler.SendCommand(courierCommand);
;
            return CustomResponse();
        }

        [HttpGet("{id:guid}/driver-license")]
        public async Task<ActionResult<GetDriverLicenseUrlResponse>> GetDriverLicenseAsync(
            [FromRoute] Guid id,
            CancellationToken ct)
        {
            var courier = await _repo.GetByIdAsync(id);
            if (courier is null)
                return NotFound("Courier not found.");

            if (string.IsNullOrWhiteSpace(courier.DriverLicenseImageUrl))
                return NotFound("Driver license not registered.");

            var objectKey = courier.DriverLicenseImageUrl;

            var exists = await _storage.ExistsAsync(objectKey, ct);
            if (!exists)
                return NotFound("Driver license image not found in storage.");

            var url = await _storage.GetPresignedUrlAsync(
                objectKey,
                TimeSpan.FromSeconds(_opt.PresignedExpirySeconds),
                ct);

            return Ok(new GetDriverLicenseUrlResponse(url));
        }

        [HttpPost("{id:guid}/driver-license")]
        [RequestSizeLimit(10 * 1024 * 1024)] // 10 MB
        public async Task<ActionResult<UploadDriverLicenseResponse>> UploadDriverLicenseAsync(
            [FromRoute] Guid id,
            IFormFile file,
            CancellationToken ct)
        {
            var courier = await _repo.GetByIdAsync(id);
            if (courier is null)
                return NotFound("Courier not found.");

            if (file is null || file.Length == 0)
                return BadRequest("No file uploaded.");

            const long maxFileSize = 10 * 1024 * 1024;
            if (file.Length > maxFileSize)
                return BadRequest("The uploaded file exceeds the 10MB limit.");

            var allowedContentTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedContentTypes.Contains(file.ContentType))
                return BadRequest("Invalid file format. Only JPG and PNG are allowed.");

            var randomFileId = Guid.NewGuid().ToString("N");
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var objectKey = $"couriers/licenses/{randomFileId}/driver-license{ext}";

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, ct);
            ms.Position = 0;

            await _storage.UploadAsync(ms, objectKey, file.ContentType, ct);

            var oldObjectKey = courier.DriverLicenseImageUrl;
            courier.UpdateDriverLicenseImage(objectKey);
            _repo.Update(courier);
            await _repo.UnitOfWork.Commit();

            var url = await _storage.GetPresignedUrlAsync(
                objectKey,
                TimeSpan.FromSeconds(_opt.PresignedExpirySeconds),
                ct);

            if (!string.IsNullOrEmpty(oldObjectKey))
            {
                var exists = await _storage.ExistsAsync(oldObjectKey, ct);
                if (exists)
                    await _storage.DeleteAsync(oldObjectKey, ct);
            }

            return Ok(new UploadDriverLicenseResponse(objectKey, url));
        }

        [HttpDelete("{id:guid}/driver-license")]
        public async Task<IActionResult> DeleteDriverLicenseAsync([FromRoute] Guid id, CancellationToken ct)
        {
            var courier = await _repo.GetByIdAsync(id);
            if (courier is null)
                return NotFound("Courier not found.");

            if (string.IsNullOrWhiteSpace(courier.DriverLicenseImageUrl))
                return NotFound("No driver license found for this courier.");

            var objectKey = courier.DriverLicenseImageUrl;
            var exists = await _storage.ExistsAsync(objectKey, ct);

            if (!exists)
            {
                courier.RemoveDriverLicenseImage();
                await _repo.UnitOfWork.Commit();
                return Ok("Driver license not found in storage, but record removed from database.");
            }

            try
            {
                await _storage.DeleteAsync(objectKey, ct);
                courier.RemoveDriverLicenseImage();
                _repo.Update(courier);
                await _repo.UnitOfWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error deleting driver license: {ex.Message}");
            }
        }
    }
}
