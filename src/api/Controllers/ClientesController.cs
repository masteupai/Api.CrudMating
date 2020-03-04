using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Domains.Models;
using API.Domains.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("clientes")]
    [SwaggerTag("Create, edit, delete and retrieve clientes")]
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpGet]
        [SwaggerOperation(
          Summary = "Retrieve a paginated list of Funcionarios",
          Description = "Retrieves only products that were created by the authenticated Cliente"
      )]
        [SwaggerResponse(200, "List of Funcionarios filtered by the informed parameters", typeof(Pagination<Cliente>))]
        public async Task<ActionResult> List(
          [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
          [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit)
        {
            var pagination = await _clienteService.ListAsync(offset, limit);

            return Ok(pagination);
        }

        [HttpGet("{clienteId}")]
        [SwaggerOperation(
            Summary = "Retrieve a Cliente by their id",
            Description = "Retrieves Cliente only if it were created by the authenticated Cliente"
        )]
        [SwaggerResponse(200, "A Cliente filtered", typeof(Cliente))]
        public async Task<ActionResult> Get([SwaggerParameter("Cliente's id")]int clienteId)
        {
            var cliente = await _clienteService.GetAsync(clienteId);

            return Ok(cliente);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Cliente",
            Description = "Creates a new product if all validations are succeded"
        )]
        [SwaggerResponse(201, "The product was successfully created", typeof(Cliente))]
        public async Task<ActionResult> Post([FromBody] Cliente cliente)
        {
            var created = await _clienteService.CreateAsync(cliente);

            return CreatedAtAction(nameof(Post), new { id = cliente.ClienteId }, created);
        }

        [HttpPut("{clienteId}")]
        [SwaggerOperation(
            Summary = "Edits an existing Cliente by their Id",
            Description = "Edits an existing Cliente if all validations are succeded"
        )]
        [SwaggerResponse(200, "The Cliente was successfully edited", typeof(Cliente))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("Cliente's Id")] int clienteId,
            [FromBody] Cliente cliente)
        {
            var edited = await _clienteService.UpdateAsync(clienteId, cliente);

            return Ok(edited);
        }

        [HttpDelete("{clienteId}")]
        [SwaggerOperation(
            Summary = "Deletes a Cliente by their Id",
            Description = "Deletes a Cliente if that Cliente is deletable"
        )]
        [SwaggerResponse(204, "The Cliente was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("Cliente's Id")]int clienteId)
        {
            await _clienteService.DeleteAsync(clienteId);

            return NoContent();
        }




    }
}