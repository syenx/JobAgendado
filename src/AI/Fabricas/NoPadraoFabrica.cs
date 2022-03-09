using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Enums;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Nos.Padroes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Fabricas
{
    public class NoPadraoFabrica : INoFabrica
    {
        public No CriarNo(IDadoNo dado)
        {
            switch (dado.Tipo)
            {
                case TipoNo.Sequencia:
                    return new Sequencia();
                case TipoNo.Seletor:
                    return new Seletor();
                case TipoNo.ParaleloAoMenosUm:
                    return new ParaleloAoMenosUmSucesso();
                case TipoNo.ParaleloTodos:
                    return new ParaleloTodosSucesso();
                default:
                    return null;
            }
        }
    }
}
