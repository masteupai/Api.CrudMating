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
    public interface IContatosFuncionarioService
    {
        Task<Pagination<ContatoFuncionario>> ListAsync(int offset, int limit, int funcionarioId);
        Task<ContatoFuncionario> GetAsync(int id);
        Task<ContatoFuncionario> CreateAsync(ContatoFuncionario contatoCliente);
        Task<ContatoFuncionario> UpdateAsync(int id, ContatoFuncionario contatoCliente);
        Task DeleteAsync(int id);
    }
    public class ContatosFuncionarioService : IContatosFuncionarioService
    {
        private readonly IValidator<ContatoFuncionario> _contatoFuncionarioValidator;
        private readonly ISqlService _sqlService;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<ContatosFuncionarioService> _logger;
        public ContatosFuncionarioService(
            IValidator<ContatoFuncionario> contatoFuncionarioValidator,
            ISqlService sqlService,
            IValidationService validationService,
            IAuthenticatedService authenticatedService,
            ILogger<ContatosFuncionarioService> logger)
        {
            _contatoFuncionarioValidator = contatoFuncionarioValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;

        }
        public async Task<ContatoFuncionario> CreateAsync(ContatoFuncionario contatoFun)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existContatoEmail = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOFUN_EMAIL, new
            {
                EMAIL = contatoFun.Email
            });
            var existContatoTelefone = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOFUN_TELEFONE, new
            {
                TELEFONE = contatoFun.Email
            });

            if (existContatoEmail && existContatoTelefone)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another funcionario with that Email or Telefone", contatoFun, Validation.ContatoExists);
            }

            this._logger.LogDebug("Inserting new Contato");

            contatoFun.ContatoFunId = await _sqlService.CreateAsync(ContatoFuncionarioQuery.INSERT, new
            {
                FUNCIONARIOID = contatoFun.FuncionarioId,
                EMAIL = contatoFun.Email,
                TELEFONE = contatoFun.Telefone
            });

            this._logger.LogDebug("Ending CreateAsync");

            return contatoFun;
        }


        public async Task<ContatoFuncionario> UpdateAsync(int id, ContatoFuncionario contatoFun)
        {

            this._logger.LogDebug("Starting UpdateAsync");

            var oldContato = await GetAsync(id);

            var existContatoEmail = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOFUN_EMAIL, new
            {
                EMAIL = contatoFun.Email
            });
            var existContatoTelefone = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOFUN_TELEFONE, new
            {
                TELEFONE = contatoFun.Telefone
            });

            if (existContatoEmail && existContatoTelefone)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another funcionario with that Email or Telefone", contatoFun, Validation.ContatoExists);
            }

            var existContato = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOFUN_ID, new
            {
                Id = oldContato.ContatoFunId
            });

            this._logger.LogDebug("Checking if that contato already exists");

            if (!existContato)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another funcionario with that id", contatoFun.ContatoFunId, Validation.ContatoNotExists);
            }

            this._logger.LogDebug("Updating contato");

            await _sqlService.ExecuteAsync(ContatoFuncionarioQuery.UPDATE, new
            {
                Id = id,
                FUNCIONARIOID = contatoFun.FuncionarioId,
                EMAIL = contatoFun.Email,
                TELEFONE = contatoFun.Telefone
            });

            contatoFun.ContatoFunId = oldContato.ContatoFunId;

            this._logger.LogDebug("Ending UpdateAsync");

            return contatoFun;
        }

        public async Task<ContatoFuncionario> GetAsync(int id)
        {

            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a Funcionario");


            var contato = await _sqlService.GetAsync<ContatoFuncionario>(ContatoFuncionarioQuery.GET, new
            {
                Id = id
            });

            this._logger.LogDebug("Checking if contato exists");

            if (contato == null)
            {
                this._logger.LogDebug("Contato does not exists, triggering 404");

                throw new ArgumentNullException(nameof(id));
            }

            this._logger.LogDebug("Ending GetAsync");

            return contato;
        }

        public async Task<Pagination<ContatoFuncionario>> ListAsync(int offset, int limit, int funcionarioId)
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
                Limit = limit,
                FuncionarioId = funcionarioId
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

            var temContato = await _sqlService.ExistsAsync(ContatoFuncionarioQuery.EXIST_CONTATOFUN_ID, new { ID = clienteContato.ContatoFunId });

            if (!temContato)
            {
                this._logger.LogDebug("Contato already exists, triggering 400");

                this._validationService.Throw("Contato", "There is already another Contato with that Id", clienteContato.ContatoFunId, Validation.ClienteNotExists);
            }


            this._logger.LogDebug("Deleting contato");

            await _sqlService.ExecuteAsync(ContatoFuncionarioQuery.DELETE, new
            {
                Id = clienteContato.ContatoFunId
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }
    }
}
