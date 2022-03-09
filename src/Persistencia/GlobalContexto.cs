using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Persistencia
{
    public class GlobalContexto : DbContext
    {
        public GlobalContexto(DbContextOptions<GlobalContexto> opcoes) : base(opcoes)
        {
        }
    }
}
