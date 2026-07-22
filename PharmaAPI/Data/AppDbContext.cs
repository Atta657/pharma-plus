using Microsoft.EntityFrameworkCore;
using PharmaAPI.Models;
using PharmaAPI.Data;

namespace PharmaAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // 🔥 ADD ALL TABLES HERE
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Visit> Visits { get; set; }

        // New Entities
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryTracking> DeliveryTrackings { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<AIInsight> AIInsights { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
    }
}