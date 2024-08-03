using MyProgressTrackerAuthenticationService.Models.DataTransferObjects;
using System.ComponentModel.DataAnnotations;

namespace MyProgressTrackerInquiryService.Models.DataTransferObjects
{
	public class ProgressReportReq : RequestWrapper
	{
		[Required]
		public long UserId { get; set; }
	}
}
