using MyProgressTrackerDependanciesLib.Models.DataTransferObjects;
using MyProgressTrackerDependanciesLib.Models.Entities;
using MyProgressTrackerInquiryService.DataResources;
using System.Linq;

namespace MyProgressTrackerInquiryService.Handlers
{
	public class StudySessionInquiryHandler
	{
		private readonly AzuerSQLDBContext _dbContext;

		public StudySessionInquiryHandler(AzuerSQLDBContext dbContext)
		{
			_dbContext = dbContext;
		}
		//---------------------------------------------------------------------------------------------------------------------------------------- ( Study Session Inquiry )----------------
		internal AddStudySessionRes addStudySession(AddStudySessionReq request) //============================== Add Study Session
		{ 
			AddStudySessionRes response = new AddStudySessionRes();
			Subject subject;
			StudySession studySession;
			Session session = validateSession(request);
			subject = validateAddStudySessionReq(request, session);
			studySession = populateStudySessionData(request, subject);
			persistStudySessio0nData(studySession);
			persistSessionUpdate(session);
			CommitData();

			response.IsRequestSuccess = true;
			response.Description = "Success!";
			response.StudySession = studySession;
			return response;
		}

		private void persistStudySessio0nData(StudySession studySession)
		{
			if (studySession != null)
			{
				_dbContext.StudySessions.Add(studySession);
			}
			else
			{
				throw new Exception("Null Study Session Entity to persist");
			}
		}

		private StudySession populateStudySessionData(AddStudySessionReq request, Subject subject)
		{
			StudySession studySession = new StudySession();
			studySession.SessionName = request.SessionName;
			studySession.SubjectId = request.SubjectID;
			studySession.Subject = subject;
			studySession.SessionStartTime = request.SessionStartTime;
			studySession.SessionEndTime = request.SessionEndTime;
			studySession.Description = request.SessionDescription;
			return studySession;
		}

		private Subject validateAddStudySessionReq(AddStudySessionReq request, Session session)
		{
			Course? course = null;
			Subject? subject = null;

			if (request.SessionName == null || request.SessionName == string.Empty)
			{
				throw new Exception("New Study Session Name has not defined! ");
			}

			if (request.SubjectID <= 0)
			{
				throw new Exception("Invalid Study Session Subject Id!");
			}

			if (request.SessionStartTime == null)
			{
				throw new Exception("Study Session Start Time Not Defined!");
			}

			if (request.SessionEndTime == null)
			{
				throw new Exception("Study Session End Time Not Defined!");
			}

			if (_dbContext.Subjects.Any())
			{
				subject = _dbContext.Subjects.SingleOrDefault<Subject>( sub => sub.SubjectId == request.SubjectID);
			}
			else
			{
				throw new Exception("No Subject Has Registerd Yet!");
			}

			if (subject == null)
			{
				throw new Exception("No Subject found under the Id: " + request.SubjectID);
			}

			if (subject.Course == null)
			{
				if (_dbContext.Courses.Any())
				{
					course = _dbContext.Courses.SingleOrDefault<Course>(cours => cours.CourseId == subject.CourseId);
				}
				else
				{
					throw new Exception("No Course Has Registerd Yet!");
				}

				if(course == null)
				{
					throw new Exception("No Course found under the Id: " + subject.CourseId);
				}
				subject.Course = course;
			}
			return subject;
		}

		internal GetAllStudySessionsRes getAllUserStudySessions(GetAllStudySessionsReq request) //============================== Get All Study Sessions
		{
			GetAllStudySessionsRes response = new GetAllStudySessionsRes();
			Session session = validateSession(request);
			validateReqUserID(request);
			List<StudySession> studySessionList = populateAllStudySessions(session);
			validateAllStudySessions(studySessionList);
			persistSessionUpdate(session);
			CommitData();

			response.IsRequestSuccess = true;
			response.Description = "Success!";
			response.StudySessionsList = studySessionList;
			return response;
		}

		private void validateAllStudySessions(List<StudySession> studySessionList)
		{
			Course? course = null;
			Subject? subject = null;
			if (studySessionList == null)
			{
				throw new Exception("Study Sessions Loading Error!");
			}
			if (studySessionList.Count == 0)
			{
				throw new Exception("No Study Session Has Found!");
			}
			foreach (StudySession studySession in studySessionList)
			{
				course = null;
				subject = null;
				if (studySession.Subject == null)
				{
					subject = _dbContext.Subjects.SingleOrDefault<Subject>( sub => sub.SubjectId == studySession.SubjectId);
					if (subject == null)
					{
						throw new Exception("Unable to load Subject for Study Session: " + studySession.SessionName);
					}
					studySession.Subject = subject;	
				}
				if(studySession.Subject.Course == null)
				{
					course = _dbContext.Courses.SingleOrDefault<Course>(cours => cours.CourseId == studySession.Subject.CourseId);
					if (course == null)
					{
						throw new Exception("Unable to load Course for Study Session: " + studySession.SessionName);
					}
					studySession.Subject.Course = course;
				}
			}
		}

		private List<StudySession> populateAllStudySessions(Session session)
		{
			if (_dbContext.StudySessions.Any())
			{
				return _dbContext.StudySessions.Where(sub => sub.Subject.Course.User.UserId == session.UserId).ToList();
			}
			else
			{
				throw new Exception("No Study Session Has Registerd Yet!");
			}
		}

		//---------------------------------------------------------------------------------------------------------------------------------------- ( Commen Methods )----------------
		private void validateReqUserID(RequestWrapper request)
		{
			if (request.UserId <= 0L)
			{
				throw new Exception("Invalid User ID!");
			}
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
				if (sessionList.Any())
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

		private void CommitData()
		{
			_dbContext.SaveChanges();
		}
	}
}
