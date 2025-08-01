

using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess
{
    public class MyRecipeBookDbContext : DbContext
    {
        public MyRecipeBookDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<CookingTime> CookingTime { get; set; }
        public DbSet<Difficulty> Difficulty { get; set; }
        public DbSet<DishType> DishTypes { get; set; }
        public DbSet<RecipeDishType> RecipesDishTypes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeDishType>()
                .HasKey(rdt => new { rdt.RecipeId, rdt.DishTypeId });

            modelBuilder.Entity<RecipeDishType>()
                .HasOne(rdt => rdt.Recipe)
                .WithMany(r => r.RecipeDishTypes)
                .HasForeignKey(rdt => rdt.RecipeId);

            modelBuilder.Entity<RecipeDishType>()
                .HasOne(rdt => rdt.DishType)
                .WithMany()
                .HasForeignKey(rdt => rdt.DishTypeId);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyRecipeBookDbContext).Assembly);
        }
    }
}
