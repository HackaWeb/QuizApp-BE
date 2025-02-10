using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain;
using QuizApp.Domain.Exceptions;
using QuizApp.Domain.Models;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;
using System.Net;
using Xabe.FFmpeg;

namespace QuizApp.Application.Handlers;

public class CreateQuizHandler(
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor,
    ILogger<CreateQuizHandler> logger,
    IUnitOfWork unitOfWork,
    IBlobStorageRepository blobStorageRepository) : IRequestHandler<CreateQuizRequest>
{
    public async Task Handle(CreateQuizRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("{handler} started executing!", nameof(CreateQuizHandler));

        var currentUser = httpContextAccessor.HttpContext!.User;
        var currentUserId = userManager.GetUserId(currentUser);

        if (string.IsNullOrEmpty(currentUserId))
        {
            throw new DomainException("User not found.", (int)HttpStatusCode.Unauthorized);
        }

        string? imageUrl = await UploadFileAsync(request.Quiz.File, new HashSet<string> { "image/png", "image/jpeg" });

        var quiz = new Quiz
        {
            Title = request.Quiz.Title,
            Description = request.Quiz.Description,
            ImageUrl = imageUrl,
            Duration = request.Quiz.Duration,
            OwnerId = Guid.Parse(currentUserId),
            Questions = new List<Question>()
        };

        foreach (var item in request.Quiz.Questions)
        {
            string? fileUrl = await UploadMediaAsync(item.File);

            var question = new Question
            {
                Text = item.Title,
                MediaUrl = fileUrl,
                Type = (QuestionType)item.Type,
                ChoiceOptions = item.Options?.Select(o => new AnswerOption
                {
                    Title = o.Title,
                    IsCorrect = o.IsCorrect
                }).ToList()
            };

            quiz.Questions.Add(question);
        }

        await unitOfWork.QuizRepository.AddAsync(quiz);
        await unitOfWork.SaveEntitiesAsync();

        logger.LogInformation("Quiz '{QuizTitle}' created successfully!", quiz.Title);
    }

    private async Task<string?> UploadFileAsync(IFormFile? file, HashSet<string> allowedFormats)
    {
        if (file is null) return null;

        if (!allowedFormats.Contains(file.ContentType))
        {
            throw new DomainException("Only PNG and JPG images are allowed.", (int)HttpStatusCode.BadRequest);
        }

        var extension = file.ContentType == "image/png" ? "png" : "jpg";
        var fileName = $"{Guid.NewGuid()}.{extension}";

        using var stream = file.OpenReadStream();
        return await blobStorageRepository.UploadAsync(stream, fileName, file.ContentType, "media");
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
