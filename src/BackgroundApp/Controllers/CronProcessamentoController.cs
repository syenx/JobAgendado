using AutoMapper;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.BackgroundApp.Fronteira;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Servicos;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.BackgroundApp.Controllers
{
    [ApiController]
    [Route("Agendamento")]
    public class CronProcessamentoController :
        EntidadeControllerAbstrato<CronProcessamento, ICronProcessamentoServico,
            CronProcessamentoDTOIn, CronProcessamentoDTOOut>
    {
        public CronProcessamentoController(ILogger<CronProcessamentoController> logger,
            IMapper map, [FromServices] ICronProcessamentoServico servico) : base(logger, map, servico)
        {
        }

        [HttpGet]
        public override async Task<IActionResult> ObterTodos()
        {
            var preocessamentos = await _servico.ObterTodos(false);
            var saida = _map.Map<List<CronProcessamentoDTOOut>>(preocessamentos);
            return Ok(saida);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> Obter([FromRoute] int id)
        {
            var processamento = await _servico.Obter(id, false);
            var dto = _map.Map<CronProcessamentoDTOOut>(processamento);
            return Ok(dto);
        }

        #region Ativar Desativar

        [HttpPost("{id}/ativar")]
        public async Task<IActionResult> Ativar([FromRoute] int id)
        {
            return Ok(await _servico.AtivarDesativar(id, true));
        }

        [HttpPost("{id}/ativar/forcar")]
        public async Task<IActionResult> AtivarForcado([FromRoute] int id)
        {
            return Ok(await _servico.AtivarDesativar(id, true, true));
        }

        [HttpPost("{id}/desativar")]
        public async Task<IActionResult> Desativar([FromRoute] int id)
        {
            return Ok(await _servico.AtivarDesativar(id, false));
        }

        [HttpPost("{id}/desativar/forcar")]
        public async Task<IActionResult> DesativarForcado([FromRoute] int id)
        {
            return Ok(await _servico.AtivarDesativar(id, false, true));
        }

        #endregion


    }
}
