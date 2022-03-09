using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Entidades
{
    public interface IProcessamento : IEntidade
    {
        string Nome { get; set; }
        string Grupo { get; set; }
        string Descricao { get; set; }
        IDadoNo ArvoreDados { get; }
        Task Executar();
    }
}
