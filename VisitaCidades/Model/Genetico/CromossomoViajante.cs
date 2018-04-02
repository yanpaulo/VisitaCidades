using GeneticSharp.Domain.Chromosomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static VisitaCidades.Utils;

namespace VisitaCidades.Model.Genetico
{
    public class CromossomoViajante : ChromosomeBase
    {
        public CromossomoViajante(int length) : base(length)
        {
            var lista = new List<int>();

            for (int i = 0; i < length; i++)
            {
                int n;
                do
                {
                    n = Rand.Next(length);
                } while (lista.Contains(n));
                lista.Add(n);
                ReplaceGene(i, new Gene(n));
            }
        }

        public override IChromosome CreateNew()
        {
            return new CromossomoViajante(Length);
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(Rand.Next(Length));
        }
    }
}
