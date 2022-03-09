using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes
{
    public interface INoFabrica
    {
        No CriarNo(IDadoNo dado);
    }
}
