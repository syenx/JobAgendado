using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Abstracoes;
using Microsoft.Extensions.Logging;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Repositorios
{
    public class MonitoriaSqlDinamicoRepositorio : SqlDinamicoRepositorioAbstrato<MonitoriaContexto>, IMonitoriaRepositorio
    {
        public MonitoriaSqlDinamicoRepositorio(ILogger<MonitoriaSqlDinamicoRepositorio> log,
            MonitoriaContexto contexto) : base(log, contexto)
        {
        }
    }
}
