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
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private int _pageNumber = 1;
        private int _pageSize = 150;

        public List<CardDto> Cards { get; private set; }
        public CardDetailDto Card { get; private set; }
        public string Error { get; private set; }
        public bool IsLoading { get; private set; } = true;
        public bool IsFirstPage { get; private set; }
        public bool IsLastPage { get; private set; }
        public event EventHandler CardsChanged;

        public CardService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CardAPI");
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        protected virtual void OnCardsChanged()
        {
            CardsChanged?.Invoke(this, EventArgs.Empty);
        }

        public async Task LoadCards(CardWebFilterV1_5 filter = null)
        {
            try
            {
                IsLoading = true;
                Error = null;

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
                    PagedResponse<IEnumerable<CardDto>> result = JsonSerializer.Deserialize<PagedResponse<IEnumerable<CardDto>>>(apiResponse, _jsonOptions);
                    Cards = result.Data.ToList();
                    IsFirstPage = result.PageNumber == 1;
                    IsLastPage = result.Data.Count() < result.PageSize;
                }
                else
                {
                    Error = $"Error: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            finally
            {
                IsLoading = false;
                OnCardsChanged();
            }
        }

        public async Task GoToPreviousPage()
        {
            if (!IsFirstPage)
            {
                _pageNumber--;
                await LoadCards();
            }
        }

        public async Task GoToNextPage()
        {
            if (!IsLastPage)
            {
                _pageNumber++;
                await LoadCards();
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
                    Error = $"Error: {response.ReasonPhrase}";
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
        }
    }
}