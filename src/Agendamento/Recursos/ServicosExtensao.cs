using EDM.RFLocal.Interceptador.Agendamento.Entidades.Jobs;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Agendamento.Servicos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Servicos;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace EDM.RFLocal.Interceptador.ConsoleApp.Principal.Recursos
{
    public static class ServicosExtensao
    {
        public static IServiceCollection ConfigurarAgendador(this IServiceCollection servicos)
        {
            servicos.AddScoped<JobDinamico>();
            servicos.AddSingleton<IAgendamentoServico, AgendamentoServico>();

            servicos.Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = true; // default: false
                options.Scheduling.OverWriteExistingData = true; // default: true
            });

            servicos.AddQuartz(q =>
            {
                q.SchedulerId = "Agendador-Monitor";
                q.SchedulerName = "Quartz Agendador de Jobs";
                q.UseMicrosoftDependencyInjectionJobFactory();

                // these are the defaults
                q.UseSimpleTypeLoader();
                q.UseInMemoryStore();
                q.UseDefaultThreadPool(tp =>
                {
                    tp.MaxConcurrency = 10;
                });
            });

            servicos.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            return servicos;
        }

    }
}
