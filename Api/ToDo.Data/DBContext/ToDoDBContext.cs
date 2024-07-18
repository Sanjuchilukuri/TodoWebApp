using Microsoft.EntityFrameworkCore;
using ToDo.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ToDo.Data.DBContext
{
    /// <summary>
    /// The DbContext class for the ToDoDB
    /// </summary>
    public class ToDoDBContext : DbContext
    {
        /// <summary>
        /// Constructor for the ToDoDBContext class that takes <see cref="DbContextOptions"/> as a parameter
        /// </summary>
        /// <param name="dbContextOptions"><see cref="DbContextOptions"/></param>
        public ToDoDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }


        /// <summary>
        /// DbSet for the User entity 
        /// </summary>
        public DbSet<User> Users { get; set; }
        
        /// <summary>
        /// DbSet for the ToDoItem entity
        /// </summary>
        public DbSet<ToDoItem> Tasks { get; set; }

        /// <summary>
        /// Override the OnModelCreating method to configure the entities
        /// </summary>
        /// <param name="modelBuilder">modelBuilder is the instance of ModelBuilder that is used to configure the entities</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(seed: 1, increment: 1);

                entity.Property(e => e.Name)
                .IsRequired();

                entity.HasIndex(e => e.Name)
                .IsUnique();

                entity.Property(e => e.Password)
                .IsRequired();
            });

            modelBuilder.Entity<ToDoItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn(seed: 1, increment: 1);

                entity.Property(e => e.Title)
                .IsRequired();

                entity.Property(e => e.Description)
                .IsRequired();

                entity.Property(e => e.CreatedOn)
                .IsRequired();

                entity.HasOne(e => e.User)
                .WithMany(u => u.ToDoItems)
                .HasForeignKey(e => e.UserId);

            });
        }

        /// <summary>
        /// Override the SaveChangesAsync method to add the CreatedBy, CreatedOn, ModifiedBy, ModifiedOn fields
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added || e.State == EntityState.Modified
            ));


            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedBy = Environment.UserName;
                    ((BaseEntity)entityEntry.Entity).CreatedOn = DateTime.Now;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    ((BaseEntity)entityEntry.Entity).ModifiedBy = Environment.UserName;
                    ((BaseEntity)entityEntry.Entity).ModifiedOn = DateTime.Now;

                    entityEntry.Property(nameof(BaseEntity.CreatedBy)).IsModified = false;
                    entityEntry.Property(nameof(BaseEntity.CreatedOn)).IsModified = false;
                }
            }
            return await base.SaveChangesAsync();
        }
    }
}