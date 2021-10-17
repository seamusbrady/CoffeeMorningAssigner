using System;
using System.Collections.Generic;
using CoffeeMorningAssigner.Models;
using GAF;
using GAF.Extensions;
using GAF.Operators;
using Math = System.Math;

namespace CoffeeMorningAssigner
{
    public static class GeneticAlgorithmRunner
    {
        private static ScoreCalculator _calculator;
        private static AlgorithmParameters _parameters;

        public static Chromosome FindFittestChromosome(List<User> includedUsers, ScoreCalculator scoreCalculator,
            AlgorithmParameters algorithmParameters)
        {
            _calculator = scoreCalculator;
            _parameters = algorithmParameters;

            var population = new Population();

            // Create the chromosomes and add to the population
            for (var p = 0; p < 100; p++)
            {
                var chromosome = new Chromosome();
                foreach (var user in includedUsers)
                {
                    chromosome.Genes.Add(new Gene(user));
                }

                chromosome.Genes.ShuffleFast();
                population.Solutions.Add(chromosome);
            }

            // Keep 1/3 of best population
            var elite = new Elite(includedUsers.Count / 3);

            // Create a DoublePointOrdered crossover operator
            //  which keeps unique users/genes in child chromosomes
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };

            // Create the mutation operator with a high mutation
            //  to have more users jump groups and not just reorder within a group
            var mutate = new SwapMutate(0.5);

            var ga = new GeneticAlgorithm(population, CalculateFitness);

            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;

            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            ga.Run(Terminate);


            var fittest = ga.Population.GetTop(1)[0];
            return fittest;
        }

        private static double CalculateFitness(Chromosome chromosome)
        {
            var score = (double)CalculateChromosomeScore(chromosome);

            var fitness = 1 - score / _parameters.FitnessDivisor;

            return fitness;
        }


        private static void ga_OnRunComplete(object sender, GaEventArgs e)
        {
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];

            Console.Write($"\rGeneration: {e.Generation}, " +
                          $"Fitness: {fittest.Fitness:P}" +
                          $", Score: {CalculateChromosomeScore(fittest):D4} (lower is better)");
        }

        private static int CalculateChromosomeScore(Chromosome chromosome)
        {
            var weekAssignment = new WeekAssignment(chromosome, "ignore");
            return _calculator.CalculateScore(weekAssignment);
        }

        private static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            // Stop if perfect solution found
            if (Math.Abs(population.GetTop(1)[0].Fitness - 1) < 0.00000001)
                return true;

            // Or we run through enough generations
            return currentGeneration >= AlgorithmParameters.NumberOfGenerations;
        }

    }
}