using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Abstracoes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Repositorios
{
    class GlobalSqlDinamicoRepositorio : SqlDinamicoRepositorioAbstrato<GlobalContexto>, IGlobalRepositorio
    {

        public GlobalSqlDinamicoRepositorio(ILogger<GlobalSqlDinamicoRepositorio> log,
            GlobalContexto contexto) : base(log, contexto)
        {
        }

        public override Task ComandoAsync(string sql, List<dynamic> parametros)
        {
            throw new NotSupportedException();
        }
    }
}
