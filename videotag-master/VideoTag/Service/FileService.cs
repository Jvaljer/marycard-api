using System.Net;
using AryDotNet.File;
using AryDotNet.Messaging;
using AryDotNet.Result;
using MimeTypes;

namespace VideoTag.Service;

internal sealed class FileService(IFileStorage storage) : IFileService
{
    private static string? GetFileName(string? mimetyme) => mimetyme?.ToUpperInvariant() switch
    {
        "IMAGE/JPEG" => $"{Guid.NewGuid():N}.jpg",
        "IMAGE/PNG" => $"{Guid.NewGuid():N}.png",
        "application/json" => $"{Guid.NewGuid():N}.png",
        "application/pdf" => $"{Guid.NewGuid():N}.pdf",
        _ => null
    };

    public async Task<Result<string, MessagingError>> UploadFileAsync(IFormFile file,
        CancellationToken cancellationToken)
    {
        var mime = MimeTypeMap.GetMimeType(file.FileName);
        var fileName = GetFileName(mime);

        if (fileName is null)
        {
            return new MessagingError(HttpStatusCode.BadRequest, "Invalid file type");
        }

        var writerResult = await storage.OpenWriterAsync(fileName, cancellationToken);

        if (writerResult.IsError)
        {
            return new MessagingError(HttpStatusCode.InternalServerError, "Internal server error");
        }

        var writer = writerResult.Unwrap();

        var result = await writer.WriteStreamAsync(file.OpenReadStream(), cancellationToken);

        return result.Map(() => fileName, err => new MessagingError(HttpStatusCode.InternalServerError, err.Message));
    }
}