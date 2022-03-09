using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Servicos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Servicos;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Recursos
{
    public static class ServiceCollectionExtensao
    {
        public static IServiceCollection ConfigurarNegocio(this IServiceCollection services)
        {
            services.AddScoped<ICronProcessamentoServico, CronProcessamentoServico>();
            return services;
        }

        public static IServiceProvider StartInicialNegocio(this IServiceProvider provider)
        {
            var processamentoServico = provider.GetRequiredService<ICronProcessamentoServico>();
            processamentoServico.StartarProcessamentosAtivos().Wait();

            return provider;
        }
    }
}
