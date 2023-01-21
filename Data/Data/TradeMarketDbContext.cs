using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) : base(options)
        {
        }

        public TradeMarketDbContext()
        {
        }
            
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var connectionString = "Server=localhost; User Id = SA; PWD = MyPass@word; Database=TradeMarket;";

        //    optionsBuilder.UseSqlServer(connectionString);
        //}


        //TODO: write DbSets for entities
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }

        //TODO: write Fluent API configuration
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Customer>()
                .HasOne<Person>(p => p.Person)
                .WithOne()
                .HasForeignKey<Customer>(c => c.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany<Receipt>(p => p.Receipts)
                .WithOne()
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Receipt>()
                .HasMany<ReceiptDetail>(r => r.ReceiptDetails)
                .WithOne()
                .HasForeignKey(r => r.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany<ReceiptDetail>(p => p.ReceiptDetails)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ProductCategory>()
                .HasMany<Product>(p => p.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(r => r.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
