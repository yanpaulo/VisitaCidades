using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitaCidades.Model
{
    public abstract class AlgoritmoBase : IAlgoritmo
    {
        public Problema Problema { get; private set; }

        public Solucao Solucao { get; protected set; }

        public AlgoritmoBase(Problema problema)
        {
            Problema = problema;
        }

        public abstract void Executa();

        public Task ExecutaAsync() =>
            Task.Run(() => Executa());
    }
}
