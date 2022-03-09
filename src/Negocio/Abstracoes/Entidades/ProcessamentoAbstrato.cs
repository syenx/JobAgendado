using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Entidades
{
    public abstract class ProcessamentoAbstrato : Entidade, IProcessamento
    {
        public string Nome { get; set; }
        public string Grupo { get; set; }
        public string Descricao { get; set; }
        public IDadoNo ArvoreDados { get; set; }

        private Arvore _arvore;
        public bool Inicializar(INoFabrica noFabrica)
        {
            if (ArvoreDados == null) return false;
            _arvore = Arvore.Criar(noFabrica, ArvoreDados);
            return _arvore != null;
        }

        public Task Executar()
        {
            return _arvore.Executar();
        }
    }
}
