using System.Data.Entity;

namespace ELibrary_Web_Service.Models
{
    public class LibraryContext : DbContext
    {
        public DbSet<Person> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Feed> NewsFeeds { get; set; }
    }
}