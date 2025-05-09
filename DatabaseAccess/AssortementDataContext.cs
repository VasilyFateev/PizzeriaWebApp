using Microsoft.EntityFrameworkCore;
using ModelClasses;

namespace DatabaseAccess
{

	public class AssortementDataContext : DbContext
	{
		public DbSet<ProductCategory> ProductCategories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductItem> ProductItems { get; set; }
		public DbSet<Variation> Variation { get; set; }
		public DbSet<VariationOption> VariationOption { get; set; }
		public DbSet<ProductConfiguration> ProductConfiguration { get; set; }

		public AssortementDataContext(DbContextOptions<AssortementDataContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
		public AssortementDataContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(DatabaseConnectionString.AssortmentDatabaseConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ProductCategory>()
				.HasMany(pc => pc.Products)
				.WithOne(p => p.Category)
				.HasForeignKey(p => p.CategoryId)
				.HasConstraintName("FK_product_product_category_id");

			modelBuilder.Entity<ProductCategory>()
				.HasMany(pc => pc.Variations)
				.WithOne(v => v.Category)
				.HasForeignKey(v => v.CategoryId)
				.HasConstraintName("FK_variation_product_category_id");

			modelBuilder.Entity<Product>()
				.HasMany(p => p.ProductItems)
				.WithOne(pi => pi.Product)
				.HasForeignKey(pi => pi.ProductId)
				.HasConstraintName("FK_product_item_product_id");

			modelBuilder.Entity<ProductItem>()
				.HasMany(pi => pi.Configurations)
				.WithOne(pc => pc.ProductItem)
				.HasForeignKey(pc => pc.ProductItemId)
				.HasConstraintName("FK_product_configuration_product_item_id");

			modelBuilder.Entity<Variation>()
				.HasMany(v => v.VariationOptions)
				.WithOne(vo => vo.Variation)
				.HasForeignKey(vo => vo.VariationId)
				.HasConstraintName("FK_variation_option_variation_id");

			modelBuilder.Entity<VariationOption>()
				.HasMany(vo => vo.Configurations)
				.WithOne(pc => pc.VariationOption)
				.HasForeignKey(pc => pc.VariationOptionId)
				.HasConstraintName("FK_product_configuration_variation_option_id");

			modelBuilder.Entity<ProductConfiguration>()
				.HasKey(pc => new { pc.ProductItemId, pc.VariationOptionId });
		}
	}

}
