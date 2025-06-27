using AryDotNet.Result;
using TagClient.Client;
using TagClient.Models;
using TagClient.Models.Tag;

namespace VideoTag.Mock;

internal sealed class TagClientMock : ITagClient
{
    public Task<Result<TagModel, Error>> GetByUid(string tagUid, string batchName,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult<Result<TagModel, Error>>(new TagModel
        {
            Id = Guid.NewGuid(),
            Payload = "",
            Uid = tagUid,
            Type = TagType.ICode3,
            LastTagCounter = 1,
            UsageCounter = 2,
            SignatureInvalidCounter = 3,
            SignatureValidCounter = 4,
            CreatorId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Batch = new BatchModel
            {
                Name = batchName,
                TagCount = 1
            }
        });
    }

    public Task<Result<TagModel, Error>> GetById(Guid tagId,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult<Result<TagModel, Error>>(new TagModel
        {
            Id = tagId,
            Payload = "",
            Uid = tagId.ToString(),
            Type = TagType.ICode3,
            LastTagCounter = 1,
            UsageCounter = 2,
            SignatureInvalidCounter = 3,
            SignatureValidCounter = 4,
            CreatorId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Batch = new BatchModel
            {
                Name = "ARY",
                TagCount = 1
            }
        });
    }

    public Task<Result<Error>> CreateOrUpdateTagRedirection(Guid tagId, string url,
        CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task<Result<Error>> DeleteTagRedirection(Guid tagId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TagRedirectionModel, Error>> GetTagRedirection(Guid tagId,
        CancellationToken cancellationToken = new CancellationToken())
    {
        return Task.FromResult<Result<TagRedirectionModel, Error>>(new TagRedirectionModel
        {
            BatchRedirection = "https://ary.eu",
            TagRedirection = "https://ary.eu",
            GroupRedirection = "https://ary.eu",
        });
    }
}