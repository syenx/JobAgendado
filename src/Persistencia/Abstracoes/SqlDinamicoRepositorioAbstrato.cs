using Dapper;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Abstracoes
{
    public abstract class SqlDinamicoRepositorioAbstrato<TContexto> : ISQLRepositorio where TContexto : DbContext
    {
        private readonly ILogger<SqlDinamicoRepositorioAbstrato<TContexto>> _log;
        private readonly TContexto _contexto;
        private AsyncPolicy PoliticaRetry { get; set; }
        private TimeSpan[] delays = new[] {
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(4),
            TimeSpan.FromSeconds(8)
        };

        protected SqlDinamicoRepositorioAbstrato(ILogger<SqlDinamicoRepositorioAbstrato<TContexto>> log, TContexto contexto)
        {
            _log = log;
            _contexto = contexto;

            PoliticaRetry = Policy.Handle<Exception>()
                                         .WaitAndRetryAsync(delays,
                                         (exception, timespan, retryCount, context) =>
                                         {
                                             log.LogWarning("Ocorreu um, fazendo retentativas...");
                                         });
        }

        public async virtual Task<List<dynamic>> ConsultarAsync(string sql, object parametros)
        {
            List<dynamic> lista = null;
            await PoliticaRetry.ExecuteAsync(async () =>
            {
                var conexao = _contexto.Database.GetDbConnection();
                var result = await conexao.QueryAsync<dynamic>(sql, parametros);
                lista = result.AsList();
            });

            return lista;
        }

        public async virtual Task ComandoAsync(string sql, List<dynamic> parametros)
        {
            await PoliticaRetry.ExecuteAsync(async () =>
            {
                var conexao = _contexto.Database.GetDbConnection();
                parametros = NularParametrosCasoVazio(parametros);
                var qtd = await conexao.ExecuteAsync(sql, parametros);
            });

        }

        private List<dynamic> NularParametrosCasoVazio(List<dynamic> parametros)
        {
            if (parametros != null && !parametros.Any()) return null;

            return parametros;
        }
    }
}
