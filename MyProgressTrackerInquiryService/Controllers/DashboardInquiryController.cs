using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProgressTrackerDependanciesLib.Models.DataTransferObjects;
using MyProgressTrackerInquiryService.Services;

namespace MyProgressTrackerInquiryService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DashboardInquiryController : Controller
	{
		private DashboardInquiryServiceCore _serviceCore;

		public DashboardInquiryController(DashboardInquiryServiceCore serviceCore)
		{
			_serviceCore = serviceCore;
		}

		[HttpGet("getProgressReport")]
		public ActionResult<ProgressReportRes> GetProgressReport([FromBody] ProgressReportReq request)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Request is null");
				}
				return Ok(_serviceCore.GetProgressReport(request));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpPost("getAllCourses")]
		public ActionResult<GetAllCoursesRes> GetAllCourses([FromBody] GetAllCoursesReq request)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Request is null");
				}
				return Ok(_serviceCore.GetAllCourses(request));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("addNewCourse")]
		public ActionResult<AddCourseRes> AddNewCourse([FromBody] AddCourseReq request)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Request is null");
				}
				return Ok(_serviceCore.AddNewCourse(request));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


	}
}
