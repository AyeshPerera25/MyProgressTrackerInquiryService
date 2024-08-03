using MyProgressTrackerAuthenticationService.Models.DataTransferObjects;

namespace MyProgressTrackerInquiryService.Models.DataTransferObjects
{
	public class ProgressReportRes : ResponseWrapper
	{
		public List<ProgressReport>? ProgressReports { get; set; }
	}
}
