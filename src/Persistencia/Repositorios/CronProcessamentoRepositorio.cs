using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Repositorios;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Abstracoes;
using Microsoft.Extensions.Logging;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Repositorios
{
    public class CronProcessamentoRepositorio : RepositorioEntidadeAbstrato<CronProcessamento>, ICronProcessamentoRepositorio
    {
        public CronProcessamentoRepositorio(ILogger<CronProcessamentoRepositorio> log, MonitoriaContexto contexto) :
            base(log, contexto)
        {

        }
    }
}
