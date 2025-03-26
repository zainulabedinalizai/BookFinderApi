using BookFinderApi.DTOs;
using BookFinderApi.Interfaces;
using System.Text.Json;

namespace BookFinderApi.Services
{
    public class BookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IFavoriteBookRepository _favoriteBookRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BookService(
            IBookRepository bookRepository,
            IFavoriteBookRepository favoriteBookRepository,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _bookRepository = bookRepository;
            _favoriteBookRepository = favoriteBookRepository;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IEnumerable<BookDto>> SearchBooks(string query, int? userId = null)
        {
            // First search in local database
            var localBooks = await _bookRepository.SearchBooksAsync(query);
            var localResults = new List<BookDto>();

            foreach (var b in localBooks)
            {
                localResults.Add(new BookDto
                {
                    Id = b.Id.ToString(),
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    CoverImageUrl = b.CoverImageUrl,
                    Publisher = b.Publisher,
                    PublishedDate = b.PublishedDate,
                    Isbn = b.Isbn,
                    IsFavorite = userId.HasValue && await _favoriteBookRepository.IsBookFavorite(userId.Value, b.Id.ToString())
                });
            }

            // Then search in Google Books API
            var googleBooks = await SearchGoogleBooks(query);
            var googleResults = new List<BookDto>();

            foreach (var b in googleBooks)
            {
                googleResults.Add(new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Description = b.Description,
                    CoverImageUrl = b.CoverImageUrl,
                    Publisher = b.Publisher,
                    PublishedDate = b.PublishedDate,
                    Isbn = b.Isbn,
                    IsFavorite = userId.HasValue && await _favoriteBookRepository.IsBookFavorite(userId.Value, b.Id)
                });
            }

            // Combine results, prioritizing local books
            var combinedResults = localResults.Concat(googleResults
                .Where(g => !localResults.Any(l => l.Id == g.Id)))
                .ToList();

            return combinedResults;
        }

        private async Task<IEnumerable<BookDto>> SearchGoogleBooks(string query)
        {
            var apiKey = _configuration["GoogleBooks:ApiKey"];
            var baseUrl = _configuration["GoogleBooks:BaseUrl"];

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync($"{baseUrl}?q={Uri.EscapeDataString(query)}&key={apiKey}&maxResults=20");

            if (!response.IsSuccessStatusCode)
                return Enumerable.Empty<BookDto>();

            var content = await response.Content.ReadAsStringAsync();
            var googleBooksResponse = JsonSerializer.Deserialize<GoogleBooksResponse>(content);

            return googleBooksResponse?.Items?.Select(item => new BookDto
            {
                Id = item.Id,
                Title = item.VolumeInfo?.Title ?? "No title",
                Author = item.VolumeInfo?.Authors != null ? string.Join(", ", item.VolumeInfo.Authors) : "Unknown author",
                Description = item.VolumeInfo?.Description ?? string.Empty,
                CoverImageUrl = item.VolumeInfo?.ImageLinks?.Thumbnail ?? string.Empty,
                Publisher = item.VolumeInfo?.Publisher ?? string.Empty,
                PublishedDate = DateTime.TryParse(item.VolumeInfo?.PublishedDate, out var date) ? date : null,
                Isbn = item.VolumeInfo?.IndustryIdentifiers?.FirstOrDefault(i => i.Type == "ISBN_13")?.Identifier ?? string.Empty
            }) ?? Enumerable.Empty<BookDto>();
        }

        private class GoogleBooksResponse
        {
            public List<GoogleBookItem> Items { get; set; }
        }

        private class GoogleBookItem
        {
            public string Id { get; set; }
            public VolumeInfo VolumeInfo { get; set; }
        }

        private class VolumeInfo
        {
            public string Title { get; set; }
            public List<string> Authors { get; set; }
            public string Description { get; set; }
            public ImageLinks ImageLinks { get; set; }
            public string Publisher { get; set; }
            public string PublishedDate { get; set; }
            public List<IndustryIdentifier> IndustryIdentifiers { get; set; }
        }

        private class ImageLinks
        {
            public string Thumbnail { get; set; }
        }

        private class IndustryIdentifier
        {
            public string Type { get; set; }
            public string Identifier { get; set; }
        }
    }
}
