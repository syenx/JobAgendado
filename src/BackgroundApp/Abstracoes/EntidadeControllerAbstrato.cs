using AutoMapper;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Entidades;
using EDM.RFLocal.Sistema.Monitor.JobsAgendados.Negocio.Abstracoes.Servicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDM.RFLocal.Sistema.Monitor.JobsAgendados.BackgroundApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class EntidadeControllerAbstrato<TEntidade, TServico, TDTOEntrada, TDTOSaida> : ControllerBase
        where TEntidade : IEntidade, new()
        where TServico : IServico<TEntidade>
    {
        protected readonly ILogger<EntidadeControllerAbstrato<TEntidade, TServico, TDTOEntrada, TDTOSaida>> _logger;
        protected readonly IMapper _map;
        protected readonly TServico _servico;

        public EntidadeControllerAbstrato(ILogger<EntidadeControllerAbstrato<TEntidade, TServico, TDTOEntrada, TDTOSaida>> logger,
            IMapper map, TServico servico)
        {
            _logger = logger;
            _map = map;
            _servico = servico;
        }

        [HttpGet]
        public virtual async Task<IActionResult> ObterTodos()
        {
            var entidades = await _servico.ObterTodos();
            var saida = _map.Map<List<TDTOSaida>>(entidades);
            return Ok(saida);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Obter([FromRoute] int id)
        {
            var entidade = await _servico.Obter(id);
            var saida = _map.Map<TDTOSaida>(entidade);
            return Ok(saida);
        }

        [HttpPost]
        public virtual async Task<IActionResult> Adicionar([FromBody] TDTOEntrada dtoEntrada)
        {
            var entidade = _map.Map<TEntidade>(dtoEntrada);
            await _servico.Adicionar(entidade);
            var saida = _map.Map<TDTOSaida>(entidade);
            return Ok(saida);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Atualizar([FromRoute] int id, [FromBody] TDTOEntrada dtoEntrada)
        {
            var entidade = _map.Map<TEntidade>(dtoEntrada);
            entidade.Id = id;
            var resultado = await _servico.Atualizar(entidade);
            return resultado ? Ok() : StatusCode(500);
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Remover([FromRoute] int id)
        {
            var entidade = Activator.CreateInstance<TEntidade>();
            entidade.Id = id;
            var resultado = await _servico.Remover(entidade);
            return resultado ? Ok() : StatusCode(500);
        }
    }
}
