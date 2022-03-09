using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI
{
    public class Arvore
    {
        private No _raiz = null;
        private Arvore(No raiz)
        {
            _raiz = raiz;
        }
        public No Raiz => _raiz;


        public Task Executar()
        {
            if (_raiz != null)
                return _raiz.ExecutarAsync();

            return Task.CompletedTask;
        }
        public static Arvore Criar(INoFabrica noFabrica, IDadoNo dado)
        {
            var raiz = CriarNo(noFabrica, dado);
            if (raiz == null) return null;

            return new Arvore(raiz);
        }

        private static No CriarNo(INoFabrica noFabrica, IDadoNo dado)
        {
            var no = noFabrica.CriarNo(dado);
            if (dado.Filhos?.Any() ?? false)
            {
                var filhos = new List<No>();
                foreach (var dadoFilho in dado.Filhos)
                {
                    var filho = CriarNo(noFabrica, dadoFilho);
                    if (filho != null)
                        filhos.Add(filho);
                }
                no.PreencherFilhos(filhos);
            }
            return no;
        }
    }
}
