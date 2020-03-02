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
    [Route("funcionarios")]
    [SwaggerTag("Create, edit, delete and retrieve funcionarios")]
    public class FuncionariosController : ControllerBase
    {

        private readonly IFuncionarioService _funcionarioService;

        public FuncionariosController(IFuncionarioService funcionarioService)
        {
            _funcionarioService = funcionarioService;
        }

        [HttpGet]
        [SwaggerOperation(
           Summary = "Retrieve a paginated list of Funcionarios",
           Description = "Retrieves only products that were created by the authenticated funcionario"
       )]
        [SwaggerResponse(200, "List of Funcionarios filtered by the informed parameters", typeof(Pagination<Funcionario>))]
        public async Task<ActionResult> List(
           [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
           [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit)
        {
            var pagination = await _funcionarioService.ListAsync(offset, limit);

            return Ok(pagination);
        }

        [HttpGet("{funcionarioId}")]
        [SwaggerOperation(
            Summary = "Retrieve a Funcionario by their id",
            Description = "Retrieves Funcionario only if it were created by the authenticated Funcionario"
        )]
        [SwaggerResponse(200, "A funcionario filtered", typeof(Funcionario))]
        public async Task<ActionResult> Get([SwaggerParameter("funcionario's id")]int funcionarioId)
        {
            var funcionario = await _funcionarioService.GetAsync(funcionarioId);

            return Ok(funcionario);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Funcionario",
            Description = "Creates a new product if all validations are succeded"
        )]
        [SwaggerResponse(201, "The product was successfully created", typeof(Funcionario))]
        public async Task<ActionResult> Post([FromBody] Funcionario funcionario)
        {
            var created = await _funcionarioService.CreateAsync(funcionario);

            return CreatedAtAction(nameof(Post), new { id = funcionario.FuncionarioId }, created);
        }

        [HttpPut("{funcionarioId}")]
        [SwaggerOperation(
            Summary = "Edits an existing Funcionario by their Id",
            Description = "Edits an existing Funcionario if all validations are succeded"
        )]
        [SwaggerResponse(200, "The Funcionario was successfully edited", typeof(Funcionario))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("Funcionario's Id")] int funcionarioId,
            [FromBody] Funcionario funcionario)
        {
            var edited = await _funcionarioService.UpdateAsync(funcionarioId, funcionario);

            return Ok(edited);
        }

        [HttpDelete("{funcionarioId}")]
        [SwaggerOperation(
            Summary = "Deletes a Funcionario by their Id",
            Description = "Deletes a Funcionario if that Funcionario is deletable"
        )]
        [SwaggerResponse(204, "The Funcionario was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("funcionario's Id")]int funcionarioId)
        {
            await _funcionarioService.DeleteAsync(funcionarioId);

            return NoContent();
        }

    }
}