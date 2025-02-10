using MediatR;
using Microsoft.AspNetCore.Http;
using QuizApp.Contracts.Rest.Requests;
using QuizApp.Domain.Exceptions;
using QuizApp.Infrastructure;
using QuizApp.Infrastructure.Repositories;
using System.Net;

namespace QuizApp.Application.Handlers;

public class UploadQuizFileHandler(IUnitOfWork unitOfWork, IBlobStorageRepository blobStorageRepository) : IRequestHandler<UploadQuizFileRequest>
{
    public async Task Handle(UploadQuizFileRequest request, CancellationToken cancellationToken)
    {
        var quiz = await unitOfWork.QuizRepository.GetByIdAsync(Guid.Parse(request.QuizId));
        string imageUrl = await UploadFileAsync(request.File, new HashSet<string> { "image/png", "image/jpeg" });

        quiz.ImageUrl = imageUrl;
        await unitOfWork.SaveEntitiesAsync();
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
}
