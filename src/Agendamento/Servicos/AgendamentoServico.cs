using EDM.RFLocal.Interceptador.Agendamento.Entidades.Jobs;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Servicos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades;
using Newtonsoft.Json;
using Quartz;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Agendamento.Servicos
{
    public class AgendamentoServico : IAgendamentoServico
    {
        private readonly IScheduler _scheduler;
        public AgendamentoServico(ISchedulerFactory fabrica)
        {
            _scheduler = fabrica.GetScheduler().Result;
        }

        public async Task CriarJob(CronProcessamento processamento)
        {
            _scheduler.Start().Wait();

            JobKey jobKey = JobKey.Create(processamento.Nome);
            var processamentoJson = JsonConvert.SerializeObject(processamento);

            var job = JobBuilder.Create<JobDinamico>().
               WithIdentity(jobKey)
               .UsingJobData("NomeJob", processamento.Nome)
               .UsingJobData(JobDinamico.PROCESSAMENTO_CHAVE, processamentoJson)
               .Build();

            var trigger = TriggerBuilder.Create()

            .WithIdentity($"{processamento.Nome}-trigger")
            .WithCronSchedule(processamento.ExpressaoCron)
            .WithDescription(processamento.ExpressaoCron)
            .StartNow()
            .Build();

            await _scheduler.ScheduleJob(job, trigger);
        }

        public async Task<bool> RemoverJob(CronProcessamento processamento)
        {

            JobKey jobKey = JobKey.Create(processamento.Nome);
            if (!await _scheduler.CheckExists(jobKey)) return true;

            return await _scheduler.DeleteJob(jobKey);
        }
    }
}
