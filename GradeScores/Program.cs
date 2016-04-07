using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GradeScores
{
	public class Program
	{
		const string OutputFileSuffix = "-graded.txt";

		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine(string.Format("Usage: {0} [filename]", Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0])));
				return;
			}

			// Spec: Takes as a parameter a string that represents a text file containing a list of names, and their scores.
			string inputFilename = args[0];
			string inputData;
			try
			{
				inputData = ReadInputFile(inputFilename);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine(string.Format("File not found: {0}", inputFilename));
				return;
			}
			catch (IOException)
			{
				Console.WriteLine(string.Format("Could not read: {0}", inputFilename));
				return;
			}

			StudentCohort cohort;
			try
			{
				cohort = new StudentCohort(inputData);
			}
			catch (FormatException ex)
			{
				Console.WriteLine(string.Format("Error parsing data: {0}", ex.Message));
				return;
			}

			// Spec: Orders the names by their score. If scores are the same, order by their last name followed by first name.
			cohort.SortByGradeDesc();

			// Spec: Example shows data being printed to console as well as written to file
			string outputData = cohort.GenerateCsv();
			Console.WriteLine(outputData);

			// Spec: Creates a new text file called <input-file-name>-graded.txt with the list of sorted score and names.
			string outputFilename = Path.GetFileNameWithoutExtension(inputFilename) + OutputFileSuffix;
			try
			{
				WriteOutputFile(outputFilename, outputData);
			}
			catch (IOException)
			{
				Console.WriteLine(string.Format("Could not write output file: ", outputFilename));
				return;
			}

			Console.WriteLine(string.Format("Finished: {0}", outputFilename));
		}

		///////////////////////////////////////////////////////////////////////

		static string ReadInputFile(string filename)
		{
			return File.ReadAllText(filename, Encoding.UTF8);
		}

		///////////////////////////////////////////////////////////////////////

		static void WriteOutputFile(string filename, string contents)
		{
			File.WriteAllText(filename, contents, Encoding.UTF8);
		}

	}
}
