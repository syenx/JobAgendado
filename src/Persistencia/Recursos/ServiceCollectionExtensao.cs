using Amazon.SecretsManager;
using EDM.RFLocal.Interceptador.Repositorio.Recursos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Repositorios;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Recursos
{
    public static class ServiceCollectionExtensao
    {
        public static IServiceCollection ConfigurarRepositorio(this IServiceCollection services,
            IConfiguration configuracao)
        {
            services.AddAWSService<IAmazonSecretsManager>();
            services.AddDbContextPool<MonitoriaContexto>((sp, opcoes) =>
            {
                var servicoSecret = sp.GetRequiredService<IAmazonSecretsManager>();
                var connectionString = GestorCredencial.GetSecret("SecretMonitoria", "BaseMonitoria", configuracao, servicoSecret);
                opcoes.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            services.AddDbContextPool<GlobalContexto>((sp, options) =>
            {
                var servicoSecret = sp.GetRequiredService<IAmazonSecretsManager>();
                var connectionString = GestorCredencial.GetSecret("SecretGlobal", "BaseGlobal", configuracao, servicoSecret);
                options.UseSqlServer(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                //options.LogTo(Console.WriteLine);
            });
            services.AddScoped<ICronProcessamentoRepositorio, CronProcessamentoRepositorio>();
            services.AddScoped<IMonitoriaRepositorio, MonitoriaSqlDinamicoRepositorio>();
            services.AddScoped<IGlobalRepositorio, GlobalSqlDinamicoRepositorio>();
            return services;
        }

        public static IServiceProvider StartInicialRepositorio(this IServiceProvider provider)
        {
            var contexto = provider.GetRequiredService<MonitoriaContexto>();
            var log = provider.GetRequiredService<ILogger<MonitoriaContexto>>();
            contexto.Database.EnsureCreated();
            RelationalDatabaseCreator databaseCreator = (RelationalDatabaseCreator)contexto.Database.GetService<IDatabaseCreator>();
            try
            {
                log.LogInformation("Fazendo a migração da base...");
                databaseCreator.CreateTables();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already exists"))
                {
                    log.LogInformation("Tabelas da aplicação já existem. A migração não foi feita.");
                    return provider;
                }
                throw;
            }

            return provider;
        }
    }
}
