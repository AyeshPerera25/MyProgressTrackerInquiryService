using MyProgressTrackerAuthenticationService.Models.DataTransferObjects;
using System.ComponentModel.DataAnnotations;

namespace MyProgressTrackerInquiryService.Models.DataTransferObjects
{
	public class AddCourseReq : RequestWrapper
	{
		[Required]
		public string CourseName { get; set; }
		[Required]
		public string UniversityName { get; set; }
		public string? CourseDescription { get; set; }
		[Required]
		public DateTime CourseStartDate { get; set; }
		[Required]
		public DateTime CourseEndDate { get; set; }
		[Required]
		public int NoOfSemesters { get; set; }
	}
}
