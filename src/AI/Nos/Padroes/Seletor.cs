using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Enums;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Nos.Padroes
{
    public class Seletor : No
    {
        public Seletor() : base() { }

        public Seletor(List<No> filhos) : base(filhos) { }

        public async override Task<EstadoNo> ExecutarAsync()
        {
            foreach (var no in filhos)
            {
                var resultado = await no.ExecutarAsync();
                if (resultado == EstadoNo.Sucesso)
                {
                    _estado = EstadoNo.Sucesso;
                    return _estado;
                }
            }
            _estado = EstadoNo.Falha;
            return _estado;
        }
    }
}
