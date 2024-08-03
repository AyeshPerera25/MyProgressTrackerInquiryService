using MyProgressTrackerAuthenticationService.Models.DataTransferObjects;
using MyProgressTrackerAuthenticationService.Models.Entities;
using MyProgressTrackerInquiryService.DataResources;
using MyProgressTrackerInquiryService.Models.DataTransferObjects;
using MyProgressTrackerInquiryService.Models.Entities;
using System.Transactions;

namespace MyProgressTrackerInquiryService.Handlers
{
	public class CourseInquiryHandler
	{
		private readonly AzuerSQLDBContext _dbContext;

		public CourseInquiryHandler(AzuerSQLDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		internal AddCourseRes addCourse(AddCourseReq request)
		{
			AddCourseRes response = new AddCourseRes();
			Course course;
			Session session = validateSession(request);
			validateAddCourseReq(request);
			course = populateCourse(request, session);
			persistCourseData(course,session);
			persistSessionUpdate(session);
			CommitData();

			response.IsRequestSuccess = true;
			response.Description = "Success!";
			response.course = course;
		
			return response;
		}

		private void CommitData()
		{
			_dbContext.SaveChanges();
		}

		private void persistCourseData(Course course, Session session)
		{
			if (course != null)
			{
				_dbContext.Courses.Add(course);
			}
			else
			{
				throw new Exception("Null Course Entity to persist");
			}
		}

		private Course populateCourse(AddCourseReq request, Session session)
		{
			Course course = new Course();
			course.CourseName = request.CourseName;
			course.CourseDescription = request.CourseDescription;
			course.CourseStartDate = request.CourseStartDate;
			course.CourseEndDate = request.CourseEndDate;
			course.NoOfSemesters = request.NoOfSemesters;
			course.UniversityName = request.UniversityName;
			course.UserId = session.UserId;
			course.User = session.User;
			return course;
		}

		private Session validateSession(RequestWrapper request)
		{
			Session session = null;
			if (request == null)
			{
				throw new Exception("Request is Null!");
			}
			if (request.SessionKey == null)
			{
				throw new Exception("Session Key Not Found!");
			}
			if (_dbContext.Sessions.Any())
			{
				List<Session> sessionList = _dbContext.Sessions.Where<Session>(sess => sess.SessionKey == request.SessionKey).ToList();
				if(sessionList.Any())
				{
					session = sessionList.First(sess => sess.UserId == request.UserId);
				}
			}
			else
			{
				throw new Exception("No any Session has registerd yet!!");
			}
			if (session == null)
			{
				throw new Exception("Invalid Session Key!");
			}
			if (!session.LoginStatus) 
			{
				throw new Exception("Session Inactivated!");
			}
			DateTime currentTime = DateTime.Now;
			TimeSpan timeDifference = currentTime - session.LastLoginTime;
			if (timeDifference.TotalMinutes > 30)
			{
				session.LoginStatus = false;
				_dbContext.Sessions.Update(session);
				_dbContext.SaveChanges();
				throw new Exception("Session Expired!");
			}
			return session;
		}

		private void validateAddCourseReq(AddCourseReq request)
		{
			if (request.CourseName == null || request.CourseName == "")
			{
				throw new Exception("New Course Name has not defined! ");
			}
			if (request.UniversityName == null || request.UniversityName == "")
			{
				throw new Exception("New Course Institute Name has not defined! ");
			}
			if (request.NoOfSemesters <= 0)
			{
				throw new Exception("Invalid No of Semesters!");
			}
			
		}

		internal GetAllCoursesRes? getAllUserCourses(GetAllCoursesReq request)
		{
			GetAllCoursesRes response = new GetAllCoursesRes();
			Session session = validateSession(request);
			validateGetAllCoursesReq(request);
			List<Course> courses = populateAllCourses(session);
			validateAllCourses(courses);
			persistSessionUpdate(session);
			CommitData();

			response.IsRequestSuccess = true;
			response.Description = "Success!";
			response.CourseList = courses;

			return response;
		}

		private void persistSessionUpdate(Session session)
		{
			if (session != null)
			{
				session.LastLoginTime = DateTime.Now;
				_dbContext.Sessions.Update(session);
			}
			else
			{
				throw new Exception("Null Session Entity to persist");
			}
			
		}

		private void validateAllCourses(List<Course> courses)
		{
			if (courses == null)
			{
				throw new Exception("Course Loading Error!");
			}
			if (courses.Count == 0)
			{
				throw new Exception("No Courses Has Found!");
			}
		}

		private List<Course> populateAllCourses(Session session)
		{
			return _dbContext.Courses.Where(course => course.UserId == session.UserId).ToList();
		}

		private void validateGetAllCoursesReq(GetAllCoursesReq request)
		{
			if (request.UserId <= 0L)
			{
				throw new Exception("Invalid User ID!");
			}
		}
	}
}
