using CarShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class CarShopContext: DbContext
    {
        public CarShopContext()
        {

        }
        public CarShopContext(DbContextOptions<CarShopContext> options):base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-27C392F;Database=CarShop;Integrated Security=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
        public virtual DbSet<Car> Cars { get; set; }
    }
}
