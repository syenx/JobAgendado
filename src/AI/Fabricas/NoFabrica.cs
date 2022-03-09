using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Enums;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Fabricas
{
    public class NoFabrica : INoFabrica
    {
        private NoPadraoFabrica _padraoFabrica;
        private NoSQLFabrica _sQLFabrica;

        public NoFabrica(NoPadraoFabrica padraoFabrica, NoSQLFabrica sQLFabrica)
        {
            _padraoFabrica = padraoFabrica;
            _sQLFabrica = sQLFabrica;
        }

        public No CriarNo(IDadoNo dado)
        {
            switch (dado.Tipo)
            {
                case TipoNo.SQLBaseGlobal:
                case TipoNo.SQLBaseMonitoria:
                    return _sQLFabrica.CriarNo(dado);
                default:
                    return _padraoFabrica.CriarNo(dado);
            }
        }
    }
}
