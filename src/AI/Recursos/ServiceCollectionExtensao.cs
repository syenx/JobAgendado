using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Fabricas;
using Microsoft.Extensions.DependencyInjection;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Recursos
{
    public static class ServiceCollectionExtensao
    {
        public static IServiceCollection ConfigurarAI(this IServiceCollection services)
        {
            services.AddScoped<NoPadraoFabrica>();
            services.AddScoped<NoSQLFabrica>();
            services.AddScoped<INoFabrica, NoFabrica>();
            return services;
        }
    }
}
