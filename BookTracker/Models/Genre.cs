public class Genre
{
    public int GenreID { get; set; }
    public string GenreName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
