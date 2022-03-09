using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia
{
    public class MonitoriaContexto : DbContext
    {
        public DbSet<CronProcessamento> CronProcessamentos { get; set; }

        public MonitoriaContexto(DbContextOptions<MonitoriaContexto> opcoes) : base(opcoes)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CronProcessamento>(entidade =>
            {
                entidade.HasKey(c => c.Id);
                entidade.Property(c => c.ArvoreDados)
                    .HasConversion(
                        v => JsonConvert.SerializeObject(v),
                        v => JsonConvert.DeserializeObject<IDadoNo>(v)
                    );
            });
        }
    }
}
