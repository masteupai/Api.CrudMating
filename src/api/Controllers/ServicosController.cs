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
    [SwaggerTag("Create, edit, delete and retrieve servicos")]

    public class ServicosController : ControllerBase
    {
        private readonly IServicoService _servicoService;

        public ServicosController(IServicoService servicoService)
        {
            _servicoService = servicoService;
        }



        [HttpGet]
        [SwaggerOperation(
                  Summary = "Retrieve a paginated list of Servicos",
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


        [HttpGet("/servicos/veiculo/{veiculoId}")]
        [SwaggerOperation(
               Summary = "Retrieve a paginated list of Servicos per Veiculo",
               Description = "Retrieves only servicos per veiculo"
           )]
        [SwaggerResponse(200, "List of Veiculos filtered by the informed parameters", typeof(Pagination<Servico>))]
        public async Task<ActionResult> ListPerVeiculo(
               [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
               [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit,
               [SwaggerParameter("Veiculo's Id")][BindRequired] int veiculoId)
        {
            var pagination = await _servicoService.ListPerVeiculoAsync(offset, limit, veiculoId);

            return Ok(pagination);
        }
        [HttpGet("{servicoId}")]
        [SwaggerOperation(
            Summary = "Retrieve a servico by their id",
            Description = "Retrieves servico only if it were created by the authenticated servico"
        )]
        [SwaggerResponse(200, "A servico filtered", typeof(Servico))]
        public async Task<ActionResult> Get([SwaggerParameter("servico's id")]int servicoId)
        {
            var servico = await _servicoService.GetAsync(servicoId);

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

            return CreatedAtAction(nameof(Post), new { id = servico.ServicoId }, created);
        }

        [HttpPut("{servicoId}")]
        [SwaggerOperation(
            Summary = "Edits an existing Veiculo by their Id",
            Description = "Edits an existing Veiculo if all validations are succeded"
        )]
        [SwaggerResponse(200, "The Veiculo was successfully edited", typeof(Servico))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("servico's Id")] int servicoId,
            [FromBody] Servico servico)
        {
            var edited = await _servicoService.UpdateAsync(servicoId, servico);

            return Ok(edited);
        }

        [HttpDelete("{servicoId}")]
        [SwaggerOperation(
            Summary = "Deletes a servico by their Id",
            Description = "Deletes a servico if that servico is deletable"
        )]
        [SwaggerResponse(204, "The servico was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("servico's Id")]int servicoId)
        {
            await _servicoService.DeleteAsync(servicoId);

            return NoContent();
        }

    }
}