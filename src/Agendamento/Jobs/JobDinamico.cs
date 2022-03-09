using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Threading.Tasks;

namespace EDM.RFLocal.Interceptador.Agendamento.Entidades.Jobs
{
    public class JobDinamico : IJob
    {
        public const string PROCESSAMENTO_CHAVE = "PROCESSAMENTO";
        private readonly ILogger<JobDinamico> _logger;
        private readonly INoFabrica _noFabrica;
        public JobDinamico(ILogger<JobDinamico> logger, INoFabrica noFabrica)
        {
            _logger = logger;
            _noFabrica = noFabrica;
        }

        public async Task Execute(IJobExecutionContext contexto)
        {
            try
            {
                var nomeJob = contexto.JobDetail.Key.ToString();

                var processamento = MontarProcessamento(contexto.JobDetail.JobDataMap, nomeJob);
                if (processamento == null)
                {
                    _logger.LogInformation($"o processamento '{nomeJob}' não foi inicado devido a erros na criação.");
                    return;
                }

                _logger.LogInformation($"Iniciando o processamento '{nomeJob}'");
                await processamento.Executar();
                _logger.LogInformation($"Concluído o processamento '{nomeJob}'");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private IProcessamento MontarProcessamento(JobDataMap mapa, string nomeJob)
        {
            if (!ValidaMapa(mapa, nomeJob, out string processamentoJson)) return null;

            _logger.LogDebug("Recuperando árvore");

            if (TentarConverterProcessamento(processamentoJson, nomeJob, out CronProcessamento processamento))
                InicializarProcessamento(processamento, nomeJob);

            return processamento;
        }

        private bool ValidaMapa(JobDataMap mapa, string nomeJob, out string json)
        {
            json = null;
            if (mapa.ContainsKey(PROCESSAMENTO_CHAVE))
            {
                json = mapa.GetString(PROCESSAMENTO_CHAVE);
                if (!string.IsNullOrEmpty(json))
                    return true;

                _logger.LogError($"Valor da chave '{PROCESSAMENTO_CHAVE}' no '{nomeJob}' não pode ser nula ou vazia.");
            }
            else
                _logger.LogError($"Processamento '{nomeJob}' não possui '{PROCESSAMENTO_CHAVE}' no mapa.");

            return false;
        }

        private bool TentarConverterProcessamento(string json, string nomeJob, out CronProcessamento processamento)
        {
            processamento = JsonConvert.DeserializeObject<CronProcessamento>(json);
            if (processamento != null) return true;

            _logger.LogError($"Erro de conversão. Não foi possível recuperar o processamento '{nomeJob}' a partir do json '{json}'.");
            return false;
        }

        private void InicializarProcessamento(CronProcessamento processamento, string nomeJob)
        {
            if (processamento.Inicializar(_noFabrica))
                return;

            _logger.LogError($"Não foi possível inicializar o processamento '{nomeJob}'.");
        }
    }
}
