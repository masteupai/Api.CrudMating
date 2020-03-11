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
    [Route("veiculos")]
    [SwaggerTag("Create, edit, delete and retrieve Veiculos")]
    public class VeiculosController : ControllerBase
    {
        private readonly IVeiculoService _veiculosService;

        public VeiculosController(IVeiculoService veiculosService)
        {
            _veiculosService = veiculosService;
        }



        [HttpGet]
        [SwaggerOperation(
                  Summary = "Retrieve a paginated list of Veiculos",
                  Description = "Retrieves only products that were created by the authenticated Veiculo"
              )]
        [SwaggerResponse(200, "List of Veiculos filtered by the informed parameters", typeof(Pagination<Veiculo>))]
        public async Task<ActionResult> List(
                  [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
                  [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit)
        {
            var pagination = await _veiculosService.ListAsync(offset, limit);

            return Ok(pagination);
        }

        [HttpGet("{veiculoId}")]
        [SwaggerOperation(
            Summary = "Retrieve a Veiculo by their id",
            Description = "Retrieves Veiculo only if it were created by the authenticated Veiculo"
        )]
        [SwaggerResponse(200, "A Veiculo filtered", typeof(Veiculo))]
        public async Task<ActionResult> Get([SwaggerParameter("Veiculo's id")]int veiculoId)
        {
            var veiculo = await _veiculosService.GetAsync(veiculoId);

            return Ok(veiculo);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Veiculo",
            Description = "Creates a new veiculo if all validations are succeded"
        )]
        [SwaggerResponse(201, "The veiculo was successfully created", typeof(Veiculo))]
        public async Task<ActionResult> Post([FromBody] Veiculo veiculo)
        {
            var created = await _veiculosService.CreateAsync(veiculo);

            return CreatedAtAction(nameof(Post), new { id = veiculo.VeiculoId }, created);
        }

        [HttpPut("{veiculoId}")]
        [SwaggerOperation(
            Summary = "Edits an existing Veiculo by their Id",
            Description = "Edits an existing Veiculo if all validations are succeded"
        )]
        [SwaggerResponse(200, "The Veiculo was successfully edited", typeof(Veiculo))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("veiculo's Id")] int veiculoId,
            [FromBody] Veiculo veiculo)
        {
            var edited = await _veiculosService.UpdateAsync(veiculoId, veiculo);

            return Ok(edited);
        }

        [HttpDelete("{veiculoId}")]
        [SwaggerOperation(
            Summary = "Deletes a veiculo by their Id",
            Description = "Deletes a veiculo if that veiculo is deletable"
        )]
        [SwaggerResponse(204, "The veiculo was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("veiculo's Id")]int veiculoId)
        {
            await _veiculosService.DeleteAsync(veiculoId);

            return NoContent();
        }
    }
}
