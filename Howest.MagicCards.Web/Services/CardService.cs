using Azure;
using Howest.MagicCards.DAL.Models;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Filters;
using Howest.MagicCards.Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Howest.MagicCards.Web.Services
{
    public class CardService
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly HttpClient _httpClient;
        private int _pageNumber = 1;
        private int _pageSize = 150;

        public List<CardDto> Cards { get; private set; }
        public CardDetailDto Card { get; private set; }
        public string Error { get; private set; }
        public bool IsLoading { get; private set; } = true;
        public bool IsFirstPage { get; private set; }
        public bool IsLastPage { get; private set; }
        public event EventHandler CardsChanged;
        public event EventHandler<string> ErrorOccurred;

        public CardService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CardAPI");
            Cards = new List<CardDto>();
        }

        protected virtual void OnCardsChanged()
        {
            CardsChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnErrorOccurred(string error)
        {
            ErrorOccurred?.Invoke(this, error);
        }

        public async Task LoadCards(CardWebFilterV1_5 filter = null)
        {
            try
            {
                IsLoading = true;

                string uri = $"Cards?PageNumber={_pageNumber}&PageSize={_pageSize}";

                if (filter != null)
                {
                    var queryParameters = new List<string>();

                    if (!string.IsNullOrEmpty(filter.SetName))
                        queryParameters.Add($"SetName={Uri.EscapeDataString(filter.SetName)}");

                    if (!string.IsNullOrEmpty(filter.ArtistName))
                        queryParameters.Add($"ArtistName={Uri.EscapeDataString(filter.ArtistName)}");

                    if (!string.IsNullOrEmpty(filter.RarityName))
                        queryParameters.Add($"RarityName={Uri.EscapeDataString(filter.RarityName)}");

                    if (!string.IsNullOrEmpty(filter.TypeName))
                        queryParameters.Add($"TypeName={Uri.EscapeDataString(filter.TypeName)}");

                    if (!string.IsNullOrEmpty(filter.CardName))
                        queryParameters.Add($"CardName={Uri.EscapeDataString(filter.CardName)}");

                    if (!string.IsNullOrEmpty(filter.CardText))
                        queryParameters.Add($"CardText={Uri.EscapeDataString(filter.CardText)}");

                    if (filter.SortBy == "desc")
                        queryParameters.Add($"SortBy={Uri.EscapeDataString(filter.SortBy)}");

                    if (queryParameters.Any())
                        uri += $"&{string.Join("&", queryParameters)}";
                }

                HttpResponseMessage response = await _httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    PagedResponse<IEnumerable<CardDto>> result = await JsonSerializer.DeserializeAsync<PagedResponse<IEnumerable<CardDto>>>(response.Content.ReadAsStream(), _jsonOptions);
                    Cards = result.Data.ToList();
                    IsFirstPage = result.PageNumber == 1;
                    IsLastPage = result.Data.Count() < result.PageSize;
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    OnErrorOccurred(errorMessage.Trim('"'));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading cards: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                OnCardsChanged();
            }
        }

        public async Task GoToPreviousPage(CardWebFilterV1_5 filter)
        {
            if (!IsFirstPage)
            {
                _pageNumber--;
                await LoadCards(filter);
            }
        }

        public async Task GoToNextPage(CardWebFilterV1_5 filter)
        {
            if (!IsLastPage)
            {
                _pageNumber++;
                await LoadCards(filter);
            }
        }

        public async Task LoadCard(int id)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"Cards/{id}");
                string apiResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    Card = JsonSerializer.Deserialize<CardDetailDto>(apiResponse, _jsonOptions);
                }
                else
                {
                    Error = "Card not found";
                }
            }
            catch (Exception ex)
            {
                Error = $"Error loading card: {ex.Message}";
                Console.WriteLine(Error);
            }
        }
    }
}