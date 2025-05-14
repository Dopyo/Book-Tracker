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