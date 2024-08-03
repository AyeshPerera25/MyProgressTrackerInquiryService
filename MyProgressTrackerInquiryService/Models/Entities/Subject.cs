using System.ComponentModel.DataAnnotations;

namespace MyProgressTrackerInquiryService.Models.Entities
{
	public class Subject
	{
		[Key]
		[Required]
		public int SubjectId { get; set; }
		[Required]
		public string SubjectName { get; set; }
		[Required]
		public int CourseId { get; set; }
		[Required]
		public Course Course { get; set; }
		public string? SubjectDescription { get; set; }
		[Required]
		public int SemesterNo { get; set; }
		[Required]
		public DateTime SemesterStartDate { get; set; }
		[Required]
		public DateTime SemesterEndDate { get; set; }

		// Default constructor
		public Subject() { }

		public Subject(int subjectId, string subjectName, int courseId, Course course, string? subjectDescription, int semesterNo, DateTime semesterStartDate, DateTime semesterEndDate)
		{
			SubjectId = subjectId;
			SubjectName = subjectName;
			CourseId = courseId;
			Course = course;
			SubjectDescription = subjectDescription;
			SemesterNo = semesterNo;
			SemesterStartDate = semesterStartDate;
			SemesterEndDate = semesterEndDate;
		}
	}
}
