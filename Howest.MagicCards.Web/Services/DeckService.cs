using AutoMapper;
using Howest.MagicCards.Shared.DTO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        public event EventHandler DeckCardsChanged;
        public event EventHandler<string> ErrorOccurred;

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

        protected virtual void OnErrorOccurred(string error)
        {
            ErrorOccurred?.Invoke(this, error);
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
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    OnErrorOccurred(errorMessage.Trim('"'));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading deck: {ex.Message}");
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
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    OnErrorOccurred(errorMessage.Trim('"'));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding card to deck: {ex.Message}");
            }
            finally
            {
                OnDeckCardsChanged();
            }
        }

        public async Task UpdateCardCount(CardInDeckDto cardInDeckDto)
        {
            try
            {
                HttpContent content = new StringContent(JsonSerializer.Serialize(cardInDeckDto), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PutAsync("deck", content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    OnErrorOccurred(errorMessage.Trim('"'));
                }
                else
                {
                    OnDeckCardsChanged();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating card: {ex.Message}");
            }
        }

        public async Task RemoveFromDeck(string cardId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"deck/{cardId}");
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();

                OnDeckCardsChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing card from deck: {ex.Message}");
            }
        }

        public async Task ClearDeck()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync("deck/clear");
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();

                OnDeckCardsChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing deck: {ex.Message}");
            }
        }
    }
}