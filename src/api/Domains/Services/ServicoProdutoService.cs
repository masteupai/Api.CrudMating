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
    public interface IServicoProdutoService
    {
        Task<Pagination<ServicoProduto>> ListAsync(int offset, int limit, int servicoId);
        Task<ServicoProduto> GetAsync(int id);
        Task<ServicoProduto> CreateAsync(ServicoProduto produto);
        Task<ServicoProduto> UpdateAsync(int id, ServicoProduto produto);
        Task DeleteAsync(int id);
    }


    public class ServicoProdutoService : IServicoProdutoService
    {
        private readonly IValidator<ServicoProduto> _servicoProdutoValidator;
        private readonly ISqlService _sqlService;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<ServicoProdutoService> _logger;

        public ServicoProdutoService(
            IValidator<ServicoProduto> servicoProdutoValidator,
            ISqlService sqlService,
            IValidationService validationService,
            IAuthenticatedService authenticatedService,
            ILogger<ServicoProdutoService> logger)
        {
            _servicoProdutoValidator = servicoProdutoValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;

        }
        public async Task<ServicoProduto> CreateAsync(ServicoProduto produto)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existProdutoServico = await _sqlService.ExistsAsync(ServicoProdutoQuery.EXIST_SERVICO_PRODUTOID, new
            {
                Id = produto.ProdutoId
            });

            if (existProdutoServico)
            {
                this._logger.LogDebug("Produto already exists in servico, triggering 400");

                this._validationService.Throw("Produto", "There is already another Produto in servico with that Id", produto.ProdutoId, Validation.ProductExists);
            }

            this._logger.LogDebug("Inserting new produto in servico");

            produto.ProdutoId = await _sqlService.CreateAsync(ServicoProdutoQuery.INSERT, new
            {
                SERVICOID = produto.ServicoId,
                PRODUTOID = produto.ProdutoId
            });

            this._logger.LogDebug("Ending CreateAsync");

            return produto;
        }

        public async Task DeleteAsync(int id)
        {
            this._logger.LogDebug("Starting DeleteAsync");

            this._logger.LogDebug("Retriving the product wants to delete");

            var produto = await GetAsync(id);

            var temProdutoServico = await _sqlService.ExistsAsync(ServicoProdutoQuery.EXIST_SERVICO_ID, new { Id = produto.ServicoProdutoId });

            if (!temProdutoServico)
            {
                this._logger.LogDebug("Produto already not exists in servico, triggering 400");

                this._validationService.Throw("Produto", "There is already another Produto with that Id", produto.ServicoProdutoId, Validation.ProductNotExists);
            }

            this._logger.LogDebug("Deleting produto");

            await _sqlService.ExecuteAsync(ServicoProdutoQuery.DELETE, new
            {
                Id = produto.ServicoProdutoId
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }

        public async Task<ServicoProduto> GetAsync(int id)
        {
            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a produto in servico");


            var produtoServico = await _sqlService.GetAsync<ServicoProduto>(ServicoProdutoQuery.GET, new
            {
                Id = id
            });

            this._logger.LogDebug("Checking if product exists in servico");

            if (produtoServico == null)
            {
                this._logger.LogDebug("produto does not exists in servico, triggering 404");

                throw new ArgumentNullException(nameof(id));
            }

            this._logger.LogDebug("Ending GetAsync");

            return produtoServico;
        }

        public async Task<Pagination<ServicoProduto>> ListAsync(int offset, int limit, int servicoId)
        {
            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of products in servico");

            var list = await _sqlService.ListAsync<ServicoProduto>(ServicoProdutoQuery.PAGINATE, new
            {
                Offset = offset,
                Limit = limit,
                SERVICOID = servicoId
            });

            this._logger.LogDebug("Retriving the number of produto registered in servico");

            var total = await _sqlService.CountAsync(ServicoProdutoQuery.TOTAL, new { SERVICOID = servicoId });

            this._logger.LogDebug("Retriving the number of produto registered in servico");

            var pagination = new Pagination<ServicoProduto>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }

        public async Task<ServicoProduto> UpdateAsync(int id, ServicoProduto produto)
        {
            this._logger.LogDebug("Starting UpdateAsync");

            var oldProdutoServico = await GetAsync(id);

            var existFuncionario = await _sqlService.ExistsAsync(ServicoProdutoQuery.EXIST_SERVICO_ID, new
            {
                Id = oldProdutoServico.ServicoProdutoId
            });

            this._logger.LogDebug("Checking if that product already exists in servico");

            if (!existFuncionario)
            {
                this._logger.LogDebug("produto already exists, triggering 400");

                this._validationService.Throw("produto", "There is already another produto with that id", produto.ServicoProdutoId, Validation.FuncionarioNotExists);
            }

            this._logger.LogDebug("Updating product");

            await _sqlService.ExecuteAsync(ServicoFuncionarioQuery.UPDATE, new
            {
                Id = id,
                SERVICOID = produto.ServicoId,
                PRODUTOID = produto.ProdutoId
            });

            produto.ServicoProdutoId = oldProdutoServico.ServicoProdutoId;

            this._logger.LogDebug("Ending UpdateAsync");

            return produto;
        }
    }
}
