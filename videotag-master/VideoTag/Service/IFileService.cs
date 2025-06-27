using AryDotNet.Messaging;
using AryDotNet.Result;

namespace VideoTag.Service;

public interface IFileService
{
    Task<Result<string, MessagingError>> UploadFileAsync(IFormFile file, CancellationToken cancellationToken);
}