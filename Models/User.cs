﻿namespace BookFinderApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string ThemePreference { get; set; } = "light";
        public ICollection<FavoriteBook> FavoriteBooks { get; set; }
    }
}
