using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GradeScores;

namespace GradeScoresTests
{
	[TestClass]
	public class ProgramTests
	{
		///////////////////////////////////////////////////////////////////////
		//
		// Data-driven tests for the whole program
		//

		// Helper function to validate expected output (when no exceptions occur)
		private void ValidateProgramOutput(string inputFilename, string expectedOutputFilename)
		{
			if (!File.Exists(inputFilename))
			{
				throw new FileNotFoundException("Missing input file: " + inputFilename);
			}
			if (!File.Exists(inputFilename))
			{
				throw new FileNotFoundException("Missing expected output file: " + expectedOutputFilename);
			}

			string[] args = { inputFilename };
			Program.Main(args);

			string outputFilename = Path.GetFileNameWithoutExtension(inputFilename) + "-graded.txt";  // From spec
			string outputData = File.ReadAllText(outputFilename, Encoding.UTF8);
			string expectedOutputData = File.ReadAllText(expectedOutputFilename, Encoding.UTF8);

			// Force newlines to be \n so different platforms don't fail this test due to their newlines
			outputData = outputData.Replace("\r\n", "\n").Replace("\r", "\n");
			expectedOutputData = expectedOutputData.Replace("\r\n", "\n").Replace("\r", "\n");

			Assert.AreEqual(outputData, expectedOutputData, "Program output did not match expected output; see " + outputFilename);
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		[DeploymentItem(@"TestData\SortEqualGrades-input.txt")]
		[DeploymentItem(@"TestData\SortEqualGrades-expected.txt")]
		public void Program_SortEqualGrades()
		{
			// Test sort order when grades are equal
			ValidateProgramOutput("SortEqualGrades-input.txt", "SortEqualGrades-expected.txt");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		[DeploymentItem(@"TestData\GradesOutsideNormalRange-input.txt")]
		[DeploymentItem(@"TestData\GradesOutsideNormalRange-expected.txt")]
		public void Program_GradesOutsideNormalRange()
		{
			// Test grades: negative values / large values; the spec doesn't define a grade range
			ValidateProgramOutput("GradesOutsideNormalRange-input.txt", "GradesOutsideNormalRange-expected.txt");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		[DeploymentItem(@"TestData\NamesContainSpecialChars-input.txt")]
		[DeploymentItem(@"TestData\NamesContainSpecialChars-expected.txt")]
		public void Program_NamesContainSpecialChars()
		{
			// Test names containing some special characters
			// Ideally we would whitelist characters allowed in names; but the spec doesn't define this
			ValidateProgramOutput("NamesContainSpecialChars-input.txt", "NamesContainSpecialChars-expected.txt");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		[DeploymentItem(@"TestData\NamesContainNonAscii-input.txt")]
		[DeploymentItem(@"TestData\NamesContainNonAscii-expected.txt")]
		public void Program_NamesContainNonAscii()
		{
			// Test names containing some non-ASCII characters
			ValidateProgramOutput("NamesContainNonAscii-input.txt", "NamesContainNonAscii-expected.txt");
		}

		///////////////////////////////////////////////////////////////////////

		[TestMethod]
		[DeploymentItem(@"TestData\LargeDataSet-input.txt")]
		[DeploymentItem(@"TestData\LargeDataSet-expected.txt")]
		public void Program_LargeDataSet()
		{
			// Test program function with a large data set
			// Ideally this data file would be much larger than it is for this programming test...
			ValidateProgramOutput("LargeDataSet-input.txt", "LargeDataSet-expected.txt");
		}

	}
}
