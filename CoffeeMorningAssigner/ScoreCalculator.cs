using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CoffeeMorningAssigner.Models;

namespace CoffeeMorningAssigner
{
    public class ScoreCalculator
    {
        private readonly SparseMatrix<int> _historicalScore;

        public ScoreCalculator(AssignmentHistory history, AlgorithmParameters parameters, int totalUserCount)
        {
            _historicalScore = CreateScoreMatrix(history, parameters, totalUserCount);
        }

        private SparseMatrix<int> CreateScoreMatrix(AssignmentHistory history, AlgorithmParameters parameters,
            int totalUserCount)
        {
            int numWeeks = parameters.NumWeeksLookBack;

            Console.WriteLine($"Using {numWeeks} weeks of history for calculations out of a total of {history.Weeks.Count} weeks history");

            var score = new SparseMatrix<int>(totalUserCount, totalUserCount);

            int penalty = parameters.MaxPenalty;

            foreach (var week in history.Weeks.OrderByDescending(w => w.WeekId).Take(numWeeks))
            {
                foreach (var users in week.Groups.Select(group => group.Members))
                {
                    for (int i = 0; i < users.Count; i++)
                    {
                        for (int j = i + 1; j < users.Count; j++)
                        {
                            var x = users[i].Id;
                            var y = users[j].Id;

                            score[x, y] += penalty;
                            score[y, x] += penalty;
                        }
                    }
                }

                penalty -= parameters.WeeklyPenalty;
                Debug.Assert(penalty > 0);
            }
            return score;
        }


        public int CalculateScore(WeekAssignment assignment)
        {
            int runningScore = 0;
            foreach (var group in assignment.Groups)
            {
                var users = group.Members;
                for (int i = 0; i < users.Count; i++)
                {
                    for (int j = i + 1; j < users.Count; j++)
                    {
                        var x = users[i].Id;
                        var y = users[j].Id;

                        runningScore += _historicalScore[x, y];
                    }
                }
            }

            return runningScore;

        }


        public string ReportPreviousMeetings(WeekAssignment current, AssignmentHistory history, AlgorithmParameters parameters)
        {
            var line = "".PadRight(80, '-');
            var report = new List<string> { "",line, "Previous Meetings Report", line};

            foreach (var recent in history.Weeks.OrderByDescending(w=>w.WeekId).Take(parameters.NumWeeksLookBack))
            {
                report.AddRange(ReportPreviousMeetings(current, recent));
            }

            return string.Join(Environment.NewLine, report);
        }

        private List<string> ReportPreviousMeetings(WeekAssignment current, WeekAssignment recent)
        {
            if (!recent.Groups.Any()) return new List<string>();

            var report = new List<string> { $"Week: {recent.Name}:" };

            foreach (var group in current.Groups)
            {
                var users = group.Members;
                for (int i = 0; i < users.Count; i++)
                {
                    for (int j = i + 1; j < users.Count; j++)
                    {
                        if (ArePairedUpAlready(users[i], users[j], recent))
                        {
                            var score = _historicalScore[users[i].Id, users[j].Id];
                            report.Add($"\tGroup {group.Id}:  {users[i]} and {users[j]}, score: {score}");
                        }
                    }
                }
            }

            return report;
        }

        private bool ArePairedUpAlready(User a, User b, WeekAssignment assignment)
        {
            return assignment.Groups.Any(group => group.Members.Contains(a) && group.Members.Contains(b));
        }
    }
}