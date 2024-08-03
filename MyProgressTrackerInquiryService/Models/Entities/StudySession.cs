using MyProgressTrackerAuthenticationService.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace MyProgressTrackerInquiryService.Models.Entities
{
	public class StudySession
	{
		[Key]
		[Required]
		public int StudySessionId { get; set; }
		[Required]
		public string SessionName { get; set; }
		[Required]
		public int SubjectId { get; set; }
		[Required]
		public Subject Subject { get; set; }
		[Required]
		public DateTime SessionStartTime { get; set; }
		[Required]
		public DateTime SessionEndTime { get; set; }
		public string? Description { get; set; }

		// Default constructor
		public StudySession() { }

		public StudySession(int studySessionId, string sessionName, int subjectId, Subject subject, DateTime sessionStartTime, DateTime sessionEndTime, string? description)
		{
			StudySessionId = studySessionId;
			SessionName = sessionName;
			SubjectId = subjectId;
			Subject = subject;
			SessionStartTime = sessionStartTime;
			SessionEndTime = sessionEndTime;
			Description = description;
		}
	}
}
