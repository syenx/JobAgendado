using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Abstracoes
{
    public abstract class RepositorioEntidadeAbstrato<T> : IRepositorio<T> where T : Entidade, new()
    {
        private readonly ILogger<RepositorioEntidadeAbstrato<T>> _log;
        private readonly MonitoriaContexto _contexto;
        private readonly DbSet<T> _tabela;
        private AsyncPolicy PoliticaRetry { get; set; }
        private TimeSpan[] delays = new[] {
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(4),
            TimeSpan.FromSeconds(8)
        };

        protected RepositorioEntidadeAbstrato(ILogger<RepositorioEntidadeAbstrato<T>> log,
            MonitoriaContexto contexto)
        {
            _log = log;
            _contexto = contexto;
            _tabela = _contexto.Set<T>();

            PoliticaRetry = Policy.Handle<ObjectDisposedException>()
                                         .WaitAndRetryAsync(delays,
                                         (exception, timespan, retryCount, context) =>
                                         {
                                             log.LogWarning("Ocorreu um 'ObjectDisposedException', fazendo retentativas...");
                                         });
        }

        protected virtual IQueryable<T> ObterTodosNoTracking()
        {
            return _tabela.AsNoTracking();
        }

        public async virtual Task<T> Obter(int id)
        {
            T entidade = default;
            await PoliticaRetry.ExecuteAsync(async () =>
            {
                entidade = await ObterTodosNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            });
            return entidade;
        }

        public async virtual Task<List<T>> ObterTodos()
        {
            List<T> entidades = default;
            await PoliticaRetry.ExecuteAsync(async () =>
            {
                entidades = await ObterTodosNoTracking().ToListAsync();
            });
            return entidades;
        }

        public virtual Task Adicionar(T entidade)
        {
            _tabela.Add(entidade);
            return SalvarContextoComRetryAsync();
        }

        public async virtual Task<bool> Atualizar(T entidade)
        {
            _contexto.Entry(entidade).State = EntityState.Modified;
            await SalvarContextoComRetryAsync();
            return true;
        }

        public async virtual Task<bool> Remover(T entidade)
        {
            _contexto.Entry(entidade).State = EntityState.Deleted;
            await SalvarContextoComRetryAsync();
            return true;
        }

        protected virtual Task SalvarContextoComRetryAsync()
        {
            return PoliticaRetry.ExecuteAsync(() => _contexto.SaveChangesAsync());
        }
    }
}
