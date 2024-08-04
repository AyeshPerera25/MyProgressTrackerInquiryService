using MyProgressTrackerDependanciesLib.Models.Entities;
using MyProgressTrackerInquiryService.DataResources;
using MyProgressTrackerDependanciesLib.Models.DataTransferObjects;

namespace MyProgressTrackerInquiryService.Handlers
{
	public class ReportInquiryHandler
	{
		private readonly AzuerSQLDBContext _dbContext;

		public ReportInquiryHandler(AzuerSQLDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		internal ProgressReportRes getProgressReport(ProgressReportReq request)
		{
			ProgressReport report;
			ProgressReportRes response = new ProgressReportRes();
			List<ProgressReport> reportList = new List<ProgressReport>();
			List<StudySession> sessions;
			List<Subject> subjects;
			List<StudySession> filteredSessions;
			TimeSpan totalStudyTime;

			User user = validateProgressReportRequest(request);
			sessions = populateUserStudySessions(user);
			subjects = populateSubjects(user);
			if (sessions.Count > 0)
			{
				subjects.ForEach(sub =>
				{
					report = new ProgressReport();
					filteredSessions = sessions.FindAll(sess1 => sess1.SubjectId == sub.SubjectId).ToList();
					totalStudyTime = TimeSpan.Zero;
					filteredSessions.ForEach(sess2 =>
					{
						totalStudyTime = totalStudyTime + (sess2.SessionEndTime - sess2.SessionStartTime);
					});
					report.NoOfSessions = filteredSessions.Count;
					report.CourseName = sub.Course.CourseName;
					report.SubjectName = sub.SubjectName;
					TimeSpan totalTimeForSubject = sub.SemesterEndDate - sub.SemesterStartDate;
					report.TotalCourseDuration = totalTimeForSubject;
					report.TotalStudyDuration = totalStudyTime;
					report.Score = (totalStudyTime.TotalHours / totalTimeForSubject.TotalHours) * 100;
					reportList.Add(report);
				});
				response.IsRequestSuccess = true;
				response.Description = "Success!";
				
			}
			else
			{
				response.IsRequestSuccess = false;
				response.Description = "No study sessions found!";
			}
			response.ProgressReports = reportList;
			return response;
		}

		private List<Subject> populateSubjects(User user)
		{
			Course course;
			List<Subject> subjectList = _dbContext.Subjects.Where(sub => sub.Course.UserId == user.UserId).ToList();
			if (subjectList.Count > 0)
			{
				foreach (Subject subject in subjectList)
				{
					course = _dbContext.Courses.SingleOrDefault<Course>( cours => cours.CourseId == subject.CourseId);
					if (course == null)
					{
						throw new Exception("Course not found for CourseID: " + subject.CourseId);
					}
					subject.Course = course;	
				}
			}
			return subjectList;
		}

		private List<StudySession> populateUserStudySessions(User user)
		{
			if (_dbContext.StudySessions.Any())
			{
				return _dbContext.StudySessions.Where(session => session.Subject.Course.UserId == user.UserId).ToList();
			}
			else
			{
				throw new Exception("No StudySessions Has Registerd Yet!");
			}
		}

		private User validateProgressReportRequest(ProgressReportReq request)
		{
			User user = null;
			if (request == null)
			{
				throw new Exception("Null Progress Report Request!");
			}
			if(request.UserId <= 0L)
			{
				throw new Exception("Invalide User ID!");
			}
			if (_dbContext.Users.Any())
			{
				user = _dbContext.Users.SingleOrDefault<User>(user => user.UserId == request.UserId);
			}
			else
			{
				throw new Exception("No any User has registerd yet!");
			}
			if (user == null)
			{
				throw new Exception("No user found under the ID: " + request.UserId);
			}
			if(!_dbContext.StudySessions.Any())
			{
				throw new Exception("No any Study Sessions Logged!");
			}
			return user;
		}
	}
}
