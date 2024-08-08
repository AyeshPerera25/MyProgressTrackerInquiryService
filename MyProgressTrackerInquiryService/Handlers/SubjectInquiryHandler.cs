using MyProgressTrackerDependanciesLib.Models.DataTransferObjects;
using MyProgressTrackerDependanciesLib.Models.Entities;
using MyProgressTrackerInquiryService.DataResources;

namespace MyProgressTrackerInquiryService.Handlers
{
	public class SubjectInquiryHandler
	{
		private readonly AzuerSQLDBContext _dbContext;

		public SubjectInquiryHandler(AzuerSQLDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		//---------------------------------------------------------------------------------------------------------------------------------------- ( Subject Inquiry )----------------
		internal GetAllSubjectsRes getAllUserSubjects(GetAllSubjectsReq request) //============================== Get All Subjects
		{
			GetAllSubjectsRes response = new GetAllSubjectsRes();
			Session session = validateSession(request);
			validateReqUserID(request);

			List<Subject> subjects = populateAllSubjects(session);
			validateAllSubjects(subjects);
			persistSessionUpdate(session);
			CommitData();

			response.IsRequestSuccess = true;
			response.Description = "Success!";
			response.SubjectList = subjects;

			return response;
		}

		private void validateAllSubjects(List<Subject> subjects)
		{
			Course? course = null;
			if (subjects == null)
			{
				throw new Exception("Subject Loading Error!");
			}
			if (subjects.Count == 0)
			{
				throw new Exception("No Subject Has Found!");
			}
			foreach (Subject subject in subjects)
			{
				course = null;
				if (subject.Course == null)
                {
					course = _dbContext.Courses.SingleOrDefault<Course>(cours => cours.CourseId == subject.CourseId);
					if (course == null)
					{
						throw new Exception("Unable to load Course for Subject: " + subject.SubjectName);
					}
					subject.Course = course;
				}
			}
		}

		private List<Subject> populateAllSubjects(Session session)
		{
			if (_dbContext.Subjects.Any())
			{
				return _dbContext.Subjects.Where(sub => sub.Course.UserId == session.UserId).ToList();
			}
			else
			{
				throw new Exception("No Subject Has Registerd Yet!");
			}
		}

		internal AddNewSubjectRes addSubject(AddNewSubjectReq request)  //============================== Add Subject
		{
			AddNewSubjectRes response = new AddNewSubjectRes();
			Course course;
			Subject subject;
			Session session = validateSession(request);
			course = validateAddSubjectReq(request, session);
			subject = populateSubjectsData(request, course);
			persistSubjectData(subject);
			persistSessionUpdate(session);
			CommitData();

			response.IsRequestSuccess = true;
			response.Description = "Success!";
			response.subject = subject;
			return response;
		}

		private void persistSubjectData(Subject subject)
		{
			if (subject != null)
			{
				_dbContext.Subjects.Add(subject);
			}
			else
			{
				throw new Exception("Null Subject Entity to persist");
			}
		}

		private Subject populateSubjectsData(AddNewSubjectReq request, Course course)
		{
			Subject subject = new Subject();
			subject.SubjectName = request.SubjectName;
			subject.Course = course;
			subject.CourseId = course.CourseId;
			subject.SemesterNo = request.SemesterNo;
			subject.SemesterStartDate = request.SemesterStartDate;
			subject.SemesterEndDate = request.SemesterEndDate;
			subject.SubjectDescription = request.Description;
			return subject;
		}

		private Course validateAddSubjectReq(AddNewSubjectReq request, Session session)
		{
			Course? course = null;

			if (request.SubjectName == null || request.SubjectName == string.Empty)
			{
				throw new Exception("New Subject Name has not defined! ");
			}
			if (request.SemesterNo <= 0)
			{
				throw new Exception("Invalid Semesters No!");
			}
			if (request.CourseID <= 0)
			{
				throw new Exception("Invalid Subject Course Id!");
			}
			if (request.SemesterStartDate == null)
			{
				throw new Exception("Semester Start Date Not Defined!");
			}
			if (request.SemesterEndDate == null)
			{
				throw new Exception("Semester End Date Not Defined!");
			}
			if(_dbContext.Courses.Any())
			{
				course = _dbContext.Courses.SingleOrDefault<Course>(course => course.CourseId == request.CourseID);
				if (course == null)
				{
					throw new Exception("No Course found under the Id: " + request.CourseID);
				}
				if (course.UserId != session.UserId)
				{
					throw new Exception("Course not belongs to the requested User, Course Id:  " + request.CourseID);
				}
			}
			else
			{
				throw new Exception("No Course Has Registerd Yet!");
			}

			return course;
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
