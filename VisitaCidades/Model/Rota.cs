using System.Collections.Generic;

namespace VisitaCidades.Model
{
    public class Rota
    {
        public Viajante Viajante { get; set; }

        public List<Local> Locais { get; set; } = new List<Local>();
    }
}