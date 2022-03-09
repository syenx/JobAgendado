using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Enums;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Nos.Dinamicos
{
    public class NoSQL : No
    {
        private readonly ISQLRepositorio _repositorio;
        private readonly ILogger<NoSQL> _logger;

        private SQLDadoNo _dado;

        public NoSQL(ILogger<NoSQL> logger, ISQLRepositorio repositorio)
        {
            _logger = logger;
            _repositorio = repositorio;
        }

        public void Configurar(SQLDadoNo dado)
        {
            _dado = dado;
            PreencherFilhos(filhos);
        }

        public async override Task<EstadoNo> ExecutarAsync()
        {
            if (string.IsNullOrEmpty(_dado.Sql))
                throw new ArgumentNullException("A string 'sql' não pode ser nula");

            try
            {
                if (_dado.Funcao == TipoSQLFuncao.Comando)
                    await ExecutarComandoAsync();
                else
                    await ExecutarConsultaAsync();

                return EstadoNo.Sucesso;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao executar {nameof(NoSQL)}.");
                return EstadoNo.Falha;
            }
        }

        private async Task ExecutarComandoAsync()
        {
            _logger.LogDebug($"Executando comando sql: {_dado.Sql}");
            var parametros = ObterVariavelContexto(_dado.VariavelContexto);
            await _repositorio.ComandoAsync(_dado.Sql, parametros);
        }

        private async Task ExecutarConsultaAsync()
        {
            _logger.LogDebug($"Executando consulta sql: {_dado.Sql}");
            var parametros = ObterParametros(_dado.Sql);
            var resultado = await _repositorio.ConsultarAsync(_dado.Sql, parametros);
            ArmazernarResultadoNoContexto(_dado.VariavelContexto, resultado);
        }

        private List<dynamic> ObterVariavelContexto(string nome)
        {
            if (string.IsNullOrEmpty(nome)) return null;

            var valor = ObterDado(nome);

            if (valor == null) return null;

            return (List<dynamic>)valor;
        }

        private void ArmazernarResultadoNoContexto(string nome, List<dynamic> lista)
        {
            if (string.IsNullOrEmpty(nome)) return;
            ArmazenarDadoNaRaiz(nome, lista);
        }

        #region Tratar Parametros do Contexto (De todos os processos)

        private Dictionary<string, object> ObterParametros(string sql)
        {
            var nomes = CapturarParametros(sql);
            if (nomes == null || !nomes.Any()) return null;

            return PreencherParametros(nomes);
        }

        private Dictionary<string, object> PreencherParametros(string[] nomesParametros)
        {
            var parametros = new Dictionary<string, object>();
            foreach (var nome in nomesParametros)
            {
                var valor = ObterDado(nome);
                parametros.Add(nome, valor);
            }
            return parametros;
        }

        private string[] CapturarParametros(string sql)
        {
            Regex regex = new Regex(@"\@([\w.$]+|""[^""]+""|'[^']+')");

            if (regex.IsMatch(sql)) return null;

            var matchs = regex.Matches(sql);
            return matchs
                .Cast<Match>()
                .Select(c => c.Value)
                .ToArray();
        }

        #endregion
    }
}
