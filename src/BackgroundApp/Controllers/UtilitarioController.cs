using EDM.RFLocal.Sistema.Monitor.JobsAgendados.AI.Abstracoes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.BackgroundApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("[controller]")]
    public class UtilitarioController : ControllerBase
    {
        protected readonly IMonitoriaRepositorio _repositorio;
        public UtilitarioController(IMonitoriaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Adicionar([FromBody] Corpo corpo)
        {
            try
            {
                if (corpo == null || string.IsNullOrWhiteSpace(corpo.Sql))
                    throw new ArgumentNullException("Parâmetros não podem ser nulos");

                await _repositorio.ComandoAsync(corpo.Sql, corpo.Parametros);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }

    public class Corpo
    {
        public string Sql { get; set; }
        public List<dynamic> Parametros { get; set; }
    }
}
