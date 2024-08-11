
using MyProgressTrackerDependanciesLib.Models.DataTransferObjects;
using MyProgressTrackerInquiryService.Handlers;
using MyProgressTrackerDependanciesLib.Models.DataTransferObjects;

namespace MyProgressTrackerInquiryService.Services
{
	public class DashboardInquiryServiceCore 
	{
		private readonly ReportInquiryHandler _reportHandler;
		private readonly CourseInquiryHandler _courseInquiryHandler;
		private readonly SubjectInquiryHandler _subjectInquiryHandler;
		private readonly StudySessionInquiryHandler _studySessionInquiryHandler;

		public DashboardInquiryServiceCore(ReportInquiryHandler reportHandler, CourseInquiryHandler courseInquiryHandler, SubjectInquiryHandler subjectInquiryHandler, StudySessionInquiryHandler studySessionInquiryHandler)
		{
			_reportHandler = reportHandler;
			_courseInquiryHandler = courseInquiryHandler;
			_subjectInquiryHandler = subjectInquiryHandler;
			_studySessionInquiryHandler = studySessionInquiryHandler;
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

		internal GetAllSubjectsRes GetAllSubjects(GetAllSubjectsReq request)
		{
			GetAllSubjectsRes response = null;
			try
			{
				response = _subjectInquiryHandler.getAllUserSubjects(request);
			}
			catch (Exception ex)
			{
				response = new GetAllSubjectsRes();
				response.IsRequestSuccess = false;
				response.Description = ex.Message;
			}
			return response;
		}

		internal AddNewSubjectRes AddNewSubject(AddNewSubjectReq request)
		{
			AddNewSubjectRes response = null;
			try
			{
				response = _subjectInquiryHandler.addSubject(request);
			}
			catch (Exception ex)
			{
				response = new AddNewSubjectRes();
				response.IsRequestSuccess = false;
				response.Description = ex.Message;
			}
			return response;
		}

		internal GetAllStudySessionsRes GetAllStudySessions(GetAllStudySessionsReq request)
		{
			GetAllStudySessionsRes response = null;
			try
			{
				response = _studySessionInquiryHandler.getAllUserStudySessions(request);
			}
			catch (Exception ex)
			{
				response = new GetAllStudySessionsRes();
				response.IsRequestSuccess = false;
				response.Description = ex.Message;
			}
			return response;
		}

		internal AddStudySessionRes AddNewStudySession(AddStudySessionReq request)
		{
			AddStudySessionRes response = null;
			try
			{
				response = _studySessionInquiryHandler.addStudySession(request);
			}
			catch (Exception ex)
			{
				response = new AddStudySessionRes();
				response.IsRequestSuccess = false;
				response.Description = ex.Message;
			}
			return response;
		}

        internal CommenResponse deleteCourse(DeleteCoursReq request)
        {
            CommenResponse response = null;
            try
            {
                response = _courseInquiryHandler.deleteCourse(request);
            }
            catch (Exception ex)
            {
                response = new CommenResponse();
                response.IsRequestSuccess = false;
                response.Description = ex.Message;
            }
            return response;
        }

        internal object? deleteSubject(DeleteSubjectReq request)
        {


            CommenResponse response = null;
            try
            {
                response = _courseInquiryHandler.deleteSubject(request);
            }
            catch (Exception ex)
            {
                response = new CommenResponse();
                response.IsRequestSuccess = false;
                response.Description = ex.Message;
            }
            return response;
        }

        internal object? deleteStudySession(DeleteStudySessionReq request)
        {

            CommenResponse response = null;
            try
            {
                response = _courseInquiryHandler.deleteSession(request);
            }
            catch (Exception ex)
            {
                response = new CommenResponse();
                response.IsRequestSuccess = false;
                response.Description = ex.Message;
            }
            return response;
        }
    }
}
