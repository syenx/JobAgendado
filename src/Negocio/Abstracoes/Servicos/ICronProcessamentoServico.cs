using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Servicos
{
    public interface ICronProcessamentoServico : IServico<CronProcessamento>
    {
        Task<CronProcessamento> Obter(int id, bool inicializar);
        Task<List<CronProcessamento>> ObterTodos(bool inicializar);
        Task<bool> AtivarDesativar(int id, bool ativar, bool forcar = false);
        Task StartarProcessamentosAtivos();
    }
}
