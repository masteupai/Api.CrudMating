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
    public interface IVeiculoService
    {
        Task<Pagination<Veiculo>> ListAsync(int offset, int limit);
        Task<Veiculo> GetAsync(int id);
        Task<Veiculo> CreateAsync(Veiculo veiculo);
        Task<Veiculo> UpdateAsync(int id, Veiculo veiculo);
        Task DeleteAsync(int id);
    }
    public class VeiculoService : IVeiculoService
    {
        private readonly IValidator<Veiculo> _veiculoValidator;
        private readonly ISqlService _sqlService;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<VeiculoService> _logger;

        public VeiculoService(
            IValidator<Veiculo> veiculoValidator,
            ISqlService sqlService,
            IValidationService validationService,
            IAuthenticatedService authenticatedService,
            ILogger<VeiculoService> logger)
        {
            _veiculoValidator = veiculoValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;

        }
        public async Task<Veiculo> CreateAsync(Veiculo veiculo)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existVeiculo = await _sqlService.ExistsAsync(VeiculoQuery.EXIST_VEICULO_DO_CLIENTE, new
            {
                Placa = veiculo.Placa,
                ClienteId = veiculo.ClienteId
            });

            if (existVeiculo)
            {
                this._logger.LogDebug("veiculo already exists, triggering 400");

                this._validationService.Throw("veiculo", "There is already another veiculo with that placa", veiculo.Placa, Validation.FuncionarioExists);
            }

            this._logger.LogDebug("Inserting new veiculo");

            veiculo.VeiculoId = await _sqlService.CreateAsync(VeiculoQuery.INSERT, new
            {
                CLIENTEID = veiculo.ClienteId,
                MODELO = veiculo.Modelo,
                MARCA = veiculo.Marca,
                ANO = veiculo.Ano,
                PLACA = veiculo.Placa,
                COR = veiculo.Cor
            });

            this._logger.LogDebug("Ending CreateAsync");

            return veiculo;
        }

        public async Task DeleteAsync(int id)
        {
            this._logger.LogDebug("Starting DeleteAsync");

            this._logger.LogDebug("Retriving the Veiculo wants to delete");

            var veiculo = await GetAsync(id);

            var temVeiculo = await _sqlService.ExistsAsync(VeiculoQuery.EXIST_VEICULO_ID, new { Id = veiculo.VeiculoId });

            if (!temVeiculo)
            {
                this._logger.LogDebug("Veiculo already exists, triggering 400");

                this._validationService.Throw("Veiculo", "There is already another Veiculo with that Id", veiculo.ClienteId, Validation.ClienteNotExists);
            }


            this._logger.LogDebug("Deleting product");

            await _sqlService.ExecuteAsync(VeiculoQuery.DELETE, new
            {
                Id = veiculo.VeiculoId
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }

        public async Task<Veiculo> GetAsync(int id)
        {
            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a veiculo");


            var veiculo = await _sqlService.GetAsync<Veiculo>(VeiculoQuery.GET, new
            {
                Id = id
            });

            this._logger.LogDebug("Checking if product exists");

            if (veiculo == null)
            {
                this._logger.LogDebug("veiculo does not exists, triggering 404");

                throw new ArgumentNullException(nameof(id));
            }

            this._logger.LogDebug("Ending GetAsync");

            return veiculo;
        }

        public async Task<Pagination<Veiculo>> ListAsync(int offset, int limit)
        {
            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of products");

            var list = await _sqlService.ListAsync<Veiculo>(VeiculoQuery.PAGINATE, new
            {
                Offset = offset,
                Limit = limit
            });

            this._logger.LogDebug("Retriving the number of veiculos registered by the authenticated veiculo");

            var total = await _sqlService.CountAsync(VeiculoQuery.TOTAL, new { });

            this._logger.LogDebug("Retriving the number of veiculos registered by the authenticated veiculo");

            var pagination = new Pagination<Veiculo>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }

        public async Task<Veiculo> UpdateAsync(int id, Veiculo veiculo)
        {
            this._logger.LogDebug("Starting UpdateAsync");

            var oldveiculo = await GetAsync(id);

            var existVeiculo = await _sqlService.ExistsAsync(VeiculoQuery.EXIST_VEICULO_DO_CLIENTE, new
            {
                ClienteId = veiculo.ClienteId,
                Placa = veiculo.Placa
            });

            this._logger.LogDebug("Checking if that product already exists");
            if (existVeiculo && oldveiculo.Placa == veiculo.Placa && oldveiculo.Marca == veiculo.Marca && oldveiculo.Modelo == veiculo.Modelo && oldveiculo.Ano == veiculo.Ano && oldveiculo.Cor == veiculo.Cor)
            {
                this._logger.LogDebug("veiculo already exists, triggering 400");

                this._validationService.Throw("veiculo", "There is already another veiculo with that placa", veiculo.Placa, Validation.FuncionarioNotExists);
            }

            this._logger.LogDebug("Updating product");

            await _sqlService.ExecuteAsync(VeiculoQuery.UPDATE, new
            {
                Id = oldveiculo.VeiculoId,
                CLIENTEID = veiculo.ClienteId,
                MODELO = veiculo.Modelo,
                MARCA = veiculo.Marca,
                ANO = veiculo.Ano,
                PLACA = veiculo.Placa,
                COR = veiculo.Cor
            });

            veiculo.VeiculoId = oldveiculo.VeiculoId;

            this._logger.LogDebug("Ending UpdateAsync");

            return veiculo;
        }
    }
}
