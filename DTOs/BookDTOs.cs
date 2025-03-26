namespace BookFinderApi.DTOs
{
    public class BookDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Isbn { get; set; }
        public bool IsFavorite { get; set; }
    }

    public class FavoriteBookDto
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CoverImageUrl { get; set; }
        public DateTime AddedDate { get; set; }
    }

    public class AddFavoriteDto
    {
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CoverImageUrl { get; set; }
    }
}
