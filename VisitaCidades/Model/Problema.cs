using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static VisitaCidades.Utils;

namespace VisitaCidades.Model
{
    class Problema
    {
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

    }
}
