using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VisitaCidades.Utils;
namespace VisitaCidades.Model.HillClimbing
{
    public class AlgoritmoHillClimbing : AlgoritmoBase
    {
        public int Iteracoes { get; private set; }
        public int Stagnacao { get; private set; }

        public AlgoritmoHillClimbing(Problema problema, int iteracoes, int stagnacao) : base(problema)
        {
            Iteracoes = iteracoes;
            Stagnacao = stagnacao;
        }

        protected override void Roda()
        {
            int stagCount = 0;

            for (int i = 0; i < Iteracoes; i++)
            {
                List<int> indexes = SolucaoAleatoria();

                var menor = indexes;
                var custoOriginal = Problema.Custo(indexes);
                var custo = custoOriginal;
                double? novoCusto = null;

                while (!(novoCusto < custo))
                {
                    for (int p = 0; p < indexes.Count; p++)
                    {
                        for (int q = 0; q < indexes.Count; q++)
                        {
                            var copia = indexes.ToList();
                            var n = copia[q];
                            copia.RemoveAt(q);
                            copia.Insert(p, n);

                            novoCusto = Problema.Custo(copia);
                            if (novoCusto < custo)
                            {
                                indexes = copia;
                                custo = novoCusto.Value;
                                break;
                            }
                        }
                        if (novoCusto < custo)
                        {
                            break;
                        }
                    }

                    Solucao = Problema.Solucao(indexes);
                    if (novoCusto >= custo)
                    {
                        break;
                    }
                }
                if (custo < custoOriginal)
                {
                    menor = indexes;
                    stagCount = 0;
                }
                else
                {
                    if (++stagCount  > Stagnacao)
                    {
                        break;
                    }
                }
            }
        }

        private List<int> SolucaoAleatoria()
        {
            var indexes = new List<int>();
            for (int n = 0; n < Problema.Mapa.Locais.Count; n++)
            {
                int index;
                do
                {
                    index = Rand.Next(Problema.Mapa.Locais.Count);
                } while (indexes.Contains(index));
                indexes.Add(index);
            }

            return indexes;
        }
    }
}
