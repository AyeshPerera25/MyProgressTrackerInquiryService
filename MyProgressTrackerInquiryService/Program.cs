
using Microsoft.EntityFrameworkCore;
using MyProgressTrackerInquiryService.DataResources;
using MyProgressTrackerInquiryService.Handlers;
using MyProgressTrackerInquiryService.Services;

namespace MyProgressTrackerInquiryService
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			builder.Services.AddDbContext<AzuerSQLDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AzuerSQLDBConnection")));
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddTransient<DashboardInquiryServiceCore>();
			builder.Services.AddScoped<ReportInquiryHandler>();
			builder.Services.AddScoped<CourseInquiryHandler>();
			builder.Services.AddScoped<SubjectInquiryHandler>();
			builder.Services.AddScoped<StudySessionInquiryHandler>();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

	
			app.UseSwagger();
			app.UseSwaggerUI();

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
