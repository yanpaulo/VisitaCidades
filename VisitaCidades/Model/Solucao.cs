using System.Collections.Generic;

namespace VisitaCidades.Model
{
    public class Solucao
    {
        public int Custo { get; set; }

        public IEnumerable<Rota> Rotas { get; set; }
    }
}