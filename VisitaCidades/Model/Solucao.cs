using System.Collections.Generic;

namespace VisitaCidades.Model
{
    public class Solucao
    {
        public float Custo { get; set; }

        public IEnumerable<Rota> Rotas { get; set; }
    }
}