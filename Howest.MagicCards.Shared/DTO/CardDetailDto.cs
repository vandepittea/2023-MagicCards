namespace Howest.MagicCards.Shared.DTO
{
    public class CardDetailDto : CardDto
    {
        public string Text { get; set; }
        public string Flavor { get; set; }
        public string Power { get; set; }
        public string Toughness { get; set; }
        public string Layout { get; set; }
        public string ArtistName { get; set; }
    }
}
