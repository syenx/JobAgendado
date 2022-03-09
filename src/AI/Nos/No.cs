using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI
{
    public abstract class No
    {
        protected EstadoNo _estado { get; set; }
        public EstadoNo Estado => _estado;

        private No _pai;
        protected List<No> filhos = new List<No>();
        private Dictionary<string, object> _contexto = new Dictionary<string, object>();

        public No() { _pai = null; }
        public No(List<No> filhos) : this()
        {
            PreencherFilhos(filhos);
        }

        public void Configurar(List<No> filhos)
        {
            PreencherFilhos(filhos);
        }

        public void PreencherFilhos(List<No> filhos)
        {
            foreach (var no in filhos)
                Atachar(no);
        }

        public void Atachar(No no)
        {
            filhos.Add(no);
            no._pai = this;
        }

        public void Desatachar(No no)
        {
            filhos.Remove(no);
            no._pai = null;
        }

        public object ObterDado(string chave)
        {
            if (_contexto.TryGetValue(chave, out object valor))
                return valor;

            No no = _pai;
            while (no != null)
            {
                valor = no.ObterDado(chave);
                if (valor != null)
                    return valor;

                no = no._pai;
            }
            return null;
        }

        public bool LimparDado(string chave)
        {
            if (_contexto.ContainsKey(chave))
            {
                _contexto.Remove(chave);
                return true;
            }

            var no = _pai;
            while (no != null)
            {
                bool limpo = no.LimparDado(chave);
                if (limpo) return true;
                no = no._pai;
            }

            return false;
        }

        public void LimparDados()
        {
            _contexto = new Dictionary<string, object>();
            foreach (var no in filhos)
                no.LimparDados();
        }

        public void ArmazenarDado(string chave, object valor)
        {
            _contexto[chave] = valor;
        }

        public void ArmazenarDadoNaRaiz(string chave, object valor)
        {
            if (Pai != null)
                Pai.ArmazenarDado(chave, valor);
            else
                ArmazenarDado(chave, valor);
        }

        public No Pai => _pai;
        public List<No> Filhos => filhos;
        public bool TemFilhos => filhos.Any();
        public abstract Task<EstadoNo> ExecutarAsync();
    }
}
