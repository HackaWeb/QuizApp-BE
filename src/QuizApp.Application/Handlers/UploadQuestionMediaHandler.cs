using MediatR;
using Microsoft.AspNetCore.Http;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Exceptions;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;
using System.Net;
using Xabe.FFmpeg;

namespace QuizApp.Application.Handlers;
public class UploadQuestionMediaHandler(IUnitOfWork unitOfWork, IBlobStorageRepository blobStorageRepository) : IRequestHandler<UploadQuestionMediaRequest>
{
    public async Task Handle(UploadQuestionMediaRequest request, CancellationToken cancellationToken)
    {
        var question = await unitOfWork.QuestionRepository.GetByIdAsync(Guid.Parse(request.QuestionId));
        var mediaUrl = await UploadMediaAsync(request.File);

        question.MediaUrl = mediaUrl;
        await unitOfWork.SaveEntitiesAsync();
    }

    private async Task<string?> UploadMediaAsync(IFormFile? file)
    {
        if (file is null) return null;

        var allowedFormats = new HashSet<string>
        {
            "image/png", "image/jpeg",
            "video/mp4", "video/webm", "video/avi", "video/mkv"
        };

        if (!allowedFormats.Contains(file.ContentType))
        {
            throw new DomainException("Only PNG, JPG images or MP4, WebM, AVI, MKV videos are allowed.",
                (int)HttpStatusCode.BadRequest);
        }

        var extension = file.ContentType switch
        {
            "image/png" => "png",
            "image/jpeg" => "jpg",
            "video/mp4" => "mp4",
            "video/webm" => "webm",
            "video/avi" => "avi",
            "video/mkv" => "mkv",
            _ => throw new DomainException("Unsupported file format.", (int)HttpStatusCode.BadRequest)
        };

        var fileName = $"{Guid.NewGuid()}.{extension}";

        if (file.ContentType.StartsWith("video"))
        {
            const int maxVideoDuration = 60;
            await ValidateVideoDurationAsync(file, maxVideoDuration);
        }

        using var uploadStream = file.OpenReadStream();
        return await blobStorageRepository.UploadAsync(uploadStream, fileName, file.ContentType, "media");
    }

    private async Task ValidateVideoDurationAsync(IFormFile file, int maxDuration)
    {
        var tempPath = Path.GetTempFileName();

        try
        {
            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var mediaInfo = await FFmpeg.GetMediaInfo(tempPath);
            var duration = mediaInfo.Duration.TotalSeconds;

            if (duration > maxDuration)
            {
                throw new DomainException($"Maximum video duration is {maxDuration} seconds.",
                    (int)HttpStatusCode.BadRequest);
            }
        }
        finally
        {
            File.Delete(tempPath);
        }
    }
}
