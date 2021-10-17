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

            // UserHistoryImporter.Run();
            // HistoryReadWriteTester.Run();
            

            bool isOk;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Enter userHistory file number:");
                var input = Console.ReadLine();

                isOk = int.TryParse(input, out var fileId);
                if (isOk)
                {
                    RunOnUserHistory(fileId);
                }

            } while (isOk);
        }

        private static void RunOnUserHistory(int fileNumber)
        {
            // Load our historical scores
            var userHistoryRepository = new UserHistoryRepository();
            var history = userHistoryRepository.Read($"userHistory{fileNumber}.csv");

            var userManager = new UserManager($"userHistory{fileNumber}.csv");
            var includedUsers = userManager.GetIncludedUsers();

            history = Run(history, includedUsers);

            var allUsers = userManager.GetAllUsers();
            userHistoryRepository.Write($"userHistory{fileNumber + 1}.csv", allUsers, history);
        }

        private static AssignmentHistory Run(AssignmentHistory history, List<User> includedUsers)
        {

            // store parameters for calculations
            var parameters = AlgorithmParametersFactory.Create(includedUsers.Count);


            var calculator = new ScoreCalculator(history, parameters);

            var fittest = GeneticAlgorithmRunner.FindFittestChromosome(includedUsers, calculator, parameters);

            var weekName = "Week " + (history.Weeks.Count + 1);
            var weekAssignment = Converter.ConvertToWeekAssignment(fittest, weekName);

            LogOutput(calculator,weekAssignment,history);

            history.Add(weekAssignment);

            return history;

        }

        private static void LogOutput(ScoreCalculator calculator, WeekAssignment weekAssignment, AssignmentHistory history)
        {
            var previousMeetingsReport = calculator.ReportPreviousMeetings(weekAssignment, history);
            Console.WriteLine(previousMeetingsReport);
            File.AppendAllText("PreviousMeetings.log", previousMeetingsReport);
            //Console.WriteLine();

            var assignmentSummary = weekAssignment.ToString();
            Console.WriteLine(assignmentSummary);
            File.AppendAllText("AssignmentSummary.log", assignmentSummary);
            //Console.WriteLine();
        }

        //private static void Run(int fileNumber)
        //{
        //    //get our users
        //    var userManager = new UserManager();
        //    var includedUsers = userManager.GetIncludedUsers();
        //    var excludedUsers = userManager.GetExcludedUsers();


        //    // store parameters for calculations
        //    var parameters = AlgorithmParametersFactory.Create(includedUsers.Count);


        //    // Load our historical scores
        //    var history = new JsonRepository().Load<AssignmentHistory>($"history{fileNumber}.json");

        //    var calculator = new ScoreCalculator(history, parameters);

        //    var fittest = GeneticAlgorithmRunner.FindFittestChromosome(includedUsers, calculator, parameters);

        //    var weekName = "Week " + (history.Weeks.Count + 1);
        //    var weekAssignment = Converter.ConvertToWeekAssignment(fittest, weekName);
        //    var userAssignments = Converter.ConvertToUserAssignments(weekAssignment, excludedUsers);

        //    Console.WriteLine(calculator.ReportPreviousMeetings(weekAssignment, history));

        //    history.Add(weekAssignment);


        //    // Save 
        //    new JsonRepository().Save($"history{fileNumber + 1}.json", history);
        //    new CsvRepository<UserAssignment>().Save($"assignments{fileNumber + 1}.csv", userAssignments);

        //}
    }

    
}
