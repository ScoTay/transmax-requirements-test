using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GradeScores;

namespace GradeScoresTests
{
	[TestClass]
	public class StudentCohortTests
	{
		///////////////////////////////////////////////////////////////////////
		//
		// Unit tests for StudentCohort
		// Use small hard-coded data sets to test specific edge cases
		//

		[TestMethod]
		public void StudentCohort_TokensAreTooMany()
		{
			string csvData = "Smith,John,10,extra";
			try
			{
				var cohort = new StudentCohort(csvData);
			}
			catch (FormatException ex)
			{
				StringAssert.Contains(ex.Message, StudentCohort.ExceptionIncorrectNumberOfTokens);
				return;
			}
			Assert.Fail("No exception was thrown.");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_TokensAreTooFew()
		{
			string csvData = "John,10";
			try
			{
				var cohort = new StudentCohort(csvData);
			}
			catch (FormatException ex)
			{
				StringAssert.Contains(ex.Message, StudentCohort.ExceptionIncorrectNumberOfTokens);
				return;
			}
			Assert.Fail("No exception was thrown.");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_GradeIsString()
		{
			string csvData = "Smith,John,StringHere";
			try
			{
				var cohort = new StudentCohort(csvData);
			}
			catch (FormatException ex)
			{
				StringAssert.Contains(ex.Message, StudentCohort.ExceptionInvalidGrade);
				return;
			}
			Assert.Fail("No exception was thrown.");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_GradeIsMissing()
		{
			string csvData = "Smith,John,";
			try
			{
				var cohort = new StudentCohort(csvData);
			}
			catch (FormatException ex)
			{
				StringAssert.Contains(ex.Message, StudentCohort.ExceptionInvalidGrade);
				return;
			}
			Assert.Fail("No exception was thrown.");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_GradeIsWhitespace()
		{
			string csvData = "Smith,John,\t  ";
			try
			{
				var cohort = new StudentCohort(csvData);
			}
			catch (FormatException ex)
			{
				StringAssert.Contains(ex.Message, StudentCohort.ExceptionInvalidGrade);
				return;
			}
			Assert.Fail("No exception was thrown.");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_GradeIsFloat()
		{
			string csvData = "Smith,John,19.99";
			try
			{
				var cohort = new StudentCohort(csvData);
			}
			catch (FormatException ex)
			{
				StringAssert.Contains(ex.Message, StudentCohort.ExceptionInvalidGrade);
				return;
			}
			Assert.Fail("No exception was thrown.");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_DataIsEmpty()
		{
			try
			{
				var cohort = new StudentCohort(string.Empty);
			}
			catch (FormatException ex)
			{
				StringAssert.Contains(ex.Message, StudentCohort.ExceptionNoDataProvided);
				return;
			}
			Assert.Fail("No exception was thrown.");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_DataIsBlankLines()
		{
			string csvData = "\n\n\n\n\n";
			try
			{
				var cohort = new StudentCohort(csvData);
			}
			catch (FormatException ex)
			{
				StringAssert.Contains(ex.Message, StudentCohort.ExceptionNoDataProvided);
				return;
			}
			Assert.Fail("No exception was thrown.");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_DataIsWhitespaceLines()
		{
			string csvData = "   \n\t\n   \t    \n";
			try
			{
				var cohort = new StudentCohort(csvData);
			}
			catch (FormatException ex)
			{
				StringAssert.Contains(ex.Message, StudentCohort.ExceptionNoDataProvided);
				return;
			}
			Assert.Fail("No exception was thrown.");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_DataContainsMixedNewlines()
		{
			string csvData = "Smith,John,10\r\nDoe,Jane,20\nCitizen,Jack,30";
			var cohort = new StudentCohort(csvData);
			// Cursory checks to make sure the data parsed successfully around the different newlines
			Assert.AreEqual(cohort.Students.Count, 3);
			Assert.AreEqual(cohort.Students[0].grade, 10);
			Assert.AreEqual(cohort.Students[1].lastName, "Doe");
			Assert.AreEqual(cohort.Students[1].grade, 20);
			Assert.AreEqual(cohort.Students[2].lastName, "Citizen");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_DataContainsBlankLines()
		{
			string csvData = "Smith,John,10\n\nDoe,Jane,20\n\n\nCitizen,Jack,30";
			var cohort = new StudentCohort(csvData);
			// Cursory checks to make sure the data parsed successfully
			Assert.AreEqual(cohort.Students.Count, 3);
			Assert.AreEqual(cohort.Students[0].lastName, "Smith");
			Assert.AreEqual(cohort.Students[1].lastName, "Doe");
			Assert.AreEqual(cohort.Students[2].lastName, "Citizen");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_DataContainsWhitespaceLines()
		{
			string csvData = "Smith,John,10\n         \nDoe,Jane,20\n\t    \t    \n\nCitizen,Jack,30";
			var cohort = new StudentCohort(csvData);
			// Cursory checks to make sure the data parsed successfully
			Assert.AreEqual(cohort.Students.Count, 3);
			Assert.AreEqual(cohort.Students[0].lastName, "Smith");
			Assert.AreEqual(cohort.Students[1].lastName, "Doe");
			Assert.AreEqual(cohort.Students[2].lastName, "Citizen");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_SortWithDifferentGrades()
		{
			string csvData = "Smith,John,10\nDoe,Jane,20\nCitizen,Jack,30";
			var cohort = new StudentCohort(csvData);
			// Cursory checks to make sure the data sorted successfully
			cohort.SortByGradeDesc();
			Assert.AreEqual(cohort.Students.Count, 3);
			Assert.AreEqual(cohort.Students[0].grade, 30);
			Assert.AreEqual(cohort.Students[1].grade, 20);
			Assert.AreEqual(cohort.Students[2].grade, 10);
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_SortWithMatchingGrades()
		{
			// Spec: Sort by grade, then by last name, then by first name
			string csvData = "Smith,John,10\nSmith,Jane,10\nCitizen,Jack,10\nCitizen,Albert,10";
			var cohort = new StudentCohort(csvData);
			// Cursory checks to make sure the data sorted successfully
			cohort.SortByGradeDesc();
			Assert.AreEqual(cohort.Students.Count, 4);
			Assert.AreEqual(cohort.Students[0].firstName, "Albert");
			Assert.AreEqual(cohort.Students[1].firstName, "Jack");
			Assert.AreEqual(cohort.Students[2].firstName, "Jane");
			Assert.AreEqual(cohort.Students[3].firstName, "John");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		public void StudentCohort_GenerateCsv()
		{
			// Simple check of GenerateCsv output
			string csvData = "Smith,John,10\nDoe,Jane,20";
			var cohort = new StudentCohort(csvData);

			string generatedCsv = cohort.GenerateCsv();
			// Force newlines to be \n so string comparison doesn't fail this test on different platforms
			generatedCsv = generatedCsv.Replace("\r\n", "\n").Replace("\r", "\n");
			string expectedCsv = "Smith, John, 10\nDoe, Jane, 20\n";  // Spec example: Output includes spaces after commas
			Assert.AreEqual(generatedCsv, expectedCsv);
		}

	}
}
