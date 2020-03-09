﻿using API.Domains.Models;
using API.Domains.Queries;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Services
{
    public interface IContatoFuncionarioService
    {
        Task<Pagination<ContatoFuncionario>> ListAsync(int offset, int limit);
        Task<ContatoFuncionario> GetAsync(int id);
        Task<ContatoFuncionario> CreateAsync(ContatoFuncionario contatoCliente);
        Task<ContatoFuncionario> UpdateAsync(int id, ContatoFuncionario contatoCliente);
        Task DeleteAsync(int id);
    }
    public class ContatoFuncionarioService : IContatoFuncionarioService
    {
        private readonly IValidator<ContatoFuncionario> _contatoFuncionarioValidator;
        private readonly ISqlService _sqlService;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<ContatoFuncionarioService> _logger;
        public ContatoFuncionarioService(
            IValidator<ContatoFuncionario> contatoFuncionarioValidator,
            ISqlService sqlService,
            IValidationService validationService,
            IAuthenticatedService authenticatedService,
            ILogger<ContatoFuncionarioService> logger)
        {
            _contatoFuncionarioValidator = contatoFuncionarioValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;

        }
        public async Task<ContatoFuncionario> CreateAsync(ContatoFuncionario contatoCliente)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existContatoEmail = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOCLIENTE_EMAIL, new
            {
                EMAIL = contatoCliente.Email
            });
            var existContatoTelefone = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOCLIENTE_TELEFONE, new
            {
                TELEFONE = contatoCliente.Email
            });

            if (existContatoEmail && existContatoTelefone)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another funcionario with that Email or Telefone", contatoCliente, Validation.ContatoExists);
            }

            this._logger.LogDebug("Inserting new Contato");

            contatoCliente.ContatoCliId = await _sqlService.CreateAsync(ContatoFuncionarioQuery.INSERT, new
            {
                CLIENTEID = contatoCliente.ClienteId,
                EMAIL = contatoCliente.Email,
                TELEFONE = contatoCliente.Telefone
            });

            this._logger.LogDebug("Ending CreateAsync");

            return contatoCliente;
        }


        public async Task<ContatoFuncionario> UpdateAsync(int id, ContatoFuncionario contatoCliente)
        {

            this._logger.LogDebug("Starting UpdateAsync");

            var oldContato = await GetAsync(id);

            var existContatoEmail = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOCLIENTE_EMAIL, new
            {
                EMAIL = contatoCliente.Email
            });
            var existContatoTelefone = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOCLIENTE_TELEFONE, new
            {
                TELEFONE = contatoCliente.Telefone
            });

            if (existContatoEmail && existContatoTelefone)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another funcionario with that Email or Telefone", contatoCliente, Validation.ContatoExists);
            }

            var existContato = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOCLIENTE_ID, new
            {
                Id = oldContato.ContatoCliId
            });

            this._logger.LogDebug("Checking if that contato already exists");

            if (!existContato)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another user with that id", contatoCliente.ContatoCliId, Validation.ContatoNotExists);
            }

            this._logger.LogDebug("Updating contato");

            await _sqlService.ExecuteAsync(ContatoFuncionarioQuery.UPDATE, new
            {
                Id = id,
                CLIENTEID = contatoCliente.ClienteId,
                EMAIL = contatoCliente.Email,
                TELEFONE = contatoCliente.Telefone
            });

            contatoCliente.ContatoCliId = oldContato.ContatoCliId;

            this._logger.LogDebug("Ending UpdateAsync");

            return contatoCliente;
        }

        public async Task<ContatoFuncionario> GetAsync(int id)
        {

            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a Funcionario");


            var contato = await _sqlService.GetAsync<ContatoFuncionario>(ContatoFuncionarioQuery.GET, new
            {
                Id = id
            });

            this._logger.LogDebug("Checking if product exists");

            if (contato == null)
            {
                this._logger.LogDebug("Contato does not exists, triggering 404");

                throw new ArgumentNullException(nameof(id));
            }

            this._logger.LogDebug("Ending GetAsync");

            return contato;
        }

        public async Task<Pagination<ContatoFuncionario>> ListAsync(int offset, int limit)
        {

            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of contatos");

            var list = await _sqlService.ListAsync<ContatoFuncionario>(ContatoFuncionarioQuery.PAGINATE, new
            {
                Offset = offset,
                Limit = limit
            });

            this._logger.LogDebug("Retriving the number of contatos registered");

            var total = await _sqlService.CountAsync(ContatoFuncionarioQuery.TOTAL, new { });

            this._logger.LogDebug("Retriving the number of contatos registered");

            var pagination = new Pagination<ContatoFuncionario>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }
        public async Task DeleteAsync(int id)
        {
            this._logger.LogDebug("Starting DeleteAsync");

            this._logger.LogDebug("Retriving the product wants to delete");

            var clienteContato = await GetAsync(id);

            var temContato = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOCLIENTE_ID, new { ID = clienteContato.ContatoCliId });

            if (!temContato)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another Contato with that Id", clienteContato.ClienteId, Validation.ClienteNotExists);
            }


            this._logger.LogDebug("Deleting contato");

            await _sqlService.ExecuteAsync(ContatoFuncionarioQuery.DELETE, new
            {
                Id = clienteContato.
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }
    }
}
