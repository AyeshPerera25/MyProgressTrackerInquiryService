using Microsoft.EntityFrameworkCore;
using MyProgressTrackerAuthenticationService.Models.Entities;
using MyProgressTrackerInquiryService.Models.Entities;

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
			base.OnModelCreating(modelBuilder);
			// Additional configuration can be done here
		}
	}
}
