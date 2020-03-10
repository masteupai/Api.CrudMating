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
    [Route("contatosCliente")]
    [SwaggerTag("Create, edit, delete and retrieve cliente contatos")]
    public class ContatosClienteController : ControllerBase
    {

        private readonly IContatosClienteService _contatoClienteService;

        public ContatosClienteController(IContatosClienteService contatoClienteService)
        {
            _contatoClienteService = contatoClienteService;
        }
        #region Cadastro Contato contatoCliente
        [HttpGet]
        [SwaggerOperation(
          Summary = "Retrieve a paginated list of contatos",
          Description = "Retrieves only contatos that were created by the authenticated contatoCliente"
      )]
        [SwaggerResponse(200, "List of contatos filtered by the informed parameters", typeof(Pagination<ContatoCliente>))]
        public async Task<ActionResult> List(
          [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
          [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit,
          [SwaggerParameter("Owner of contatos")][BindRequired] int clienteId)
        {        
            var pagination = await _contatoClienteService.ListAsync(offset, limit, clienteId);

            return Ok(pagination);
        }

        [HttpGet("{clienteId}/{contatoCliId}")]
        [SwaggerOperation(
            Summary = "Retrieve a Contato by their id",
            Description = "Retrieves Contato only"
        )]
        [SwaggerResponse(200, "A Contato filtered", typeof(ContatoCliente))]
        public async Task<ActionResult> Get([SwaggerParameter("contatoCli's id")]int contatoCliId)
        {
            var contatoCliente = await _contatoClienteService.GetAsync(contatoCliId);

            return Ok(contatoCliente);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Contato Cliente",
            Description = "Creates a new product if all validations are succeded"
        )]
        [SwaggerResponse(201, "The product was successfully created", typeof(ContatoCliente))]
        public async Task<ActionResult> Post([FromBody] ContatoCliente contatoCliente)
        {
            var created = await _contatoClienteService.CreateAsync(contatoCliente);

            return CreatedAtAction(nameof(Post), new { id = contatoCliente.ContatoCliId }, created);
        }

        [HttpPut("{clienteId}/{contatoCliId}")]
        [SwaggerOperation(
            Summary = "Edits an existing contatoCliente by their Id",
            Description = "Edits an existing contatoCliente if all validations are succeded"
        )]
        [SwaggerResponse(200, "The contatoCliente was successfully edited", typeof(ContatoCliente))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("contatoCli's Id")] int contatoCliId,
            [FromBody] ContatoCliente contatoCliente)
        {
            var edited = await _contatoClienteService.UpdateAsync(contatoCliId, contatoCliente);

            return Ok(edited);
        }

        [HttpDelete("{clienteId}/{contatoCliId}")]
        [SwaggerOperation(
            Summary = "Deletes a contatoCliente by their Id",
            Description = "Deletes a contatoCliente if that contatoCliente is deletable"
        )]
        [SwaggerResponse(204, "The contatoCliente was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("contatoCli's Id")]int contatoCliId)
        {
            await _contatoClienteService.DeleteAsync(contatoCliId);

            return NoContent();
        }
        #endregion
    }
}