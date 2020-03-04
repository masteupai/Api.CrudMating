using API.Domains.Models;
using API.Domains.Models.Faults;
using API.Domains.Queries;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Services
{
    public interface IClienteService
    {
        Task<Pagination<Cliente>> ListAsync(int offset, int limit);
        Task<Cliente> GetAsync(int id);
        Task<Cliente> CreateAsync(Cliente cliente);
        Task<Cliente> UpdateAsync(int id, Cliente cliente);
        Task DeleteAsync(int id);
    }


    public class ClienteService : IClienteService
    {

        private readonly IValidator<Cliente> _clienteValidator;
        private readonly ISqlService _sqlService;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(
            IValidator<Cliente> clienteValidator,
            ISqlService sqlService,
            IValidationService validationService,
            IAuthenticatedService authenticatedService,
            ILogger<ClienteService> logger)
        {
            _clienteValidator = clienteValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;

        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existCliente = await _sqlService.ExistsAsync(ClienteQuery.EXIST_CLIENTE, new
            {
                Cpf = cliente.Documento
            });

            if (existCliente)
            {
                this._logger.LogDebug("Cliente already exists, triggering 400");

                this._validationService.Throw("Cliente", "There is already another cliente with that Cpf", cliente.Documento, Validation.ClienteExists);
            }

            this._logger.LogDebug("Inserting new Funcionario");

            cliente.ClienteId = await _sqlService.CreateAsync(ClienteQuery.INSERT, new
            {
                Nome = cliente.Nome,
                Documento = cliente.Documento,
                DataNascimento = cliente.DataNascimento,
                Ativo = cliente.Ativo
            });

            this._logger.LogDebug("Ending CreateAsync");

            return cliente;
        }

        public async Task DeleteAsync(int id)
        {
            this._logger.LogDebug("Starting DeleteAsync");

            this._logger.LogDebug("Retriving the product wants to delete");

            var cliente = await GetAsync(id);

            var temCliente = await _sqlService.ExistsAsync(ClienteQuery.EXIST_CLIENTE_ID, new { Id = cliente.ClienteId });

            if (!temCliente)
            {
                this._logger.LogDebug("Cliente already exists, triggering 400");

                this._validationService.Throw("Cliente", "There is already another cliente with that Id", cliente.ClienteId, Validation.ClienteNotExists);
            }


            this._logger.LogDebug("Deleting product");

            await _sqlService.ExecuteAsync(ClienteQuery.DELETE, new
            {
                Id = cliente.ClienteId
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }

        public async Task<Cliente> GetAsync(int id)
        {
            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a Funcionario");


            var cliente = await _sqlService.GetAsync<Cliente>(ClienteQuery.GET, new
            {
                Id = id
            });

            this._logger.LogDebug("Checking if product exists");

            if (cliente == null)
            {
                this._logger.LogDebug("Cliente does not exists, triggering 404");

                throw new ArgumentNullException(nameof(id));
            }

            this._logger.LogDebug("Ending GetAsync");

            return cliente;
        }

        public async Task<Pagination<Cliente>> ListAsync(int offset, int limit)
        {
            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of products");

            var list = await _sqlService.ListAsync<Cliente>(ClienteQuery.PAGINATE, new
            {
                Offset = offset,
                Limit = limit
            });

            this._logger.LogDebug("Retriving the number of clientes registered by the authenticated funcionario");

            var total = await _sqlService.CountAsync(ClienteQuery.TOTAL, new { });

            this._logger.LogDebug("Retriving the number of clientes registered by the authenticated funcionario");

            var pagination = new Pagination<Cliente>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }

        public async Task<Cliente> UpdateAsync(int id, Cliente cliente)
        {
            this._logger.LogDebug("Starting UpdateAsync");

            var oldCliente = await GetAsync(id);

            var existCliente = await _sqlService.ExistsAsync(ClienteQuery.EXIST_CLIENTE_ID, new
            {
                Id = oldCliente.ClienteId
            });

            this._logger.LogDebug("Checking if that product already exists");

            if (!existCliente)
            {
                this._logger.LogDebug("Cliente already exists, triggering 400");

                this._validationService.Throw("Cliente", "There isn't already another Cliente with that Id", cliente.ClienteId, Validation.FuncionarioNotExists);
            }

            this._logger.LogDebug("Updating product");

            await _sqlService.ExecuteAsync(ClienteQuery.UPDATE, new
            {
                Id = id,
                Nome = cliente.Nome,
                DataNascimento = cliente.DataNascimento,
                Documento = cliente.Documento,
                Ativo = cliente.Ativo
            });

            cliente.ClienteId = oldCliente.ClienteId;

            this._logger.LogDebug("Ending UpdateAsync");

            return cliente;
        }
    }
}
