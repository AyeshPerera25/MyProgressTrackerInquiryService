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

		[HttpPost("getProgressReport")]
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

		[HttpPost("getAllSubjects")]
		public ActionResult<GetAllSubjectsRes> GetAllSubjects([FromBody] GetAllSubjectsReq request)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Request is null");
				}
				return Ok(_serviceCore.GetAllSubjects(request));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("addNewSubject")]
		public ActionResult<AddNewSubjectRes> AddNewSubject([FromBody] AddNewSubjectReq request)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Request is null");
				}
				return Ok(_serviceCore.AddNewSubject(request));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("getAllStudySessions")]
		public ActionResult<GetAllStudySessionsRes> GetAllSubjects([FromBody] GetAllStudySessionsReq request)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Request is null");
				}
				return Ok(_serviceCore.GetAllStudySessions(request));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("addNewStudySession")]
		public ActionResult<AddStudySessionRes> AddNewStudySession([FromBody] AddStudySessionReq request)
		{
			try
			{
				if (request == null)
				{
					return BadRequest("Request is null");
				}
				return Ok(_serviceCore.AddNewStudySession(request));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



	}
}
