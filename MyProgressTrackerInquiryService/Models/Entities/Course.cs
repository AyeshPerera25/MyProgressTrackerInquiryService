using MyProgressTrackerAuthenticationService.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyProgressTrackerInquiryService.Models.Entities
{
	public class Course
	{
		[Key]
		[Required]
		public int CourseId { get; set; }
		[Required]
		public string CourseName { get; set; }
		[Required]
		public long UserId { get; set; }
		[Required]
		public User User { get; set; }
		[Required]
		public string UniversityName { get; set; }
		public string? CourseDescription { get; set; }
		[Required]
		public DateTime CourseStartDate { get; set; }
		[Required]
		public DateTime CourseEndDate { get; set; }
		[Required]
		public int NoOfSemesters { get; set; }

		// Default constructor
		public Course() { }

		public Course(int courseId, string courseName, long userId, User user, string universityName, string? courseDescription, DateTime courseStartDate, DateTime courseEndDate, int noOfSemesters)
		{
			CourseId = courseId;
			CourseName = courseName;
			UserId = userId;
			User = user;
			UniversityName = universityName;
			CourseDescription = courseDescription;
			CourseStartDate = courseStartDate;
			CourseEndDate = courseEndDate;
			NoOfSemesters = noOfSemesters;
		}
	}
}
