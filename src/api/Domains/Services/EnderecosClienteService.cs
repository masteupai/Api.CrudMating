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
    public interface IEnderecosClienteService
    {
        Task<Pagination<EnderecoCliente>> ListAsync(int offset, int limit, int clienteId);
        Task<EnderecoCliente> GetAsync(int id);
        Task<EnderecoCliente> CreateAsync(EnderecoCliente enderecosCliente);
        Task<EnderecoCliente> UpdateAsync(int id, EnderecoCliente enderecosCliente);
        Task DeleteAsync(int id);
    }
    public class EnderecosClienteService : IEnderecosClienteService
    {
        private readonly IValidator<EnderecoCliente> _enderecoClienteValidator;
        private readonly ISqlService _sqlService;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<EnderecosClienteService> _logger;
        public EnderecosClienteService(
            IValidator<EnderecoCliente> enderecoClienteValidator,
            ISqlService sqlService,
            IValidationService validationService,
            IAuthenticatedService authenticatedService,
            ILogger<EnderecosClienteService> logger)
        {
            _enderecoClienteValidator = enderecoClienteValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;
        }

        public async Task<EnderecoCliente> CreateAsync(EnderecoCliente enderecosCliente)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existCep = await _sqlService.ExistsAsync(EnderecosCliQuery.EXIST_ENDERECO_CEP, new
            {
                CEP = enderecosCliente.CEP,
                CLIENTEID = enderecosCliente.ClienteId
            });          

            if (existCep)
            {
                this._logger.LogDebug("Endereco already exists, triggering 400");

                this._validationService.Throw("Endereco", "There is already another Endereco with that Cep", enderecosCliente.CEP, Validation.EnderecoExist);
            }

            this._logger.LogDebug("Inserting new Endereco");

            enderecosCliente.EnderecoCliId = await _sqlService.CreateAsync(EnderecosCliQuery.INSERT, new
            {
                CLIENTEID = enderecosCliente.ClienteId,
                LOGRADOURO = enderecosCliente.Logradouro,
                BAIRRO = enderecosCliente.Bairro,
                CIDADE = enderecosCliente.Cidade,
                UF = enderecosCliente.Uf,
                CEP = enderecosCliente.CEP
            });

            this._logger.LogDebug("Ending CreateAsync");

            return enderecosCliente;
        }

        public async Task DeleteAsync(int id)
        {
            this._logger.LogDebug("Starting DeleteAsync");

            this._logger.LogDebug("Retriving the endereco wants to delete");

            var endereco = await GetAsync(id);

            var temEndereco = await _sqlService.ExistsAsync(EnderecosCliQuery.EXIST_ENDERECO_ID, new { ID = endereco.EnderecoCliId });

            if (!temEndereco)
            {
                this._logger.LogDebug("Endereco already exists, triggering 400");

                this._validationService.Throw("Endereco", "There is already another endereco with that Id", endereco.ClienteId, Validation.EnderecoNotExists);
            }


            this._logger.LogDebug("Deleting endereco");

            await _sqlService.ExecuteAsync(EnderecosCliQuery.DELETE, new
            {
                Id = endereco.EnderecoCliId
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }

        public async Task<EnderecoCliente> GetAsync(int id)
        {
            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a Endereco");


            var endereco = await _sqlService.GetAsync<EnderecoCliente>(EnderecosCliQuery.GET, new
            {
                Id = id
            });

            this._logger.LogDebug("Checking if endereco exists");

            if (endereco == null)
            {
                this._logger.LogDebug("endereco does not exists, triggering 404");

                throw new ArgumentNullException(nameof(id));
            }

            this._logger.LogDebug("Ending GetAsync");

            return endereco;
        }

        public async Task<Pagination<EnderecoCliente>> ListAsync(int offset, int limit, int clienteId)
        {

            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of enderecos");

            var list = await _sqlService.ListAsync<EnderecoCliente>(EnderecosCliQuery.PAGINATE, new
            {
                Offset = offset,
                Limit = limit,
                ClienteId = clienteId
            });

            this._logger.LogDebug("Retriving the number of enderecos registered");

            var total = await _sqlService.CountAsync(EnderecosCliQuery.TOTAL, new { CLIENTEID = clienteId });

            this._logger.LogDebug("Retriving the number of enderecos registered");

            var pagination = new Pagination<EnderecoCliente>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }

        public async Task<EnderecoCliente> UpdateAsync(int id, EnderecoCliente enderecoCliente)
        {
            this._logger.LogDebug("Starting UpdateAsync");

            var oldEndereco = await GetAsync(id);

            var existCep = await _sqlService.ExistsAsync(EnderecosCliQuery.EXIST_ENDERECO_CEP, new
            {
                CEP = enderecoCliente.CEP,
                CLIENTEID = enderecoCliente.ClienteId
            });

            this._logger.LogDebug("Checking if that endereco already exists");

            if (existCep)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another funcionario with that Email or Telefone", enderecoCliente.CEP, Validation.EnderecoNotExists);
            }    
          
            this._logger.LogDebug("Updating contato");

            await _sqlService.ExecuteAsync(EnderecosCliQuery.UPDATE, new
            {
                Id = id,
                CLIENTEID = enderecoCliente.ClienteId,
                CEP = enderecoCliente.CEP,
                LOGRADOURO = enderecoCliente.Logradouro,
                BAIRRO = enderecoCliente.Bairro,
                UF = enderecoCliente.Uf,
                CIDADE = enderecoCliente.Cidade
            });

            enderecoCliente.EnderecoCliId = oldEndereco.EnderecoCliId;

            this._logger.LogDebug("Ending UpdateAsync");

            return enderecoCliente;
        }
    }
}
