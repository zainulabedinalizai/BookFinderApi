﻿namespace BookFinderApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }
        public string Publisher { get; set; }
        public DateTime? PublishedDate { get; set; }
        public string Isbn { get; set; }
    }
}
