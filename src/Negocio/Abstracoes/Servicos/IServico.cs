using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Servicos
{
    public interface IServico<T> where T : IEntidade, new()
    {
        Task<T> Obter(int id);
        Task<List<T>> ObterTodos();
        Task Adicionar(T entidade);
        Task<bool> Atualizar(T entidade);
        Task<bool> Remover(T entidade);
    }
}
