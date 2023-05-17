namespace Howest.MagicCards.Shared.DTO
{
    public class CardDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ConvertedManaCost { get; set; }
        public string Type { get; set; }
        public string SetName { get; set; }
        public string RarityName { get; set; }
        public string Image { get; set; }
        public IEnumerable<string> CardColors { get; set; }
        public IEnumerable<string> CardTypes { get; set; }
    }
}
