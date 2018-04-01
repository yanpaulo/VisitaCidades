using System.Threading.Tasks;

namespace VisitaCidades.Model
{
    public interface IAlgoritmo
    {
        Problema Problema { get; }
        Solucao Solucao { get; }

        void Executa();

        Task ExecutaAsync();
    }
}