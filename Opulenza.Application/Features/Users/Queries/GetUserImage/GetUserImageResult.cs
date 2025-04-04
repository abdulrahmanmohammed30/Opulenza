namespace Opulenza.Application.Features.Users.Queries.GetUserImage;

public class GetUserImageResult
{
    public required byte[] Image { get; set; }
    public required string MimeType { get; set; }
}