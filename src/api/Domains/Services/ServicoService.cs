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
    public interface IServicoService
    {
        Task<Pagination<Servico>> ListAsync(int offset, int limit);
        Task<Pagination<Servico>> ListPerVeiculoAsync(int offset, int limit, int veiculoId);
        Task<Servico> GetAsync(int id);
        Task<Servico> CreateAsync(Servico servico);
        Task<Servico> UpdateAsync(int id, Servico servico);
        Task DeleteAsync(int id);
    }
    public class ServicoService : IServicoService
    {
        private readonly ISqlService _sqlService;
        private readonly IValidator<Servico> _servicoValidator;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<ServicoService> _logger;

        public ServicoService(
             IValidator<Servico> servicoValidator,
             ISqlService sqlService,
             IValidationService validationService,
             IAuthenticatedService authenticatedService,
             ILogger<ServicoService> logger)
        {
            _servicoValidator = servicoValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;
        }

        public async Task<Servico> CreateAsync(Servico servico)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existsProduct = await _sqlService.ExistsAsync(ServicosQuery.EXIST_SERVICO_ID, new
            {
                Id = servico.ServicoId
            });

            if (existsProduct)
            {
                this._logger.LogDebug("Servico already exists, triggering 400");

                this._validationService.Throw("Servico", "There is already another servico with that Id", servico.ServicoId, Validation.UserRepeatedDocument);
            }

            this._logger.LogDebug("Inserting new servico");

            servico.ServicoId = await _sqlService.CreateAsync(ServicosQuery.INSERT, new
            {
                VEICULOID = servico.VeiculoId,
                DATAINICIO = servico.DataInicio,
                DATAFIM = servico.DataFim,
                QUILOMETRAGEM = servico.Quilometragem,
                PRECOTOTAL = servico.PrecoTotal
            });

            this._logger.LogDebug("Ending CreateAsync");

            return servico;
        }

        public async Task DeleteAsync(int id)
        {
            this._logger.LogDebug("Starting DeleteAsync");

            this._logger.LogDebug("Retriving the servico wants to delete");

            var servico = await GetAsync(id);

            var temServico = await _sqlService.ExistsAsync(ServicosQuery.EXIST_SERVICO_ID, new { Id = servico.ServicoId });

            if (!temServico)
            {
                this._logger.LogDebug("Servico already exists, triggering 400");

                this._validationService.Throw("Servico", "There is already another product with that Id", servico.ServicoId, Validation.ProductExists);
            }

            this._logger.LogDebug("Deleting Servico");

            await _sqlService.ExecuteAsync(ServicosQuery.DELETE, new
            {
                ID = id
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }

        public async Task<Servico> GetAsync(int id)
        {
            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a Servico");


            var servico = await _sqlService.GetAsync<Servico>(ServicosQuery.GET, new
            {
                Id = id
            });

            this._logger.LogDebug("Checking if servico exists");

            if (servico == null)
            {
                this._logger.LogDebug("Servico does not exists, triggering 404");

                throw new ArgumentNullException(nameof(id));
            }

            this._logger.LogDebug("Ending GetAsync");

            return servico;
        }

        public async Task<Pagination<Servico>> ListAsync(int offset, int limit)
        {
            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of Servicos");

            var list = await _sqlService.ListAsync<Servico>(ServicosQuery.PAGINATE, new
            {
                Offset = offset,
                Limit = limit
            });

            this._logger.LogDebug("Retriving the number of Servicos");

            var total = await _sqlService.CountAsync(ServicosQuery.TOTAL, new
            {
                CreatedBy = _authenticatedService.Token().Subject
            });

            this._logger.LogDebug("Retriving the number of servicos");

            var pagination = new Pagination<Servico>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }

        public async Task<Servico> UpdateAsync(int id, Servico servico)
        {

            this._logger.LogDebug("Starting UpdateAsync");

            var oldServico = await GetAsync(id);

            var existsServico = await _sqlService.ExistsAsync(ServicosQuery.EXIST_SERVICO_ID, new
            {
                Id = oldServico.ServicoId
            });

            this._logger.LogDebug("Checking if that product already exists");

            if (!existsServico)
            {
                this._logger.LogDebug("Servico already not exists, triggering 400");

                this._validationService.Throw("Servico", "There is already another Servico with that Id", servico.ServicoId, Validation.ProductNotExists);
            }

            this._logger.LogDebug("Updating Servico");

            await _sqlService.ExecuteAsync(ServicosQuery.UPDATE, new
            {
                ID = servico.ServicoId,
                VEICULOID = servico.VeiculoId,
                DATAINICIO = servico.DataInicio,
                DATAFIM = servico.DataFim,
                QUILOMETRAGEM = servico.Quilometragem,
                PRECOTOTAL = servico.PrecoTotal
            });

            servico.ServicoId = oldServico.ServicoId;

            this._logger.LogDebug("Ending UpdateAsync");

            return servico;
        }


        public async Task<Pagination<Servico>> ListPerVeiculoAsync(int offset, int limit, int veiculoId)
        {
            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of Servicos");

            var list = await _sqlService.ListAsync<Servico>(ServicosQuery.PAGINATEPERVEICULO, new
            {
                Offset = offset,
                Limit = limit,
                VEICULOID = veiculoId
            });

            this._logger.LogDebug("Retriving the number of Servicos");

            var total = await _sqlService.CountAsync(ServicosQuery.TOTAL, new
            {
                CreatedBy = _authenticatedService.Token().Subject
            });

            this._logger.LogDebug("Retriving the number of servicos");

            var pagination = new Pagination<Servico>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }
    }
}
