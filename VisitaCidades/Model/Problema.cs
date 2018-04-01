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
        public Problema(int tamanho)
        {
            Mapa = new Mapa(tamanho);
        }

        public Mapa Mapa { get; set; } = new Mapa();

        public List<Viajante> Viajantes { get; set; } = new List<Viajante>
        {
            new Viajante{ QuantidadeLocais = 10, Cor = Color.Red },
            new Viajante{ QuantidadeLocais = 10, Cor = Color.Blue },
            new Viajante{ QuantidadeLocais = 10, Cor = Color.Green },
        };

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

        public double Custo (IList<int> indexes)
        {
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
            }

            var repetidos = indexes.Count - indexes.Distinct().Count();
            if (repetidos > 0)
            {
                custo *= repetidos;
            }

            custo *= Vector2.Distance(Mapa.Locais[indexes.First()].Posicao, Mapa.Locais[indexes.Last()].Posicao);

            return custo;
        }

    }
}
