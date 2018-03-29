using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using static VisitaCidades.Utils;

namespace VisitaCidades.Model
{
    public class Mapa
    {
        public Rectangle Tamanho { get; private set; } =
            new Rectangle(0, 0, 800, 600);

        public List<Local> Locais { get; private set; } = 
            Enumerable
                .Range(0, 30)
                .Select(n => new Local(('A' + n).ToString(), new Vector2(Rand.Next(640), Rand.Next(480))))
                .ToList();


        public Local LocailAleatorio() => Locais[Rand.Next(Locais.Count)];
    }
}