using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static VisitaCidades.Utils;

namespace VisitaCidades.Model
{
    public class Problema
    {
        private double pesoProximidade;

        public Problema(Mapa mapa, int[] tamanhoRotas, double pesoProximidade)
        {
            var nomes = new[] { "Maria", "Sebastiao", "Brito", "Raquel", "Priscila", "Naruto", "Alucard", "Vegeta", "Goku", "Solid Snake", "John Connor" };
            var sobreNomes = new[] { "Joao", "Silva", "Freire", "Uzumaki", "Son", "Uchiha", "Nanomachines", "Pereira" };

            var cores = new Queue<Color>(new[] { Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Purple, Color.Brown, Color.Orange, Color.Gray });

            Mapa = mapa;

            Viajantes = tamanhoRotas.Select(t => new Viajante
            {
                QuantidadeLocais = t,
                Cor = cores.Dequeue(),
                Nome = $"{nomes.Random()} {sobreNomes.Random()}"
            })
            .ToArray();

            this.pesoProximidade = pesoProximidade;
        }


        public Mapa Mapa { get; set; }

        public IEnumerable<Viajante> Viajantes { get; set; }

        public Solucao SolucaoAleatoria()
        {
            var rotas = new List<Rota>();
            foreach (var viajante in Viajantes)
            {
                var rota = new Rota { Viajante = viajante };
                rotas.Add(rota);

                while (rota.Locais.Count < viajante.QuantidadeLocais)
                {
                    Local local;
                    do
                    {
                        local = Mapa.LocailAleatorio();
                    } while (rotas.Any(r => r.Locais.Contains(local)));

                    rota.Locais.Add(local);
                }
            }
            return new Solucao { Rotas = rotas };
        }

        public Solucao Solucao(IList<int> indexes)
        {
            var rotas = new List<Rota>();
            var locaisCount = 0;
            float custo = 0;

            foreach (var viajante in Viajantes)
            {
                var rota = new Rota
                {
                    Viajante = viajante,
                    Locais = indexes.Skip(locaisCount).Take(viajante.QuantidadeLocais).Select(index => Mapa.Locais[index]).ToList()
                };

                if (rotas.Any())
                {
                    rota.Locais.Insert(0, rotas.Last().Locais.Last());
                } 

                for (int i = 0; i < rota.Locais.Count - 1; i++)
                {
                    var atual = rota.Locais[i];
                    var proximo = rota.Locais[i + 1];

                    custo += Vector2.Distance(atual.Posicao, proximo.Posicao);
                }

                locaisCount += viajante.QuantidadeLocais;
                rotas.Add(rota);
            }

            return new Solucao
            {
                Custo = custo,
                Rotas = rotas
            };

        }

        public double Custo(IList<int> indexes)
        {
            double distanciaEntreRotas = 0;
            double custo = 0;
            int count = 0;
            foreach (var viajante in Viajantes)
            {
                var items = indexes.Skip(count).Take(viajante.QuantidadeLocais).ToList();

                for (int i = 0; i < items.Count - 1; i++)
                {
                    var atual = Mapa.Locais[items[i]];
                    var proximo = Mapa.Locais[items[i + 1]];

                    custo += Vector2.Distance(atual.Posicao, proximo.Posicao);
                }
                count += items.Count;

                var fimAtual = items.Last();
                var inicioProximo = indexes[count % indexes.Count];

                distanciaEntreRotas += Vector2.Distance(Mapa.Locais[fimAtual].Posicao, Mapa.Locais[inicioProximo].Posicao);


            }
            custo += pesoProximidade * distanciaEntreRotas;

            var repetidos = indexes.Count - indexes.Distinct().Count();
            if (repetidos > 0)
            {
                custo *= Math.Pow(2, repetidos);
            }


            return custo;
        }

    }
}
