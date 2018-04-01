using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using static VisitaCidades.Utils;

namespace VisitaCidades.Model
{
    public class Mapa
    {
        public Mapa(int quantidadeLocais = 30)
        {
            Locais = Enumerable
                .Range(0, quantidadeLocais)
                .Select(n => new Local('A' + n <= 'Z' ? ((char)('A' + n)).ToString() : n.ToString(), new Vector2(Rand.Next(50, Tamanho.Width - 50), Rand.Next(50, Tamanho.Height - 50))))
                .ToList();
        }

        public Rectangle Tamanho { get; private set; } =
            new Rectangle(0, 0, 1280, 600);

        public List<Local> Locais { get; private set; }


        public Local LocailAleatorio() => Locais[Rand.Next(Locais.Count)];
    }
}