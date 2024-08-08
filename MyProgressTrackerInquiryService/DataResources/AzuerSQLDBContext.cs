using Microsoft.EntityFrameworkCore;
using MyProgressTrackerDependanciesLib.Models.Entities;
using MyProgressTrackerDependanciesLib.Models.Entities;

namespace MyProgressTrackerInquiryService.DataResources
{
	public class AzuerSQLDBContext : DbContext
	{
		public AzuerSQLDBContext(DbContextOptions<AzuerSQLDBContext> options) : base(options)
		{

		}
		public DbSet<User> Users { get; set; }
		public DbSet<Session> Sessions { get; set; }
		public DbSet<StudySession> StudySessions { get; set; }
		public DbSet<Subject> Subjects { get; set; }
		public DbSet<Course> Courses { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// This configures EF Core to map the User entity to the existing Users table, without trying to create it
			modelBuilder.Entity<User>().ToTable("Users", t => t.ExcludeFromMigrations());
			modelBuilder.Entity<Session>().ToTable("Sessions", t => t.ExcludeFromMigrations());

			// Define the foreign key relationship in Course
			modelBuilder.Entity<Course>()
				.HasOne(c => c.User)
				.WithMany() // Or specify the inverse navigation property
				.HasForeignKey(c => c.UserId);

			base.OnModelCreating(modelBuilder);
			// Additional configuration can be done here
		}
	}
}
