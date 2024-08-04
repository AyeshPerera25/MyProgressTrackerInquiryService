
using MyProgressTrackerDependanciesLib.Models.DataTransferObjects;
using MyProgressTrackerInquiryService.Handlers;
using MyProgressTrackerDependanciesLib.Models.DataTransferObjects;

namespace MyProgressTrackerInquiryService.Services
{
	public class DashboardInquiryServiceCore
	{
		private readonly ReportInquiryHandler _reportHandler;
		private readonly CourseInquiryHandler _courseInquiryHandler;

		public DashboardInquiryServiceCore(ReportInquiryHandler reportHandler, CourseInquiryHandler courseInquiryHandler)
		{
			_reportHandler = reportHandler;
			_courseInquiryHandler = courseInquiryHandler;
		}

		public ProgressReportRes GetProgressReport(ProgressReportReq request)
		{
			ProgressReportRes response = null;
			try
			{
				response = _reportHandler.getProgressReport(request); ;
			}
			catch (Exception ex)
			{
				response = new ProgressReportRes();
				response.IsRequestSuccess = false;
				response.Description = ex.Message;
			}
			return response;
		}
		public AddCourseRes AddNewCourse(AddCourseReq request)
		{
			AddCourseRes response = null;
			try
			{
				response = _courseInquiryHandler.addCourse(request);
			}
			catch (Exception ex)
			{
				response = new AddCourseRes();
				response.IsRequestSuccess = false;
				response.Description = ex.Message;
			}
			return response;
		}

		internal GetAllCoursesRes GetAllCourses(GetAllCoursesReq request)
		{
			GetAllCoursesRes response = null;
			try
			{
				response = _courseInquiryHandler.getAllUserCourses(request);
			}
			catch (Exception ex)
			{
				response = new GetAllCoursesRes();
				response.IsRequestSuccess = false;
				response.Description = ex.Message;
			}
			return response;

		}
	}
}
