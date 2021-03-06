﻿using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitaCidades.Model;
using VisitaCidades.Model.Genetico;
using VisitaCidades.Model.HillClimbing;

namespace VisitaCidades
{
    public class App
    {
        public static void Run(string[] args)
        {
            if (args.Any(a => a.Contains("?") || a.ToLower().Contains("help")))
            {
                DisplayHelp();
                return;
            }

            try
            {
                using (var game = new Game1(Create(args)))
                    game.Run();
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static IAlgoritmo Create(string[] args)
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
            Mapa mapa;
            Problema problema;

            int tamanho;
            int[] rotas;
            double pesoProximidade;

            tamanho = dict.IntOrDefault("n", 30).Value;
            pesoProximidade = dict.DoubleOrDefault("pr", 1.0).Value;
            rotas = dict
                .ArrayOrDefault("r", new[] { (tamanho / 3).ToString(), (tamanho / 3).ToString(), (tamanho / 3 + tamanho % 3).ToString() })
                .Select(s => int.Parse(s))
                .ToArray();

            if (rotas.Sum() != tamanho)
            {
                throw new ArgumentException("A soma do tamanho das rotas deve ser igual à quantidade de locais no mapa.");
            }

            switch (dict.ValueOrDefault("f", "e"))
            {
                case "e":
                    mapa = Mapa.Elipse(tamanho);
                    break;
                case "a":
                    mapa = Mapa.Random(tamanho);
                    break;
                default:
                    throw new ArgumentException("Formato de mapa invalido.");
            }

            problema = new Problema(mapa, rotas, pesoProximidade);

            var a = dict.ValueOrDefault("a", "g");

            

            switch (a)
            {
                case "g":
                    return CriaAlgoritmoGenetico(dict, flat, problema);
                case "hc":
                    return CriaAlgoritmoHillClimbing(dict, flat, problema);
                default:
                    throw new ArgumentException("Algoritmo invalido.");
            }

        }

        private static IAlgoritmo CriaAlgoritmoHillClimbing(Dictionary<string, string[]> dict, List<string> flat, Problema problema)
        {
            var iteracoes = dict.IntOrDefault("i", 1000);
            if (iteracoes == null)
            {
                throw new ArgumentException("Especificador de iteracoes invalido!!!");
            }
            var stagnacao = dict.IntOrDefault("s", 100);
            if (stagnacao == null)
            {
                throw new ArgumentException("Especificador de stagnacao invalido!!!");
            }
            return new AlgoritmoHillClimbing(problema, iteracoes.Value, stagnacao.Value);
        }

        private static IAlgoritmo CriaAlgoritmoGenetico(Dictionary<string, string[]> dict, List<string> flat, Problema problema)
        {
            
            int populacaoMin, populacaoMax;
            IPopulation population;

            ISelection selection;
            ICrossover crossover;
            IMutation mutation;
            ITermination termination;
            IReinsertion reinsertion;
            float crossoverProbability, mutationProbability;

            

            

            var p = dict.ValueOrDefault("p", "50,100").Split(new[] { ',' });
            if (p.Length != 2 || !int.TryParse(p[0], out populacaoMin) || !int.TryParse(p[1], out populacaoMax))
            {
                throw new ArgumentException("Faixa de população inválida.");
            }

            population = new Population(populacaoMin, populacaoMax, new CromossomoViajante(problema.Mapa.Locais.Count));

            switch (dict.ValueOrDefault("s", "t"))
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

            switch (dict.ValueOrDefault("e", "e"))
            {
                case "e":
                    reinsertion = new ElitistReinsertion();
                    break;
                case "p":
                    reinsertion = new PureReinsertion();
                    break;
                case "u":
                    reinsertion = new UniformReinsertion();
                    break;
                default:
                    throw new ArgumentException("Reinserção inválida.");
            }

            if (!float.TryParse(dict.ValueOrDefault("cp", "0,75"), out crossoverProbability))
            {
                throw new ArgumentException("Probabilidade de crossover inválida.");
            }

            if (!float.TryParse(dict.ValueOrDefault("mp", "0,25"), out mutationProbability))
            {
                throw new ArgumentException("Probabilidade de mutação inválida.");
            }


            return new AlgoritmoGenetico(problema, population, selection, crossover, crossoverProbability, mutation, mutationProbability, termination, reinsertion);
        }

        private static void DisplayHelp()
        {
            var text = @"
VisitaCidades
Uso:    visita.exe [-a algoritmo] [-n numero-locais=30] [-r rotas] [-f formato-mapa] [-pr peso-proximidade=1,0] [opcoes do algoritmo]
Onde:
    algoritmo:
        g* (Genetico), hc (Hill Climbing), sa (Simulated Arealing)
    numero-locais: 
        Quantidade de pontos no mapa (padrão: 30)
    rotas:
        Quantidade de locais por rotas. (padrão: 3 rotas com distribuição proporcional)
        Exemplo: -r 8 12 10 cria 3 rotas, sendo uma de 8, uma de 12 e outra de 10 caminhos.
    formato-mapa:
        e* (Elipsoide), a (Aleatorio)
    peso-proximidade: 
        A prioridade que a proximidade entre sub-rotas tomará no algoritmo.

Opcoes por algoritmo:
----------------------------------------------------------------------------------------
    g:
        -p [min=50],[max=100]: Tamanho minimo e/ou maximo da populacao
        -s [selection]: Selecao
            t*: Tournament Selection
            e: Truncation Selection
            r: Roulette Wheel Selection
            s: Stochastic Universal Sampling Selection
        -c [crossover]: Crossover.
            o*: Ordered Crossover
            ob: Order Based Crossover
            u: Uniform Crossover
            Mais opcoes no codigo.
        -m [mutacao]: Mutacao.
            r*: Reverse Sequence Mutation
            s: Partial Shuffle Mutation
            d: Displacement Mutation
            Mais opcoes no codigo.
        -e [reinsercao]: Reinsercao.
            e*: Elitist Reinsertion
            u: Uniform Reinsertion
            p: Pure Reinsertion
            Mais opcoes no codigo.
        -t [termination]: Condicao de parada
            s*: Fitness Stagnation Termination
            t: Fitness Threshold Termination
            g: Generation Number Termination
        -cp [crossover-probability=0,75]: Probabilidade de crossover.
        -cp [mutation-probability=0,25]: Probabilidade de crossover.
----------------------------------------------------------------------------------------
    hc:
        -i [iteracoes=1000]: Quantidade de iteracoes.
        -s [iteracoes=100]: Contador de estagnacao.
----------------------------------------------------------------------------------------
    sa:
        NÃO IMPLEMENTADO!!!
----------------------------------------------------------------------------------------
";
            Console.WriteLine(text);
        }


    }
}
