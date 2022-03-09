using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes
{
    public interface ISQLRepositorio
    {
        Task<List<dynamic>> ConsultarAsync(string sql, object parametros);
        Task ComandoAsync(string sql, List<dynamic> parametros);

    }

    public interface IMonitoriaRepositorio : ISQLRepositorio
    {
    }

    public interface IGlobalRepositorio : ISQLRepositorio
    {
    }
}
