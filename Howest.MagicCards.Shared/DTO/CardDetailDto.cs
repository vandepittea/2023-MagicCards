namespace Howest.MagicCards.Shared.DTO
{
    public class CardDetailDto : CardDto
    {
        public string Flavor { get; set; }
        public string Toughness { get; set; }
        public string Layout { get; set; }
        public string ConvertedManaCost { get; set; }
        public IEnumerable<string> CardColors { get; set; }
    }
}
