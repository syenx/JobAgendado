using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Servicos
{
    public interface IAgendamentoServico
    {
        Task CriarJob(CronProcessamento processamento);
        Task<bool> RemoverJob(CronProcessamento processamento);
    }
}
