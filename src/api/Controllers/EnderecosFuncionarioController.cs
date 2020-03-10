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
    [Route("enderecosFuncionario")]
    [SwaggerTag("Create, edit, delete and retrieve cliente enderecos")]
    public class EnderecosFuncionarioController : ControllerBase
    {
        private readonly IEnderecosFuncionarioService _enderecosFuncionarioService;

        public EnderecosFuncionarioController(IEnderecosFuncionarioService enderecoFuncionarioService)
        {
            _enderecosFuncionarioService = enderecoFuncionarioService;
        }
        #region Cadastro Endereco Cliente
        [HttpGet]
        [SwaggerOperation(
          Summary = "Retrieve a paginated list of enderecos",
          Description = "Retrieves only enderecos"
      )]
        [SwaggerResponse(200, "List of enderecos filtered by the informed parameters", typeof(Pagination<EnderecoFuncionario>))]
        public async Task<ActionResult> List(
          [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
          [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit,
          [SwaggerParameter("Owner of enderecos")][BindRequired] int funcionarioId)
        {
            var pagination = await _enderecosFuncionarioService.ListAsync(offset, limit, funcionarioId);

            return Ok(pagination);
        }

        [HttpGet("{funcionarioId}/{enderecoFunId}")]
        [SwaggerOperation(
            Summary = "Retrieve a Contato by their id",
            Description = "Retrieves Endereco only"
        )]
        [SwaggerResponse(200, "A Endereco filtered", typeof(EnderecoFuncionario))]
        public async Task<ActionResult> Get([SwaggerParameter("contatoCli's id")]int enderecoFunId)
        {
            var enderecoFuncionario = await _enderecosFuncionarioService.GetAsync(enderecoFunId);

            return Ok(enderecoFuncionario);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Endereco Cliente",
            Description = "Creates a new product if all validations are succeded"
        )]
        [SwaggerResponse(201, "The product was successfully created", typeof(EnderecoFuncionario))]
        public async Task<ActionResult> Post([FromBody] EnderecoFuncionario enderecoFuncionario)
        {
            var created = await _enderecosFuncionarioService.CreateAsync(enderecoFuncionario);

            return CreatedAtAction(nameof(Post), new { id = enderecoFuncionario.FuncionarioId }, created);
        }

        [HttpPut("{funcionarioId}/{enderecoFunId}")]
        [SwaggerOperation(
            Summary = "Edits an existing Endereco by their Id",
            Description = "Edits an existing Endereco if all validations are succeded"
        )]
        [SwaggerResponse(200, "The Endereco was successfully edited", typeof(EnderecoFuncionario))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("enderecoCli's Id")] int enderecoFunId,
            [FromBody] EnderecoFuncionario enderecoFuncionario)
        {
            var edited = await _enderecosFuncionarioService.UpdateAsync(enderecoFunId, enderecoFuncionario);

            return Ok(edited);
        }

        [HttpDelete("{funcionarioId}/{enderecoFunId}")]
        [SwaggerOperation(
            Summary = "Deletes a Endereco by their Id",
            Description = "Deletes a Endereco if that Endereco is deletable"
        )]
        [SwaggerResponse(204, "The Endereco was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("contatoCli's Id")]int enderecoFunId)
        {
            await _enderecosFuncionarioService.DeleteAsync(enderecoFunId);

            return NoContent();
        }
        #endregion
    }
}