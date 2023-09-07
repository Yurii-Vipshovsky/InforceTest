namespace ShorderLink.Models
{
    public class Link
    {
        public Guid Id { get; set; }
        public required string ShortLink { get; set; }
        public required string OriginalLink { get; set; }
        public required string CreatorId { get; set; }
        public DateTime CreationTime { get; set; }
        public uint ViewsCout { get; set; }
    }
}
