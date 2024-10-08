﻿using Microsoft.EntityFrameworkCore;
using MyProgressTrackerDependanciesLib.Models.DataTransferObjects;
using MyProgressTrackerDependanciesLib.Models.Entities;
using MyProgressTrackerInquiryService.DataResources;
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

		//---------------------------------------------------------------------------------------------------------------------------------------- ( Course Inquiry )----------------

		internal AddCourseRes addCourse(AddCourseReq request) //============================== Add Course
		{
			AddCourseRes response = new AddCourseRes();
			Course course;
			Session session = validateSession(request);
			validateAddCourseReq(request);
			course = populateCourse(request, session);
			persistCourseData(course);
			persistSessionUpdate(session);
			CommitData();

			response.IsRequestSuccess = true;
			response.Description = "Success!";
			response.course = course;
		
			return response;
		}

		private void persistCourseData(Course course)
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

		internal GetAllCoursesRes getAllUserCourses(GetAllCoursesReq request) //============================== Get All Courses
		{
			GetAllCoursesRes response = new GetAllCoursesRes();
			Session session = validateSession(request);
			validateReqUserID(request);
			List<Course> courses = populateAllCourses(session);
			validateAllCourses(courses);
			persistSessionUpdate(session);
			CommitData();

			response.IsRequestSuccess = true;
			response.Description = "Success!";
			response.CourseList = courses;

			return response;
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
			if (_dbContext.Courses.Any())
			{
				return _dbContext.Courses.Where(course => course.UserId == session.UserId).ToList();
			}
			else
			{
				throw new Exception("No Courses Has Registerd Yet!");
			}
		}
        public CommenResponse deleteCourse(DeleteCoursReq request) //============================== Delete Course
        {
            CommenResponse response = new CommenResponse();
            Session session = validateSession(request);
            validateCourseDeleteRequest(request);
            Course course = populateCourseDelete(request);

			persistDeleteCourse(course);
            persistSessionUpdate(session);
            CommitData();

            response.IsRequestSuccess = true;
            response.Description = "Success!";
           
            return response;
        }

        private void persistDeleteCourse(Course course)
        {
            if (course != null)
            {
                _dbContext.Courses.Remove(course);
            }
            else
            {
                throw new Exception("Null Course Entity to persist");
            }
        }

        private void validateCourseDeleteRequest(DeleteCoursReq request)
        {
            if (request.CourseId <= 0L)
            {
                throw new Exception("Invalide Course Id to Delete");
            }
        }

        private Course populateCourseDelete(DeleteCoursReq request)
        {
			Course course = null;
            if (_dbContext.Courses.Any())
            {
                course =  _dbContext.Courses.SingleOrDefault(course => course.CourseId == request.CourseId);
            }
            else
            {
                throw new Exception("No Courses Has Registerd Yet!");
            }
			if (course == null)
			{
                throw new Exception("No Courses Has found under Course ID: "+request.CourseId);
            }
			
			return course;
        }

        public CommenResponse deleteSubject(DeleteSubjectReq request)
        {
            CommenResponse response = new CommenResponse();
            Session session = validateSession(request);
            validateSubjectDeleteRequest(request);
            Subject subject = populateSubjectDelete(request);

            persistDeleteSubject(subject);
            persistSessionUpdate(session);
            CommitData();

            response.IsRequestSuccess = true;
            response.Description = "Success!";

            return response;
        }

        private void persistDeleteSubject(Subject subject)
        {
            if (subject != null)
            {
                _dbContext.Subjects.Remove(subject);
            }
            else
            {
                throw new Exception("Null Course Entity to persist");
            }
        }

        private Subject populateSubjectDelete(DeleteSubjectReq request)
        {
            Subject sub = null;

            if (_dbContext.Subjects.Any())
            {
                sub = _dbContext.Subjects.SingleOrDefault(subject => subject.SubjectId == request.SubjectId);
            }
            else
            {
                throw new Exception("No Subject Has Registerd Yet!");
            }
            if (sub == null)
            {
                throw new Exception("No Subject Has found under Subject ID: " + request.SubjectId);
            }

            Course course = null;
            if (_dbContext.Courses.Any())
            {
                course = _dbContext.Courses.SingleOrDefault(course => course.CourseId == sub.CourseId);
            }
            else
            {
                throw new Exception("No Courses Has Registerd Yet!");
            }
            if (course == null)
            {
                throw new Exception("No Courses Has found under Course ID: " + sub.CourseId);
            }

			sub.Course = course;

            return sub;
        }

        private void validateSubjectDeleteRequest(DeleteSubjectReq request)
        {
            if (request.SubjectId <= 0L)
            {
                throw new Exception("Invalide Subject Id to Delete");
            }
        }

        public CommenResponse deleteSession(DeleteStudySessionReq request)
        {
            CommenResponse response = new CommenResponse();
            Session session = validateSession(request);
            validateStudySessionDeleteRequest(request);
            StudySession studySession = populateStudySessionDelete(request);

            persistDeleteStudySession(studySession);
            persistSessionUpdate(session);
            CommitData();

            response.IsRequestSuccess = true;
            response.Description = "Success!";

            return response;
        }

        private void persistDeleteStudySession(StudySession studySession)
        {
            if (studySession != null)
            {
                _dbContext.StudySessions.Remove(studySession);
            }
            else
            {
                throw new Exception("Null Course Entity to persist");
            }
        }

        private StudySession populateStudySessionDelete(DeleteStudySessionReq request)
        {
            StudySession session = null;
            if (_dbContext.Subjects.Any())
            {
                session = _dbContext.StudySessions.SingleOrDefault(session => session.SubjectId == request.StudySessionId);
            }
            else
            {
                throw new Exception("No Study Session Has Registerd Yet!");
            }
            if (session == null)
            {
                throw new Exception("No Study Session Has found under Session ID: " + request.StudySessionId);
            }
            Subject sub = null;

            if (_dbContext.Subjects.Any())
            {
                sub = _dbContext.Subjects.SingleOrDefault(subject => subject.SubjectId == session.SubjectId);
            }
            else
            {
                throw new Exception("No Subject Has Registerd Yet!");
            }
            if (sub == null)
            {
                throw new Exception("No Subject Has found under Subject ID: " + session.SubjectId);
            }

            Course course = null;
            if (_dbContext.Courses.Any())
            {
                course = _dbContext.Courses.SingleOrDefault(course => course.CourseId == sub.CourseId);
            }
            else
            {
                throw new Exception("No Courses Has Registerd Yet!");
            }
            if (course == null)
            {
                throw new Exception("No Courses Has found under Course ID: " + sub.CourseId);
            }

            sub.Course = course;
            session.Subject = sub;

            return session;
        }

        private void validateStudySessionDeleteRequest(DeleteStudySessionReq request)
        {
            if (request.StudySessionId <= 0L)
            {
                throw new Exception("Invalide Study Session Id to Delete");
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
