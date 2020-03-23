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
    [Route("servicos/{servicoId}/produtos")]
    [SwaggerTag("Create, edit, delete and retrieve produtos in servicos")]
    public class ServicoProdutosController : ControllerBase
    {
        private readonly IServicoProdutoService _servicoProdutoService;

        public ServicoProdutosController(IServicoProdutoService servicoProdutoService)
        {
            _servicoProdutoService = servicoProdutoService;
        }

        [HttpGet]
        [SwaggerOperation(
          Summary = "Retrieve a paginated list of produtos in servicos",
          Description = "Retrieves only produtos in servicos"
      )]
        [SwaggerResponse(200, "List of produtos in servicos filtered by the informed parameters", typeof(Pagination<ServicoProduto>))]
        public async Task<ActionResult> List(
          [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
          [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit,
          [SwaggerParameter("Limit the number of items that are returned")][BindRequired][Required] int servicoId)
        {
            var pagination = await _servicoProdutoService.ListAsync(offset, limit,servicoId);

            return Ok(pagination);
        }
        [HttpGet("{servicoProdutoId}")]
        [SwaggerOperation(
            Summary = "Retrieve a produtos in servicos by their id",
            Description = "Retrieves produtos in servicos"
        )]
        [SwaggerResponse(200, "A produtos in servicos filtered", typeof(ServicoProduto))]
        public async Task<ActionResult> Get([SwaggerParameter("ServicoProduto Id")]int servicoProdutoId)
        {
            var servicoProduto = await _servicoProdutoService.GetAsync(servicoProdutoId);

            return Ok(servicoProduto);
        }
        [HttpPost]
        [SwaggerOperation(
           Summary = "Creates a new  produtos in servicos",
           Description = "Creates a new  produtos in servicos if all validations are succeded"
       )]
        [SwaggerResponse(201, "The  produtos in servicos was successfully created", typeof(ServicoProduto))]
        public async Task<ActionResult> Post([FromBody] ServicoProduto cliente)
        {
            var created = await _servicoProdutoService.CreateAsync(cliente);

            return CreatedAtAction(nameof(Post), new { id = cliente.ServicoProdutoId }, created);
        }
        [HttpPut("{servicoProdutoId}")]
        [SwaggerOperation(
            Summary = "Edits an existing produtos in servicos by their Id",
            Description = "Edits an existing produtos in servicos if all validations are succeded"
        )]
        [SwaggerResponse(200, "The produtos in servicos was successfully edited", typeof(ServicoProduto))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("ServicoProduto Id")] int servicoProdutoId,
            [FromBody] ServicoProduto servicoProduto)
        {
            var edited = await _servicoProdutoService.UpdateAsync(servicoProdutoId, servicoProduto);

            return Ok(edited);
        }
        [HttpDelete("{servicoProdutoId}")]
        [SwaggerOperation(
            Summary = "Deletes a produtos in servicos by their Id",
            Description = "Deletes a produtos in servicos if that Cliente is deletable"
        )]
        [SwaggerResponse(204, "The produtos in servicos was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("ServicoProduto Id")]int servicoProdutoId)
        {
            await _servicoProdutoService.DeleteAsync(servicoProdutoId);

            return NoContent();
        }

    }
}