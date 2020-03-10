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
    [Route("enderecosCliente")]
    [SwaggerTag("Create, edit, delete and retrieve cliente enderecos")]
    public class EnderecosClienteController : ControllerBase
    {

        private readonly IEnderecosClienteService _enderecosClienteService;

        public EnderecosClienteController(IEnderecosClienteService enderecoClienteService)
        {
            _enderecosClienteService = enderecoClienteService;
        }
        #region Cadastro Endereco Cliente
        [HttpGet]
        [SwaggerOperation(
          Summary = "Retrieve a paginated list of enderecos",
          Description = "Retrieves only enderecos"
      )]
        [SwaggerResponse(200, "List of enderecos filtered by the informed parameters", typeof(Pagination<EnderecoCliente>))]
        public async Task<ActionResult> List(
          [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
          [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit,
          [SwaggerParameter("Owner of enderecos")][BindRequired] int clienteId)
        {
            var pagination = await _enderecosClienteService.ListAsync(offset, limit, clienteId);

            return Ok(pagination);
        }

        [HttpGet("{clienteId}/{enderecoCliId}")]
        [SwaggerOperation(
            Summary = "Retrieve a Contato by their id",
            Description = "Retrieves Endereco only"
        )]
        [SwaggerResponse(200, "A Endereco filtered", typeof(EnderecoCliente))]
        public async Task<ActionResult> Get([SwaggerParameter("contatoCli's id")]int enderecoCliId)
        {
            var enderecoCliente = await _enderecosClienteService.GetAsync(enderecoCliId);

            return Ok(enderecoCliente);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Endereco Cliente",
            Description = "Creates a new product if all validations are succeded"
        )]
        [SwaggerResponse(201, "The product was successfully created", typeof(EnderecoCliente))]
        public async Task<ActionResult> Post([FromBody] EnderecoCliente enderecoCliente)
        {
            var created = await _enderecosClienteService.CreateAsync(enderecoCliente);

            return CreatedAtAction(nameof(Post), new { id = enderecoCliente.EnderecoCliId }, created);
        }

        [HttpPut("{clienteId}/{enderecoCliId}")]
        [SwaggerOperation(
            Summary = "Edits an existing Endereco by their Id",
            Description = "Edits an existing Endereco if all validations are succeded"
        )]
        [SwaggerResponse(200, "The Endereco was successfully edited", typeof(EnderecoCliente))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("enderecoCli's Id")] int enderecoCliId,
            [FromBody] EnderecoCliente enderecoCliente)
        {
            var edited = await _enderecosClienteService.UpdateAsync(enderecoCliId, enderecoCliente);

            return Ok(edited);
        }

        [HttpDelete("{clienteId}/{enderecoCliId}")]
        [SwaggerOperation(
            Summary = "Deletes a Endereco by their Id",
            Description = "Deletes a Endereco if that Endereco is deletable"
        )]
        [SwaggerResponse(204, "The Endereco was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("contatoCli's Id")]int enderecoCliId)
        {
            await _enderecosClienteService.DeleteAsync(enderecoCliId);

            return NoContent();
        }
        #endregion
    }
}