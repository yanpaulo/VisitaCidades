using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitaCidades.Model
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
            var genes = chromosome.GetGenes();
            var ordem = genes.Select(g => (int)g.Value).ToList();
            double custo = 0;
            int count = 0;
            foreach (var viajante in Problema.Viajantes)
            {
                var items = ordem.Skip(count).Take(viajante.QuantidadeLocais).ToList();

                for (int i = 0; i < items.Count - 1; i++)
                {
                    var atual = Problema.Mapa.Locais[items[i]];
                    var proximo = Problema.Mapa.Locais[items[i + 1]];

                    custo += Vector2.Distance(atual.Posicao, proximo.Posicao);
                }

                count += items.Count;
            }

            var repetidos = ordem.Count - ordem.Distinct().Count();
            if (repetidos > 0)
            {
                custo *= repetidos;
            }

            custo *= Vector2.Distance(Problema.Mapa.Locais[ordem.First()].Posicao, Problema.Mapa.Locais[ordem.Last()].Posicao);


            return 1 / custo;
        }
    }
}
