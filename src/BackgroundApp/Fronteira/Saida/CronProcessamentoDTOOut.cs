using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.BackgroundApp.Fronteira
{
    public class CronProcessamentoDTOOut
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Grupo { get; set; }
        public string Descricao { get; set; }
        public IDadoNo ArvoreDados { get; set; }
        public string ExpressaoCron { get; set; }
        public bool Ativo { get; set; }
    }
}
