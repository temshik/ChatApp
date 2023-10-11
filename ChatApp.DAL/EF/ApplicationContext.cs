using Microsoft.EntityFrameworkCore;
using ChatApp.DAL.Entities;
using ChatApp.DAL.Configurations;

namespace ChatApp.DAL.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Message> MessageSet { get; set; } = null!;
        public DbSet<Room> RoomSet { get; set; } = null!;
        public DbSet<ApplicationUser> AppUsers { get; set; } = null!;

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

        /// <summary>
        /// Overriding method to modify the mapping of these types.
        /// </summary>
        /// <param name="modelBuilder">The model builder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
        }
    }
}
