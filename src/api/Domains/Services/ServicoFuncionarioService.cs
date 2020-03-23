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
    public interface IServicoFuncionarioService
    {
        Task<Pagination<ServicoFuncionario>> ListAsync(int offset, int limit, int servicoId);
        Task<ServicoFuncionario> GetAsync(int id);
        Task<ServicoFuncionario> CreateAsync(ServicoFuncionario funcionario);
        Task<ServicoFuncionario> UpdateAsync(int id, ServicoFuncionario funcionario);
        Task DeleteAsync(int id);
    }
    public class ServicoFuncionarioService : IServicoFuncionarioService
    {
        private readonly IValidator<ServicoFuncionario> _servicoFuncionarioValidator;
        private readonly ISqlService _sqlService;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<ServicoFuncionarioService> _logger;

        public ServicoFuncionarioService(
            IValidator<ServicoFuncionario> servicoFuncionarioValidator,
            ISqlService sqlService,
            IValidationService validationService,
            IAuthenticatedService authenticatedService,
            ILogger<ServicoFuncionarioService> logger)
        {
            _servicoFuncionarioValidator = servicoFuncionarioValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;

        }

        public async Task<ServicoFuncionario> CreateAsync(ServicoFuncionario funcionario)
        {

            this._logger.LogDebug("Starting CreateAsync");

            var existFuncionarioServico = await _sqlService.ExistsAsync(ServicoFuncionarioQuery.EXIST_SERVICO_FUNCIONARIOID, new
            {
                Id = funcionario.FuncionarioId
            });

            if (existFuncionarioServico)
            {
                this._logger.LogDebug("Funcionario already exists in servico, triggering 400");

                this._validationService.Throw("Funcionario", "There is already another funcionario in servico with that id", funcionario.FuncionarioId, Validation.FuncionarioExists);
            }

            this._logger.LogDebug("Inserting new Funcionario");

            funcionario.FuncionarioId = await _sqlService.CreateAsync(ServicoFuncionarioQuery.INSERT, new
            {
                FUNCIONARIOID = funcionario.FuncionarioId,
                SERVICOID = funcionario.ServicoId,
                COMISSAO = funcionario.Comissao
            });

            this._logger.LogDebug("Ending CreateAsync");

            return funcionario;
        }

        public async Task DeleteAsync(int id)
        {
            this._logger.LogDebug("Starting DeleteAsync");

            this._logger.LogDebug("Retriving the product wants to delete");

            var funcionarioServico = await GetAsync(id);

            var temFuncionarioServico = await _sqlService.ExistsAsync(ServicoFuncionarioQuery.EXIST_SERVICO_ID, new { Id = funcionarioServico.ServicoFuncionarioId });

            if (!temFuncionarioServico)
            {
                this._logger.LogDebug("Funcionario already not exists in servico, triggering 400");

                this._validationService.Throw("Funcionario", "There is already another funcionario in servico with that Id", funcionarioServico.ServicoFuncionarioId, Validation.FuncionarioNotExists);
            }

            this._logger.LogDebug("Deleting funcionario in servico");

            await _sqlService.ExecuteAsync(ServicoFuncionarioQuery.DELETE, new
            {
                FUNCIONARIOID = funcionarioServico.ServicoFuncionarioId
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }

        public async Task<ServicoFuncionario> GetAsync(int id)
        {
            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a Funcionario in servico");


            var funcionarioServico = await _sqlService.GetAsync<ServicoFuncionario>(ServicoFuncionarioQuery.GET, new
            {
                Id = id
            });

            this._logger.LogDebug("Checking if funcionario exists in servico");

            if (funcionarioServico == null)
            {
                this._logger.LogDebug("funcionario does not exists in servico, triggering 404");

                throw new ArgumentNullException(nameof(id));
            }

            this._logger.LogDebug("Ending GetAsync");

            return funcionarioServico;
        }

        public async Task<Pagination<ServicoFuncionario>> ListAsync(int offset, int limit, int servicoId)
        {
            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of products");

            var list = await _sqlService.ListAsync<ServicoFuncionario>(ServicoFuncionarioQuery.PAGINATE, new
            {
                Offset = offset,
                Limit = limit,
                SERVICOID = servicoId
            });

            this._logger.LogDebug("Retriving the number of funcionarios registered by the authenticated funcionario");

            var total = await _sqlService.CountAsync(ServicoFuncionarioQuery.TOTAL, new { SERVICOID = servicoId});

            this._logger.LogDebug("Retriving the number of funcionarios registered by the authenticated funcionario");

            var pagination = new Pagination<ServicoFuncionario>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }

        public async Task<ServicoFuncionario> UpdateAsync(int id, ServicoFuncionario funcionario)
        {
            this._logger.LogDebug("Starting UpdateAsync");

            var oldFuncServico = await GetAsync(id);

            var existFuncionarioServico = await _sqlService.ExistsAsync(ServicoFuncionarioQuery.EXIST_SERVICO_ID, new
            {
                Id = oldFuncServico.ServicoFuncionarioId
            });

            this._logger.LogDebug("Checking if that product already exists");

            if (!existFuncionarioServico)
            {
                this._logger.LogDebug("Funcionario already exists in servico, triggering 400");

                this._validationService.Throw("funcionario", "There is already another funcionario in servico with that id", funcionario.FuncionarioId, Validation.FuncionarioNotExists);
            }

            this._logger.LogDebug("Updating product");

            await _sqlService.ExecuteAsync(ServicoFuncionarioQuery.UPDATE, new
            {
                Id = id,
                SERVICOID = funcionario.ServicoId,
                FUNCIONARIOID = funcionario.FuncionarioId,
                COMISSAO = funcionario.Comissao
            });

            funcionario.ServicoFuncionarioId = oldFuncServico.ServicoFuncionarioId;

            this._logger.LogDebug("Ending UpdateAsync");

            return funcionario;
        }
    }
}
