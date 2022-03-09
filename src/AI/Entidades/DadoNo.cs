using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Entidades
{
    [JsonConverter(typeof(JsonSubTypes.JsonSubtypes), "Tipo")]
    [JsonSubTypes.JsonSubtypes.KnownSubType(typeof(SQLDadoNo), TipoNo.SQLBaseGlobal)]
    [JsonSubTypes.JsonSubtypes.KnownSubType(typeof(SQLDadoNo), TipoNo.SQLBaseMonitoria)]
    [JsonSubTypes.JsonSubtypes.FallBackSubType(typeof(DadoNo))]
    public interface IDadoNo
    {
        TipoNo Tipo { get; set; }
        public List<IDadoNo> Filhos { get; set; }
    }


    public class DadoNo : IDadoNo
    {
        public TipoNo Tipo { get; set; }
        public List<IDadoNo> Filhos { get; set; }
    }

    public class SQLDadoNo : DadoNo
    {
        public TipoSQLFuncao Funcao { get; set; }
        public string Sql { get; set; }
        public string VariavelContexto { get; set; }
    }
}
