using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Entidades;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades
{
    public class CronProcessamento : ProcessamentoAbstrato
    {
        public string ExpressaoCron { get; set; }
    }
}
