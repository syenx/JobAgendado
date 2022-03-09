using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Fabricas;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Nos.Dinamicos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.AI;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Testes
{
    public class DadoNoTeste
    {
        private const string json = @"
 {
        ""tipo"": 0,
        ""filhos"": [
            {
                ""tipo"": 5,
                ""sql"": ""SELECT * FROM 1""
            },
            {
                ""tipo"": 0,
                ""filhos"": [
                    {
                        ""tipo"": 5,
                        ""sql"": ""SELECT * FROM 2""
                    },
                    {
                        ""tipo"": 4,
                        ""sql"": ""SELECT * FROM 3""
                    }
                ]
            },
            {
                ""tipo"": 4,
                ""sql"": ""SELECT * FROM 4""
            }
        ]
    }
";
        [Fact]
        public void Desserializacao()
        {
            var no = JsonConvert.DeserializeObject<IDadoNo>(json);
            var jsonVolta = JsonConvert.SerializeObject(no);
        }

        [Fact]
        public void CriarArvore()
        {
            var dado = JsonConvert.DeserializeObject<IDadoNo>(json);
            var repoLog = new Mock<ILogger<IMonitoriaRepositorio>>().Object;
            var noSqlLog = new Mock<ILogger<NoSQL>>().Object;
            var global = new Mock<IGlobalRepositorio>().Object;
            var monitor = new Mock<IMonitoriaRepositorio>().Object;
            var noSQLFabrica = new NoSQLFabrica(noSqlLog, global, monitor);
            var noPadraoFabrica = new NoPadraoFabrica();
            var noFabrica = new NoFabrica(noPadraoFabrica, noSQLFabrica);

            var arvore = Arvore.Criar(noFabrica, dado);
        }

    }
}
