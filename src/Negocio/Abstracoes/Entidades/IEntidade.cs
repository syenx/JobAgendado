namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Entidades
{
    public interface IEntidade
    {
        int Id { get; set; }
        bool Ativo { get; set; }
    }
}
