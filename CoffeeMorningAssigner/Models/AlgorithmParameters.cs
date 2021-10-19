namespace CoffeeMorningAssigner.Models
{
    public class AlgorithmParameters
    {
        public int NumUsers { get; set; }
        public int NumWeeksLookBack { get; set; }
        public int MaxPenalty { get; set; }
        public int WeeklyPenalty { get; set; }
        public double FitnessDivisor { get; set; }

        public static int NumberOfGenerations => 10000;
        public static int MaxUsersPerGroup => 4;
    }

    public static class AlgorithmParametersFactory
    {
        public static AlgorithmParameters Create(int numUsers)
        {

            var p = new AlgorithmParameters
            {
                NumUsers = numUsers,
                NumWeeksLookBack = numUsers / AlgorithmParameters.MaxUsersPerGroup + 2,
            };

            p.MaxPenalty = p.NumWeeksLookBack * 10 + 10;
            p.WeeklyPenalty = 10;
            p.FitnessDivisor = numUsers * p.MaxPenalty * 2;
            return p;
        }
    }
}