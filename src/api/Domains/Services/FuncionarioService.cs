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
    public interface IFuncionarioService
    {
        Task<Pagination<Funcionario>> ListAsync(int offset, int limit);
        Task<Funcionario> GetAsync(int id);        
        Task<Funcionario> CreateAsync(Funcionario funcionario);
        Task<Funcionario> UpdateAsync(int id, Funcionario funcionario);
        Task DeleteAsync(int id);
    }

    public class FuncionarioService : IFuncionarioService
    {
        private readonly IValidator<Funcionario> _funcionarioValidator;
        private readonly ISqlService _sqlService;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<FuncionarioService> _logger;

        public FuncionarioService(
            IValidator<Funcionario> funcionarioValidator,
            ISqlService sqlService,
            IValidationService validationService,
            IAuthenticatedService authenticatedService,
            ILogger<FuncionarioService> logger)
        {
            _funcionarioValidator = funcionarioValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;

        }

        public async Task<Funcionario> CreateAsync(Funcionario funcionario)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existFuncionario = await _sqlService.ExistsAsync(FuncionarioQuery.EXIST_FUNCIONARIO, new
            {
                Cpf = funcionario.Documento
            });

            if (existFuncionario)
            {
                this._logger.LogDebug("Funcionario already exists, triggering 400");

                this._validationService.Throw("Funcionario", "There is already another funcionario with that Cpf", funcionario.Documento, Validation.FuncionarioExists);
            }

            this._logger.LogDebug("Inserting new Funcionario");

            funcionario.FuncionarioId = await _sqlService.CreateAsync(FuncionarioQuery.INSERT, new
            {
                FUNCIONARIOID = funcionario.FuncionarioId,
                NOME = funcionario.Nome,
                DOCUMENTO = funcionario.Documento,
                SALARIO = funcionario.Salario,
                ATIVO = funcionario.Ativo
            });

            this._logger.LogDebug("Ending CreateAsync");

            return funcionario;
        }

        public async Task DeleteAsync(int id)
        {
            this._logger.LogDebug("Starting DeleteAsync");

            this._logger.LogDebug("Retriving the product wants to delete");

            var funcionario = await GetAsync(id);

            var temFuncionario = await _sqlService.ExistsAsync(FuncionarioQuery.EXIST_FUNCIONARIO_ID, new { Id = funcionario.FuncionarioId });

            if (!temFuncionario)
            {
                this._logger.LogDebug("Funcionario already exists, triggering 400");

                this._validationService.Throw("Funcionario", "There is already another product with that Id", funcionario.FuncionarioId, Validation.FuncionarioNotExists);
            }

            this._logger.LogDebug("Deleting product");

            await _sqlService.ExecuteAsync(FuncionarioQuery.DELETE, new
            {
                FUNCIONARIOID = funcionario.FuncionarioId
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }

        public async Task<Funcionario> GetAsync(int id)
        {
            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a Funcionario");


            var funcionario = await _sqlService.GetAsync<Funcionario>(FuncionarioQuery.GET, new
            {
                Id = id
            });

            this._logger.LogDebug("Checking if product exists");

            if (funcionario == null)
            {
                this._logger.LogDebug("funcionario does not exists, triggering 404");

                throw new ArgumentNullException(nameof(id));
            }

            this._logger.LogDebug("Ending GetAsync");

            return funcionario;
        }

        public async Task<Pagination<Funcionario>> ListAsync(int offset, int limit)
        {
            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of products");

            var list = await _sqlService.ListAsync<Funcionario>(FuncionarioQuery.PAGINATE, new
            {
                Offset = offset,
                Limit = limit
            });

            this._logger.LogDebug("Retriving the number of funcionarios registered by the authenticated funcionario");

            var total = await _sqlService.CountAsync(FuncionarioQuery.TOTAL, new { });

            this._logger.LogDebug("Retriving the number of funcionarios registered by the authenticated funcionario");

            var pagination = new Pagination<Funcionario>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }

        public async Task<Funcionario> UpdateAsync(int id, Funcionario funcionario)
        {
            this._logger.LogDebug("Starting UpdateAsync");

            var oldFunc = await GetAsync(id);

            var existFuncionario = await _sqlService.ExistsAsync(FuncionarioQuery.EXIST_FUNCIONARIO_ID, new
            {
                Id = oldFunc.FuncionarioId
            });

            this._logger.LogDebug("Checking if that product already exists");

            if (!existFuncionario)
            {
                this._logger.LogDebug("Funcionario already exists, triggering 400");

                this._validationService.Throw("funcionario", "There is already another user with that funcionario", funcionario.FuncionarioId, Validation.FuncionarioNotExists);
            }

            this._logger.LogDebug("Updating product");

            await _sqlService.ExecuteAsync(FuncionarioQuery.UPDATE, new
            {
                Id = id,
                Nome = funcionario.Nome,
                Documento = funcionario.Documento,
                Salario = funcionario.Salario
            });

            funcionario.FuncionarioId = oldFunc.FuncionarioId;

            this._logger.LogDebug("Ending UpdateAsync");

            return funcionario;
        }
    }
}
