using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Repositorios;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Servicos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Servicos
{
    public class CronProcessamentoServico : ICronProcessamentoServico
    {
        private readonly ILogger<CronProcessamentoServico> _log;
        private readonly ICronProcessamentoRepositorio _repositorio;
        private readonly IAgendamentoServico _agendamento;
        private readonly INoFabrica _noFabrica;

        public CronProcessamentoServico(ILogger<CronProcessamentoServico> log,
            ICronProcessamentoRepositorio repositorio,
            IAgendamentoServico agendamento,
            INoFabrica noFabrica)
        {
            _log = log;
            _repositorio = repositorio;
            _agendamento = agendamento;
            _noFabrica = noFabrica;
        }

        #region Consulta

        public Task<CronProcessamento> Obter(int id)
        {
            return Obter(id, true);
        }

        public async Task<CronProcessamento> Obter(int id, bool inicializar)
        {
            var job = await _repositorio.Obter(id);
            if (!inicializar)
                return job;

            var inicializado = job.Inicializar(_noFabrica);
            return inicializado ? job : null;
        }

        public Task<List<CronProcessamento>> ObterTodos()
        {
            return ObterTodos(true);
        }

        public async Task<List<CronProcessamento>> ObterTodos(bool inicializar)
        {
            var jobs = await _repositorio.ObterTodos();
            if (!inicializar)
                return jobs;

            var inicializados = new List<CronProcessamento>();
            foreach (var job in jobs)
            {
                var inicializado = job.Inicializar(_noFabrica);
                if (inicializado)
                    inicializados.Add(job);
            }

            return inicializados;
        }

        #endregion

        public async Task Adicionar(CronProcessamento processamento)
        {
            await _repositorio.Adicionar(processamento);
            if (processamento.Ativo)
                await _agendamento.CriarJob(processamento);
        }

        public async Task<bool> Atualizar(CronProcessamento processamento)
        {
            var removido = await PararJobAoAtualizar(processamento.Id);
            if (!removido) return false;

            var atualizado = await _repositorio.Atualizar(processamento);
            if (!atualizado) return false;

            if (processamento.Ativo)
                await _agendamento.CriarJob(processamento);

            return true;
        }

        private async Task<bool> PararJobAoAtualizar(int id)
        {
            var processamentoAntigo = await Obter(id, false);
            return await _agendamento.RemoverJob(processamentoAntigo);
        }

        public async Task<bool> Remover(CronProcessamento processamento)
        {
            processamento = await _repositorio.Obter(processamento.Id);
            if (processamento == null || string.IsNullOrEmpty(processamento.Nome)) return false;

            var removido = await _agendamento.RemoverJob(processamento);
            if (!removido) return false;

            removido = await _repositorio.Remover(processamento);
            return removido;
        }

        public async Task<bool> AtivarDesativar(int id, bool ativar, bool forcar = false)
        {
            var processamento = await _repositorio.Obter(id);
            if (!forcar && processamento.Ativo == ativar) return false;

            processamento.Ativo = ativar;
            return await Atualizar(processamento);
        }

        public async Task StartarProcessamentosAtivos()
        {
            var todos = await _repositorio.ObterTodos();
            var ativos = todos.Where(c => c.Ativo).ToList();
            foreach (var processamento in ativos)
            {
                var removido = await _agendamento.RemoverJob(processamento);
                if (!removido) continue;

                await _agendamento.CriarJob(processamento);
            }
        }
    }
}
