using Microsoft.EntityFrameworkCore;

namespace TravelAPI.Models
{
    public class TravelAPIContext : DbContext
    {
        public TravelAPIContext(DbContextOptions<TravelAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Review> Reviews { get; set; }
    }
}