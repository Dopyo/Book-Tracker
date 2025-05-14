public record BookSummaryDto(
    int BookID,
    string Title,
    List<string> Authors,
    int AvailableCopies
);