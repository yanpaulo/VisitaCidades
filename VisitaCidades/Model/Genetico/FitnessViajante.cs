using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitaCidades.Model.Genetico
{
    public class FitnessViajante : IFitness
    {
        public Problema Problema { get; private set; }

        public FitnessViajante(Problema problema)
        {
            Problema = problema;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var ordem = chromosome.GetGenes().Select(g => (int)g.Value).ToList();

            return 1 / Problema.Custo(ordem);
        }
    }
}
