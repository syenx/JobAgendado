using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Enums;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Nos.Dinamicos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI;
using Microsoft.Extensions.Logging;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Fabricas
{
    public class NoSQLFabrica : INoFabrica
    {
        private readonly IGlobalRepositorio _globalRepositorio;
        private readonly IMonitoriaRepositorio _monitoriaRepositorio;
        private readonly ILogger<NoSQL> _logger;

        public NoSQLFabrica(ILogger<NoSQL> logger,
            IGlobalRepositorio globalRepositorio,
            IMonitoriaRepositorio monitoriaRepositorio)
        {
            _logger = logger;
            _globalRepositorio = globalRepositorio;
            _monitoriaRepositorio = monitoriaRepositorio;
        }

        public No CriarNo(IDadoNo dado)
        {
            var dadoSQL = (SQLDadoNo)dado;
            var no = new NoSQL(_logger, ObterRepositorio(dado.Tipo));
            no.Configurar(dadoSQL);
            return no;
        }

        private ISQLRepositorio ObterRepositorio(TipoNo tipo)
        {
            return tipo == TipoNo.SQLBaseGlobal ? (ISQLRepositorio)_globalRepositorio : _monitoriaRepositorio;
        }
    }
}
