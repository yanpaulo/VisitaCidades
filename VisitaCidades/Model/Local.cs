using Microsoft.Xna.Framework;

namespace VisitaCidades.Model
{
    public class Local
    {
        public Local(string nome, Vector2 posicao)
        {
            Nome = nome;
            Posicao = posicao;
        }

        public string Nome { get; set; }

        public Vector2 Posicao { get; set; }

        public override string ToString() => Nome;
    }
}