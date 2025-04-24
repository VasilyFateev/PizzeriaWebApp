using Microsoft.EntityFrameworkCore;
using ModelClasses;

namespace DatabaseAccess
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<Variation> Variation { get; set; }
        public DbSet<VariationOption> VariationOption { get; set; }
        public DbSet<ProductConfiguration> ProductConfiguration { get; set; }

        public ApplicationContext(DbSet<ProductCategory> productCategories, DbSet<Product> products,
            DbSet<ProductItem> productItems, DbSet<Variation> variation, DbSet<VariationOption> variationOption,
            DbSet<ProductConfiguration> productConfiguration)
        {
            ProductCategories = productCategories;
            Products = products;
            ProductItems = productItems;
            Variation = variation;
            VariationOption = variationOption;
            ProductConfiguration = productConfiguration;
        }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(DatabaseConnectionString.ConnectionString);
        }
    }

}
