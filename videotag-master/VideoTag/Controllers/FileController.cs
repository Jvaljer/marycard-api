using System.Net;
using AryDotNet.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeTypes;
using VideoTag.Service;

namespace VideoTag.Controllers;

[Authorize]
[Route("file")]
public sealed class FileController(IFileService service, IFileStorage storage) : Controller
{
    [HttpGet("{file}")]
    public async Task<IActionResult> GetFile(string file, CancellationToken cancellationToken)
    {
        var reader = await storage.OpenReaderAsync(file, cancellationToken);

        if (reader.IsError)
        {
            return StatusCode((int)HttpStatusCode.NotFound);
        }

        var stream = (await reader.Unwrap().ReadStreamAsync(cancellationToken)).Unwrap();
        var mime = MimeTypeMap.GetMimeType(file);
        return File(stream, mime, file);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
    {
        var result = await service.UploadFileAsync(file, cancellationToken);
        return result.MapOrElse<IActionResult>(o => Ok(new
        {
            FileId = o
        }), err => StatusCode((int)err.Code, new
        {
            Reason = err.Message
        }));
    }
}