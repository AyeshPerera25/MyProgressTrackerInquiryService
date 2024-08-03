using MyProgressTrackerAuthenticationService.Models.DataTransferObjects;
using MyProgressTrackerInquiryService.Models.Entities;

namespace MyProgressTrackerInquiryService.Models.DataTransferObjects
{
	public class AddCourseRes : ResponseWrapper
	{
		public Course? course { get; set; } = null;
	}
}
