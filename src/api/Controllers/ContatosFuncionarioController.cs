using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domains.Models;
using API.Domains.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("contatosFuncionario")]
    [SwaggerTag("Create, edit, delete and retrieve funcionario contatos")]
    public class ContatosFuncionarioController : ControllerBase
    {

        private readonly IContatosFuncionarioService _contatoFuncionarioService;

        public ContatosFuncionarioController(IContatosFuncionarioService contatoFuncionarioService)
        {
            _contatoFuncionarioService = contatoFuncionarioService;
        }
        #region Cadastro Contato contatoCliente
        [HttpGet]
        [SwaggerOperation(
          Summary = "Retrieve a paginated list of contatos",
          Description = "Retrieves only contatos"
      )]
        [SwaggerResponse(200, "List of contatos filtered by the informed parameters", typeof(Pagination<ContatoFuncionario>))]
        public async Task<ActionResult> List(
          [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
          [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit,
          [SwaggerParameter("Owner of contatos")][BindRequired] int funcionarioId)
        {
            var pagination = await _contatoFuncionarioService.ListAsync(offset, limit, funcionarioId);

            return Ok(pagination);
        }

        [HttpGet("{FuncionarioId}/{contatoFunId}")]
        [SwaggerOperation(
            Summary = "Retrieve a Contato by their id",
            Description = "Retrieves Contato only"
        )]
        [SwaggerResponse(200, "A Contato filtered", typeof(ContatoFuncionario))]
        public async Task<ActionResult> Get([SwaggerParameter("contatoFun's Id")]int contatoFunId)
        {
            var contatoFun = await _contatoFuncionarioService.GetAsync(contatoFunId);

            return Ok(contatoFun);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Contato Cliente",
            Description = "Creates a new product if all validations are succeded"
        )]
        [SwaggerResponse(201, "The product was successfully created", typeof(ContatoFuncionario))]
        public async Task<ActionResult> Post([FromBody] ContatoFuncionario contatoFun)
        {
            var created = await _contatoFuncionarioService.CreateAsync(contatoFun);

            return CreatedAtAction(nameof(Post), new { id = contatoFun.ContatoFunId }, created);
        }

        [HttpPut("{FuncionarioId}/{contatoFunId}")]
        [SwaggerOperation(
            Summary = "Edits an existing contatoCliente by their Id",
            Description = "Edits an existing contatoCliente if all validations are succeded"
        )]
        [SwaggerResponse(200, "The contatoCliente was successfully edited", typeof(ContatoFuncionario))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("contatoFun's Id")] int contatoFunId,
            [FromBody] ContatoFuncionario contatoFun)
        {
            var edited = await _contatoFuncionarioService.UpdateAsync(contatoFunId, contatoFun);

            return Ok(edited);
        }

        [HttpDelete("{FuncionarioId}/{contatoFunId}")]
        [SwaggerOperation(
            Summary = "Deletes a contatoCliente by their Id",
            Description = "Deletes a contatoCliente if that contatoCliente is deletable"
        )]
        [SwaggerResponse(204, "The contatoCliente was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("contatoFun's Id")]int contatoFunId)
        {
            await _contatoFuncionarioService.DeleteAsync(contatoFunId);

            return NoContent();
        }
        #endregion
    }
}