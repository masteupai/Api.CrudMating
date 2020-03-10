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
    public interface IEnderecosFuncionarioService
    {
        Task<Pagination<EnderecoFuncionario>> ListAsync(int offset, int limit, int clienteId);
        Task<EnderecoFuncionario> GetAsync(int id);
        Task<EnderecoFuncionario> CreateAsync(EnderecoFuncionario enderecosFuncionario);
        Task<EnderecoFuncionario> UpdateAsync(int id, EnderecoFuncionario enderecosFuncionario);
        Task DeleteAsync(int id);
    }
    public class EnderecosFuncionarioService : IEnderecosFuncionarioService
    {
        private readonly IValidator<EnderecoFuncionario> _enderecoFuncionarioValidator;
        private readonly ISqlService _sqlService;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<EnderecosFuncionarioService> _logger;
        public EnderecosFuncionarioService(
            IValidator<EnderecoFuncionario> enderecoFuncionarioValidator,
            ISqlService sqlService,
            IValidationService validationService,
            IAuthenticatedService authenticatedService,
            ILogger<EnderecosFuncionarioService> logger)
        {
            _enderecoFuncionarioValidator = enderecoFuncionarioValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;
        }


        public async Task<EnderecoFuncionario> CreateAsync(EnderecoFuncionario enderecosCliente)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existCep = await _sqlService.ExistsAsync(EnderecosFunQuery.EXIST_ENDERECO_CEP, new
            {
                CEP = enderecosCliente.CEP,
                FUNCIONARIOID = enderecosCliente.FuncionarioId
            });

            if (existCep)
            {
                this._logger.LogDebug("Endereco already exists, triggering 400");

                this._validationService.Throw("Endereco", "There is already another Endereco with that Cep", enderecosCliente.CEP, Validation.EnderecoExist);
            }

            this._logger.LogDebug("Inserting new Endereco");

            enderecosCliente.EnderecoFunId = await _sqlService.CreateAsync(EnderecosFunQuery.INSERT, new
            {
                FUNCIONARIOID = enderecosCliente.FuncionarioId,
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

            var temEndereco = await _sqlService.ExistsAsync(EnderecosFunQuery.EXIST_ENDERECO_ID, new { ID = endereco.EnderecoFunId });

            if (!temEndereco)
            {
                this._logger.LogDebug("Endereco already exists, triggering 400");

                this._validationService.Throw("Endereco", "There is already another endereco with that Id", endereco.FuncionarioId, Validation.EnderecoNotExists);
            }


            this._logger.LogDebug("Deleting endereco");

            await _sqlService.ExecuteAsync(EnderecosFunQuery.DELETE, new
            {
                Id = endereco.EnderecoFunId
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }

        public async Task<EnderecoFuncionario> GetAsync(int id)
        {
            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a Endereco");


            var endereco = await _sqlService.GetAsync<EnderecoFuncionario>(EnderecosFunQuery.GET, new
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

        public async Task<Pagination<EnderecoFuncionario>> ListAsync(int offset, int limit, int funcionarioId)
        {

            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of enderecos");

            var list = await _sqlService.ListAsync<EnderecoFuncionario>(EnderecosFunQuery.PAGINATE, new
            {
                Offset = offset,
                Limit = limit,
                FUNCIONARIOID = funcionarioId
            });

            this._logger.LogDebug("Retriving the number of enderecos registered");

            var total = await _sqlService.CountAsync(EnderecosFunQuery.TOTAL, new { FUNCIONARIOID = funcionarioId });

            this._logger.LogDebug("Retriving the number of enderecos registered");

            var pagination = new Pagination<EnderecoFuncionario>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }

        public async Task<EnderecoFuncionario> UpdateAsync(int id, EnderecoFuncionario enderecoFuncionario)
        {
            this._logger.LogDebug("Starting UpdateAsync");

            var oldEndereco = await GetAsync(id);

            var existCep = await _sqlService.ExistsAsync(EnderecosFunQuery.EXIST_ENDERECO_CEP, new
            {
                CEP = enderecoFuncionario.CEP,
                FUNCIONARIOID = enderecoFuncionario.FuncionarioId
            });

            this._logger.LogDebug("Checking if that endereco already exists");

            if (existCep)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another funcionario with that Email or Telefone", enderecoFuncionario.CEP, Validation.EnderecoNotExists);
            }

            this._logger.LogDebug("Updating contato");

            await _sqlService.ExecuteAsync(EnderecosFunQuery.UPDATE, new
            {
                Id = id,
                FUNCIONARIOID = enderecoFuncionario.FuncionarioId,
                CEP = enderecoFuncionario.CEP,
                LOGRADOURO = enderecoFuncionario.Logradouro,
                BAIRRO = enderecoFuncionario.Bairro,
                UF = enderecoFuncionario.Uf,
                CIDADE = enderecoFuncionario.Cidade
            });

            enderecoFuncionario.FuncionarioId = oldEndereco.FuncionarioId;

            this._logger.LogDebug("Ending UpdateAsync");

            return enderecoFuncionario;
        }
    }
}
