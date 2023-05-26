using System.Collections.Generic;

namespace CardDeckManagement.Data
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string SetName { get; set; }
        public string RarityName { get; set; }
        public string Power { get; set; }
        public string Image { get; set; }
        public string ArtistName { get; set; }
        public List<string> CardTypes { get; set; }
    }
}