using EDM.RFLocal.Interceptador.ConsoleApp.Principal.Recursos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Recursos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.BackgroundApp.Recursos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Recursos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia.Recursos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.BackgroundApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(logging => logging.AddAWSProvider());
            services.AddAutoMapper(typeof(PerfilMapeamento));
            services.AddControllers().AddNewtonsoftJson();
            services.ConfigurarAI();
            services.ConfigurarNegocio();
            services.ConfigurarRepositorio(Configuration);
            //services.ConfigurarRepositorioMock();
            services.ConfigurarAgendador();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Configuração API"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Iniciadores da aplicação
            //using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            //{
            serviceProvider
                .StartInicialRepositorio()
                .StartInicialNegocio();
            //}

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Configuração API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
