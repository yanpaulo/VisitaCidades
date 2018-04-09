using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using static VisitaCidades.Utils;
using System;

namespace VisitaCidades.Model
{
    public class Mapa
    {
        private Mapa() { }

        public static Mapa Random(int quantidadeLocais = 30)
        {
            var mapa = new Mapa();
            int
                xMin = (int)(mapa.Tamanho.Width * 0.05f),
                xMax = (int)(mapa.Tamanho.Width * 0.85f),
                yMin = (int)(mapa.Tamanho.Height * 0.05f),
                yMax = (int)(mapa.Tamanho.Height * 0.95f);

            var locais = Enumerable
                .Range(0, quantidadeLocais)
                .Select(n => new Local('A' + n <= 'Z' ? ((char)('A' + n)).ToString() : n.ToString(), new Vector2(Rand.Next(xMin, xMax), Rand.Next(yMin, yMax))))
                .ToList();

            mapa.Locais = locais;

            return mapa;
        }

        public static Mapa Elipse(int quantidadeLocais = 30)
        {
            var mapa = new Mapa();
            var tamanho = mapa.Tamanho;

            float raio = Math.Min(tamanho.Width, tamanho.Height) / 2;
            var faixa = (int)(raio * 0.15);
            raio *= 0.8f;

            var delta = 2 * Math.PI / quantidadeLocais;
            var centro = tamanho.Center.ToVector2();
            var locais = Enumerable.Range(0, quantidadeLocais)
                .Select(n => Rand.Next(faixa) + raio)
                .Select((r, i) => new Local(
                    'A' + i <= 'Z' ? ((char)('A' + i)).ToString() : i.ToString(),
                    centro + new Vector2((float)(Math.Cos(delta * i) * r + (r * Math.Cos(delta * i))), (float)Math.Sin(delta * i) * r)))
                .ToList();

            mapa.Locais = locais;

            return mapa;
        }

        public Rectangle Tamanho { get; private set; } =
            new Rectangle(0, 0, 1280, 600);

        public List<Local> Locais { get; private set; }


        public Local LocailAleatorio() => Locais[Rand.Next(Locais.Count)];
    }
}