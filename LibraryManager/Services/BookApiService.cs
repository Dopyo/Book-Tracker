namespace LibraryManager.Services
{

    using System.Net.Http.Json;

    public class BookApiService
    {
        private readonly HttpClient _httpClient;

        public BookApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookSummaryDto>?> GetBooksAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<BookSummaryDto>>("books");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching books: {ex.Message}");
                return null;
            }
        }

        public async Task<BookDetailDto?> GetBookByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<BookDetailDto>($"books/{id}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching book {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<BookDetailDto?> CreateBookAsync(CreateBookDto newBook)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("books", newBook);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<BookDetailDto>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error creating book: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"books/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error deleting book {id}: {ex.Message}");
                return false;
            }
        }
    }

    public record CreateBookDto(
        string ISBN,
        string Title,
        int? GenreId,
        List<int> AuthorIds,
        int? PublicationYear,
        string? Publisher,
        int? NumberOfPages,
        int TotalCopies = 1,
        string? Edition = null,
        string? CoverImageURL = null,
        string? Description = null
    );

    public record BookDetailDto(
        int BookID,
        string ISBN,
        string Title,
        string? GenreName,
        List<string> Authors,
        int? PublicationYear,
        string? Publisher,
        int? NumberOfPages,
        int TotalCopies,
        int AvailableCopies,
        string? Edition,
        string? CoverImageURL,
        string? Description,
        DateTime DateAdded
    );

    public record BookSummaryDto(
        int BookID,
        string Title,
        List<string> Authors,
        int AvailableCopies
    );

}