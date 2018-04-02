using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using GeneticSharp.Infrastructure.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitaCidades.Model.Genetico
{
    public class AlgoritmoGenetico : AlgoritmoBase
    {
        private GeneticAlgorithm ga;

        public AlgoritmoGenetico(Problema problema, IPopulation population, ISelection selection, 
            ICrossover crossover, float crossoverProbability, IMutation mutation, float mutationProbability, ITermination termination) : base(problema)
        {
            ga = new GeneticAlgorithm(population, new FitnessViajante(problema), selection, crossover, mutation)
            {
                CrossoverProbability = crossoverProbability,
                MutationProbability = mutationProbability,
                Termination = termination,
                TaskExecutor = new SmartThreadPoolTaskExecutor(),
            };

            ga.GenerationRan += Ga_GenerationRan;
        }

        private void Ga_GenerationRan(object sender, EventArgs e)
        {
            var list = ga.BestChromosome.GetGenes().Select(g => (int)g.Value).ToList();
            Solucao = Problema.Solucao(list);
        }

        protected override void Roda()
        {
            ga.Start();
        }

        public override string ToString() =>
            $"Populacao: {ga.Population.MinSize}-{ga.Population.MaxSize}\n" +
            $"Selecao: {ga.Selection.GetType().GetDisplayName()}\n" +
            $"Crossover: {ga.Crossover.GetType().GetDisplayName()}\n" +
            $"Mutation: {ga.Mutation.GetType().GetDisplayName()}\n" +
            $"Termination: {ga.Termination.GetType().GetDisplayName()}\n";
    }
}
