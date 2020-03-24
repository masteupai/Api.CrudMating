using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [Route("servicofuncionarios")]
    [SwaggerTag("Create, edit, delete and retrieve funcionarios in servicos")]
    public class ServicoFuncionariosController : ControllerBase
    {
        private readonly IServicoFuncionarioService _servicoFuncionarioService;

        public ServicoFuncionariosController(IServicoFuncionarioService servicoFuncionarioService)
        {
            _servicoFuncionarioService = servicoFuncionarioService;
        }
        [HttpGet]
        [SwaggerOperation(
          Summary = "Retrieve a paginated list of funcionarios in servicos",
          Description = "Retrieves only funcionarios in servicos"
      )]
        [SwaggerResponse(200, "List of funcionarios in servicos filtered by the informed parameters", typeof(Pagination<ServicoFuncionario>))]
        public async Task<ActionResult> List(
          [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
          [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit,
          [SwaggerParameter("Limit the number of items that are returned")][BindRequired][Required] int servicoId)
        {
            var pagination = await _servicoFuncionarioService.ListAsync(offset, limit, servicoId);

            return Ok(pagination);
        }


        [HttpGet("{servicoFuncionarioId}")]
        [SwaggerOperation(
          Summary = "Retrieve a funcionarios in servicos by their id",
          Description = "Retrieves funcionarios in servicos"
      )]
        [SwaggerResponse(200, "A funcionarios in servicos filtered", typeof(ServicoFuncionario))]
        public async Task<ActionResult> Get([SwaggerParameter("servicoFuncionario Id")]int servicoFuncionarioId)
        {
            var servicoFuncionario = await _servicoFuncionarioService.GetAsync(servicoFuncionarioId);

            return Ok(servicoFuncionario);
        }

        [HttpPost]
        [SwaggerOperation(
           Summary = "Creates a new  funcionarios in servicos",
           Description = "Creates a new  funcionarios in servicos if all validations are succeded"
       )]
        [SwaggerResponse(201, "The  funcionarios in servicos was successfully created", typeof(ServicoFuncionario))]
        public async Task<ActionResult> Post([FromBody] ServicoFuncionario servicoFuncionario)
        {
            var created = await _servicoFuncionarioService.CreateAsync(servicoFuncionario);

            return CreatedAtAction(nameof(Post), new { id = servicoFuncionario.ServicoFuncionarioId }, created);
        }
        [HttpPut("{servicoFuncionarioId}")]
        [SwaggerOperation(
           Summary = "Edits an existing funcionarios in servicos by their Id",
           Description = "Edits an existing funcionarios in servicos if all validations are succeded"
       )]
        [SwaggerResponse(200, "The funcionarios in servicos was successfully edited", typeof(ServicoFuncionario))]
        public async Task<ActionResult> Put(
           [SwaggerParameter("ServicoProduto Id")] int servicoFuncionarioId,
           [FromBody] ServicoFuncionario servicoFuncionario)
        {
            var edited = await _servicoFuncionarioService.UpdateAsync(servicoFuncionarioId, servicoFuncionario);

            return Ok(edited);
        }
        [HttpDelete("{servicoFuncionarioId}")]
        [SwaggerOperation(
            Summary = "Deletes a funcionarios in servicos by their Id",
            Description = "Deletes a funcionarios in servicos if that Cliente is deletable"
        )]
        [SwaggerResponse(204, "The funcionarios in servicos was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("servicoFuncionario Id")]int servicoFuncionarioId)
        {
            await _servicoFuncionarioService.DeleteAsync(servicoFuncionarioId);

            return NoContent();
        }
    }
}