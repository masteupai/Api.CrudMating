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
    [Route("servicos")]
    [SwaggerTag("Create, edit, delete and retrieve Veiculos")]

    public class ServicosController : ControllerBase
    {
        private readonly IServicoService _servicoService;

        public ServicosController(IServicoService servicoService)
        {
            _servicoService = servicoService;
        }



        [HttpGet]
        [SwaggerOperation(
                  Summary = "Retrieve a paginated list of Veiculos",
                  Description = "Retrieves only products that were created by the authenticated servico"
              )]
        [SwaggerResponse(200, "List of Veiculos filtered by the informed parameters", typeof(Pagination<Servico>))]
        public async Task<ActionResult> List(
                  [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
                  [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit)
        {
            var pagination = await _servicoService.ListAsync(offset, limit);

            return Ok(pagination);
        }

        [HttpGet("{veiculoId}")]
        [SwaggerOperation(
            Summary = "Retrieve a servico by their id",
            Description = "Retrieves servico only if it were created by the authenticated servico"
        )]
        [SwaggerResponse(200, "A servico filtered", typeof(Servico))]
        public async Task<ActionResult> Get([SwaggerParameter("servico's id")]int veiculoId)
        {
            var servico = await _servicoService.GetAsync(veiculoId);

            return Ok(servico);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new servico",
            Description = "Creates a new servico if all validations are succeded"
        )]
        [SwaggerResponse(201, "The servico was successfully created", typeof(Servico))]
        public async Task<ActionResult> Post([FromBody] Servico servico)
        {
            var created = await _servicoService.CreateAsync(servico);

            return CreatedAtAction(nameof(Post), new { id = servico.VeiculoId }, created);
        }

        [HttpPut("{veiculoId}")]
        [SwaggerOperation(
            Summary = "Edits an existing Veiculo by their Id",
            Description = "Edits an existing Veiculo if all validations are succeded"
        )]
        [SwaggerResponse(200, "The Veiculo was successfully edited", typeof(Servico))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("servico's Id")] int veiculoId,
            [FromBody] Servico servico)
        {
            var edited = await _servicoService.UpdateAsync(veiculoId, servico);

            return Ok(edited);
        }

        [HttpDelete("{veiculoId}")]
        [SwaggerOperation(
            Summary = "Deletes a servico by their Id",
            Description = "Deletes a servico if that servico is deletable"
        )]
        [SwaggerResponse(204, "The servico was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("servico's Id")]int veiculoId)
        {
            await _servicoService.DeleteAsync(veiculoId);

            return NoContent();
        }

    }
}