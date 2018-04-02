using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected abstract void Roda();

        public void Executa()
        {
            var sw = new Stopwatch();
            Console.WriteLine("Informacoes do Algoritmo:");
            Console.WriteLine(this);

            sw.Start();
            Roda();
            sw.Stop();

            Console.WriteLine($"Finalizou em {sw.ElapsedMilliseconds}ms");
        }

        public Task ExecutaAsync() =>
            Task.Run(() => Executa());
    }
}
