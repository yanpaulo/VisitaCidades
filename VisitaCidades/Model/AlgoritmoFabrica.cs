﻿using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitaCidades.Model
{
    public class AlgoritmoFabrica
    {
        public static IAlgoritmo CreateAlgoritmo(string[] args)
        {
            return Run(args);
        }

        private static IAlgoritmo Run(string[] args)
        {
            var dict = new Dictionary<string, string[]>();
            var flat = new List<string>();

            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.StartsWith("-"))
                {
                    arg = arg.Remove(0, 1);
                    var list = new List<string>();

                    for (int n = i + 1; n < args.Length && !args[n].StartsWith("-"); n++, i++)
                    {
                        list.Add(args[n]);
                    }

                    dict[arg] = list.ToArray();
                }
                else
                {
                    flat.Add(arg);
                }
            }

            var a = dict.ValueOrDefault("a", "g");
            switch (a)
            {
                case "g":
                    return CriaAlgoritmoGenetico(dict, flat);
                default:
                    break;
            }

            throw new NotImplementedException();
        }

        private static IAlgoritmo CriaAlgoritmoGenetico(Dictionary<string, string[]> dict, List<string> flat)
        {
            int tamanho;
            Problema problema;

            int populacaoMin, populacaoMax;
            IPopulation population;

            ISelection selection;
            ICrossover crossover;
            IMutation mutation;
            ITermination termination;
            float crossoverProbability, mutationProbability;

            tamanho = dict.IntOrDefault("n", 30).Value;
            problema = new Problema(tamanho);

            var p = dict.ValueOrDefault("p", "5,50").Split(new[] { ',' });
            if (p.Length != 2 || !int.TryParse(p[0], out populacaoMin) || !int.TryParse(p[1], out populacaoMax))
            {
                throw new ArgumentException("Faixa de população inválida.");
            }

            population = new Population(populacaoMin, populacaoMax, new CromossomoViajante(tamanho));
            
            switch (dict.ValueOrDefault("s", "e"))
            {
                case "e":
                    selection = new EliteSelection();
                    break;
                case "r":
                    selection = new RouletteWheelSelection();
                    break;
                case "s":
                    selection = new StochasticUniversalSamplingSelection();
                    break;
                case "t":
                    selection = new TournamentSelection();
                    break;
                default:
                    throw new ArgumentException("Seleção inválida.");
            }

            switch (dict.ValueOrDefault("c", "o"))
            {
                case "s":
                    crossover = new CutAndSpliceCrossover();
                    break;
                case "c":
                    crossover = new CycleCrossover();
                    break;
                case "o":
                    crossover = new OrderedCrossover();
                    break;
                case "ob":
                    crossover = new OrderBasedCrossover();
                    break;
                case "op":
                    crossover = new OnePointCrossover();
                    break;
                case "pm":
                    crossover = new PartiallyMappedCrossover();
                    break;
                case "p":
                    crossover = new PositionBasedCrossover();
                    break;
                case "tpa":
                    crossover = new ThreeParentCrossover();
                    break;
                case "tp":
                    crossover = new TwoPointCrossover();
                    break;
                case "u":
                    crossover = new UniformCrossover();
                    break;
                default:
                    throw new ArgumentException("Crossover inválido.");
            }

            switch (dict.ValueOrDefault("m", "r"))
            {
                case "d":
                    mutation = new DisplacementMutation();
                    break;
                case "f":
                    mutation = new FlipBitMutation();
                    break;
                case "i":
                    mutation = new InsertionMutation();
                    break;
                case "s":
                    mutation = new PartialShuffleMutation();
                    break;
                case "r":
                    mutation = new ReverseSequenceMutation();
                    break;
                case "t":
                    mutation = new TworsMutation();
                    break;
                case "u":
                    mutation = new UniformMutation();
                    break;
                default:
                    throw new ArgumentException("Mutação inválida.");
            }

            switch (dict.ValueOrDefault("t", "s"))
            {
                case "s":
                    termination = new FitnessStagnationTermination();
                    break;
                case "t":
                    termination = new FitnessThresholdTermination();
                    break;
                case "g":
                    termination = new GenerationNumberTermination();
                    break;
                default:
                    throw new ArgumentException("Terminação inválida.");
            }

            if(!float.TryParse(dict.ValueOrDefault("cp", "0,75"), out crossoverProbability))
            {
                throw new ArgumentException("Probabilidade de crossover inválida.");
            }

            if (!float.TryParse(dict.ValueOrDefault("mp", "0,25"), out mutationProbability))
            {
                throw new ArgumentException("Probabilidade de mutação inválida.");
            }


            return new AlgoritmoGenetico(problema, population, selection, crossover, crossoverProbability, mutation, mutationProbability, termination);
        }
    }

    public static class LocalExtensions
    {
        public static string ValueOrDefault(this Dictionary<string, string[]> dictionary, string key, string defaultValue = null)
        {
            if (dictionary.TryGetValue(key, out string[] value))
            {
                return value.SingleOrDefault() ?? defaultValue;
            }
            return defaultValue;
        }

        public static int? IntOrDefault(this Dictionary<string, string[]> dictionary, string key, int? defaultValue = null)
        {
            if(int.TryParse(dictionary.ValueOrDefault(key, null), out int n))
            {
                return n;
            }
            return defaultValue;
        }
    }
}