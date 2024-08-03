using MyProgressTrackerAuthenticationService.Models.DataTransferObjects;
using MyProgressTrackerInquiryService.Models.Entities;

namespace MyProgressTrackerInquiryService.Models.DataTransferObjects
{
	public class GetAllCoursesRes : ResponseWrapper
	{
		public List<Course>? CourseList { get; set; }
	}
}
