using AutoMapper;
using Howest.MagicCards.Shared.DTO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Howest.MagicCards.Web.Services
{
    public class DeckService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;
        private readonly CardService _cardService;
        private readonly IMapper _mapper;

        public List<CardInDeckDto> DeckCards { get; private set; }
        public List<CardDetailDto> DeckCardsExtended { get; private set; }
        public string Error { get; private set; }
        public event EventHandler DeckCardsChanged;

        public DeckService(IMapper mapper, CardService cardService, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("DeckAPI");
            _cardService = cardService;
            _mapper = mapper;
            DeckCards = new List<CardInDeckDto>();
            DeckCardsExtended = new List<CardDetailDto>();
        }

        protected virtual void OnDeckCardsChanged()
        {
            DeckCardsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task LoadDeck()
        {
            try
            {
                DeckCards.Clear();
                DeckCardsExtended.Clear();

                HttpResponseMessage response = await _httpClient.GetAsync("deck");
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    DeckCards = JsonSerializer.Deserialize<List<CardInDeckDto>>(apiResponse, _jsonOptions);

                    foreach (CardInDeckDto deckCard in DeckCards)
                    {
                        await _cardService.LoadCard(int.Parse(deckCard.Id));
                        DeckCardsExtended.Add(_cardService.Card);
                    }
                }
                else
                {
                    Error = $"Error loading deck: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }

        public async Task AddToDeck(CardDto cardDto)
        {
            try
            {
                CardInDeckDto cardInDeckDto = _mapper.Map<CardInDeckDto>(cardDto);

                HttpContent content = new StringContent(JsonSerializer.Serialize(cardInDeckDto), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("deck", content);

                if (!response.IsSuccessStatusCode)
                {
                    Error = $"Adding card to deck: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                Error = $"Error adding card to deck: {ex.Message}";
            }
            finally
            {
                OnDeckCardsChanged();
            }
        }

        public async Task UpdateCardCount(CardInDeckDto cardInDeckDto)
        {
            HttpContent content = new StringContent(JsonSerializer.Serialize(cardInDeckDto), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PutAsync("deck", content);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                Error = $"Error updating card: {response.ReasonPhrase}";
            }
            else
            {
                OnDeckCardsChanged();
            }
        }

        public async Task RemoveFromDeck(string cardId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"deck/{cardId}");

                OnDeckCardsChanged();
            }
            catch (Exception ex)
            {
                Error = $"Error removing card from deck: {ex.Message}";
            }
        }

        public async Task ClearDeck()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync("deck/clear");

                OnDeckCardsChanged();
            }
            catch (Exception ex)
            {
                Error = $"Error clearing deck: {ex.Message}";
            }
        }
    }
}