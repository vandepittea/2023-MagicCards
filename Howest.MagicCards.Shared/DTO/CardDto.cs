namespace Howest.MagicCards.Shared.DTO
{
    public class CardDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string SetName { get; set; }
        public string RarityName { get; set; }
        public string Power { get; set; }
        public string OriginalImageUrl { get; set; }
        public string ArtistName { get; set; }
        public IEnumerable<string> CardTypes { get; set; }
    }
}
