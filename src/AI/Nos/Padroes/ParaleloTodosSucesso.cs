using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Enums;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Nos.Padroes
{
    public class ParaleloTodosSucesso : No
    {
        public ParaleloTodosSucesso() : base() { }

        public ParaleloTodosSucesso(List<No> filhos) : base(filhos) { }

        public async override Task<EstadoNo> ExecutarAsync()
        {
            var execucaoFilhos = filhos.Select(c => c.ExecutarAsync());
            var resultados = await Task.WhenAll(execucaoFilhos);
            _estado = resultados.All(c => c == EstadoNo.Sucesso) ? EstadoNo.Sucesso : EstadoNo.Falha;
            return _estado;
        }
    }
}
