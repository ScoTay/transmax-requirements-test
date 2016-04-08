using System;
using System.Collections.Generic;
using System.Text;

namespace GradeScores
{
	public class StudentCohort
	{
		// Exception messages for test framework
		public const string ExceptionIncorrectNumberOfTokens = "Incorrect number of tokens";
		public const string ExceptionInvalidGrade = "Invalid grade";
		public const string ExceptionNoDataProvided = "No data provided";

		///////////////////////////////////////////////////////////////////////

		public struct Student
		{
			public string lastName;
			public string firstName;
			public int grade;
		}

		///////////////////////////////////////////////////////////////////////

		private List<Student> m_students = new List<Student>();

		///////////////////////////////////////////////////////////////////////

		public List<Student> Students { get { return m_students; } }

		///////////////////////////////////////////////////////////////////////

		public StudentCohort(string csvData)
		{
			m_students = ParseCsvStudentList(csvData);
		}

		///////////////////////////////////////////////////////////////////////
		//
		// Sorts student data in-place by grade, last name, first name
		// Results are accessible via Students property or GenerateCsv()
		//
		public void SortByGradeDesc()
		{
			m_students.Sort((a,b) => {
				var ret = b.grade.CompareTo(a.grade);
				if (ret == 0)
				{
					ret = a.lastName.CompareTo(b.lastName);
				}
				if (ret == 0)
				{
					ret = a.firstName.CompareTo(b.firstName);
				}
				return ret;
			});
		}

		///////////////////////////////////////////////////////////////////////
		//
		// Outputs list of students in their current sort order
		// Format: LASTNAME, FIRSTNAME, GRADE
		//
		public string GenerateCsv()
		{
			// Note: Normally we'd use a library to write CSV files, but for the purpose of this test, do it manually
			// Warning: Double quotes are not treated according to CSV spec!
			StringBuilder builder = new StringBuilder();
			foreach (Student student in m_students)
			{
				builder.AppendFormat("{0}, {1}, {2}", student.lastName, student.firstName, student.grade);
				builder.AppendLine();
			}
			return builder.ToString();
		}

		///////////////////////////////////////////////////////////////////////
		//
		// Generates a list of students from CSV data
		// csvData: string containing CSV data; format: LASTNAME, FIRSTNAME, GRADE
		//
		private static List<Student> ParseCsvStudentList(string csvData)
		{
			List<Student> outData = new List<Student>();

			// Note: Normally we'd use a library to parse CSV files, but for the purpose of this test, do it manually
			string[] lines = csvData.Split('\n');
			for (int lineIndex = 0; lineIndex < lines.Length; ++lineIndex)
			{
				string line = lines[lineIndex];
				if (!string.IsNullOrWhiteSpace(line))  // Silently discard blank lines
				{
					string[] tokens = line.Split(',');

					if (tokens.Length != 3)
					{
						throw new FormatException(string.Format("{0} (line {1})", ExceptionIncorrectNumberOfTokens, lineIndex));
					}

					// Note: Allow first/last name to be blank; spec doesn't require them to be valid
					// Warning: Does not parse double quotes according to CSV spec!
					string lastName = tokens[0].Trim();
					string firstName = tokens[1].Trim();

					// Spec: Grade appears to be between 1..100, but no range limit is given, so don't impose one
					//   However this will fail tests on grades outside 32 bit range
					int grade;
					if (!Int32.TryParse(tokens[2].Trim(), out grade))
					{
						throw new FormatException(string.Format("{0} (line {1})", ExceptionInvalidGrade, lineIndex));
					}

					Student student = new Student
					{
						lastName = lastName,
						firstName = firstName,
						grade = grade,
					};

					outData.Add(student);
				}
			}

			if (outData.Count <= 0)
			{
				throw new FormatException(ExceptionNoDataProvided);
			}

			return outData;
		}
	}
}
