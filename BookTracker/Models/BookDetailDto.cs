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