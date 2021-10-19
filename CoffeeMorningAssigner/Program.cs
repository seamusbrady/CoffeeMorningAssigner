using System;
using System.Collections.Generic;
using System.IO;
using CoffeeMorningAssigner.DAL;
using CoffeeMorningAssigner.Models;

namespace CoffeeMorningAssigner
{
    class Program
    {
        static void Main(string[] args)
        {

            // First time running the application - copy the userhistory18.csv to your MyDocuments folder
            CsvRepository<User>.CopyFileToWorkingFolder("Data\\userHistory18.csv");

            bool validFileNumber;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Enter userHistory file number or any other character to quit.");
                var input = Console.ReadLine();

                validFileNumber = int.TryParse(input, out var fileId);
                if (validFileNumber)
                {
                    RunOnUserHistory(fileId);
                }

            } while (validFileNumber);
        }
   
        private static void RunOnUserHistory(int fileNumber)
        {
            // Load our historical scores
            var userHistoryRepository = new UserHistoryRepository();
            var history = userHistoryRepository.Read($"userHistory{fileNumber}.csv");

            var userRepository = new UserRepository($"userHistory{fileNumber}.csv");
            var includedUsers = userRepository.GetIncludedUsers();
            var totalUserCount = userRepository.GetAllUsers().Count;

            var weekAssignment = RunAlgorithm(history, includedUsers, totalUserCount);

            history.Add(weekAssignment);


            var allUsers = userRepository.GetAllUsers();
            userHistoryRepository.Write($"userHistory{fileNumber + 1}.csv", allUsers, history);
        }

        private static WeekAssignment RunAlgorithm(AssignmentHistory history, List<User> includedUsers, int totalUserCount)
        {
            // store parameters for calculations
            var parameters = AlgorithmParametersFactory.Create(includedUsers.Count);

            var calculator = new ScoreCalculator(history, parameters, totalUserCount);

            var fittest = GeneticAlgorithmRunner.FindFittestChromosome(includedUsers, calculator, parameters);

            var weekName = "Week " + (history.Weeks.Count + 1);
            var weekAssignment = new WeekAssignment(fittest, weekName);

            LogOutput(calculator, weekAssignment, history);

            return weekAssignment;
        }

        private static void LogOutput(ScoreCalculator calculator, WeekAssignment weekAssignment, AssignmentHistory history)
        {
            var previousMeetingsReport = calculator.ReportPreviousMeetings(weekAssignment, history);
            Console.WriteLine(previousMeetingsReport);
            File.AppendAllText("PreviousMeetings.log", previousMeetingsReport);

            var assignmentSummary = weekAssignment.ToString();
            Console.WriteLine(assignmentSummary);
            File.AppendAllText("AssignmentSummary.log", assignmentSummary);
        }

    }
}
