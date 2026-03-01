using Microsoft.EntityFrameworkCore;
using server_api.Models;

namespace server_api.Data
{
    public class SellContext : DbContext
    {
        public SellContext(DbContextOptions<SellContext> options) : base(options) { }

        public DbSet<Gift> Gifts => Set<Gift>();
        public DbSet<Basket> Baskets => Set<Basket>();
        public DbSet<BasketItem> BasketItems => Set<BasketItem>();
        public DbSet<Donor> Donors => Set<Donor>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> categories => Set<Category>();

        //לשים לב להריץ על זה מגריישן
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // 🔐 UserName חייב להיות ייחודי
        //    modelBuilder.Entity<User>()
        //        .HasIndex(u => u.UserName)
        //        .IsUnique();
        //}
    }


}


