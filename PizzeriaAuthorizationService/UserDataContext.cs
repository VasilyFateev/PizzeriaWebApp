using AccountsModelClasses;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationService
{
	public class UserDataContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<UserAdress> UserAdresses { get; set; }
		public DbSet<Adress> Adresses { get; set; }
		public DbSet<UserPaymentMethod> PaymentMethods { get; set; }
		public DbSet<ShopppingCart> ShopppingCarts { get; set; }
		public DbSet<ShopppingCartItem> ShopppingCartItems { get; set; }
		public UserDataContext(DbContextOptions<UserDataContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
		public UserDataContext()
		{
			Database.EnsureCreated();
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseNpgsql(DatabaseConnectionString.UsersDatabaseConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasMany(u => u.ShopppingCarts)
				.WithOne(s => s.User)
				.HasForeignKey(s => s.UserId)
				.HasConstraintName("FK_shopping_cart_user_id");

			modelBuilder.Entity<User>()
				.HasMany(u => u.UserPaymentMethods)
				.WithOne(s => s.User)
				.HasForeignKey(s => s.UserId)
				.HasConstraintName("FK_user_payment_method_user_id");

			modelBuilder.Entity<User>()
				.HasMany(u => u.UserAdresses)
				.WithOne(s => s.User)
				.HasForeignKey(s => s.UserId)
				.HasConstraintName("FK_user_adress_user_id");

			modelBuilder.Entity<Adress>()
				.HasMany(a => a.UserAdresses)
				.WithOne(ua => ua.Adress)
				.HasForeignKey(ua => ua.AdresssId)
				.HasConstraintName("FK_adress_user_id");

			modelBuilder.Entity<UserAdress>()
				.HasKey(pc => new { pc.UserId, pc.AdresssId });

			modelBuilder.Entity<ShopppingCart>()
				.HasMany(sc => sc.Items)
				.WithOne(sci => sci.ShopppingCart)
				.HasForeignKey(sci => sci.CartId)
				.HasConstraintName("FK_shopping_cart_item_shopping_cart_id");
		}
	}
}
