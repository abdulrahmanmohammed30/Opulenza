namespace Opulenza.Contracts.Common
{
    public class ImageResponse
    {
        public int Id { get; set; }
        public required string FilePath { get; set; }
        public bool IsFeaturedImage { get; set; }
    }
}