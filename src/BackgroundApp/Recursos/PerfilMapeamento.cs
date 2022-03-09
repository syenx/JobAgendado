using AutoMapper;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.BackgroundApp.Fronteira;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.BackgroundApp.Recursos
{
    public class PerfilMapeamento : Profile
    {
        public PerfilMapeamento()
        {
            CreateMap<CronProcessamento, CronProcessamentoDTOOut>();
            CreateMap<CronProcessamentoDTOIn, CronProcessamento>();
        }
    }
}
