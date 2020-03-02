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
    public interface IProductService
    {
        Task<Pagination<Product>> ListAsync(int offset, int limit);
        Task<Product> GetAsync(int id);
        Task<Product> CreateAsync(Product user);
        Task<Product> UpdateAsync(int id, Product user);
        Task DeleteAsync(int id);
       
    }

    public class ProductService : IProductService
    {
        private readonly ISqlService _sqlService;
        private readonly IValidator<Product> _productValidator;
        private readonly IValidationService _validationService;
        private readonly IAuthenticatedService _authenticatedService;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
             IValidator<Product> productValidator,
             ISqlService sqlService,
             IValidationService validationService,
             IAuthenticatedService authenticatedService,
             ILogger<ProductService> logger)
        {
            _productValidator = productValidator;
            _sqlService = sqlService;
            _validationService = validationService;
            _authenticatedService = authenticatedService;
            _logger = logger;
        }

        public async Task<Product> CreateAsync(Product product)
        {
            this._logger.LogDebug("Starting CreateAsync");

            var existsProduct = await _sqlService.ExistsAsync(ProductQuery.EXIST_PRODUCT , new
            {
                CodProduct = product.Codigo
            });

            if (existsProduct)
            {
                this._logger.LogDebug("Product already exists, triggering 400");

                this._validationService.Throw("Product", "There is already another product with that CodProduct", product.Codigo, Validation.UserRepeatedDocument);
            }

            this._logger.LogDebug("Inserting new Product");

            product.ProdutoId = await _sqlService.CreateAsync(ProductQuery.INSERT, new
            {
                IdProduct = product.ProdutoId,
                CodProduct = product.Codigo,
                ProductName = product.Nome,
                Descricao = product.Descricao,
                Quant = product.Quantidade,
                Value = product.Valor,
                ProdType = product.Tipo,
                Active = product.Ativo,
                CreatedBy = _authenticatedService.Token().Subject
            });

            this._logger.LogDebug("Ending CreateAsync");

            return product;
        }

        public async Task DeleteAsync(int id)
        {
            this._logger.LogDebug("Starting DeleteAsync");

            this._logger.LogDebug("Retriving the product wants to delete");

            var product = await GetAsync(id);

            var temProduct = await _sqlService.ExistsAsync(ProductQuery.EXIST_PRODUCT, new { CodProduct = product.Codigo });

            if (!temProduct)
            {
                this._logger.LogDebug("Product already exists, triggering 400");

                this._validationService.Throw("Product", "There is already another product with that CodProduct", product.Codigo, Validation.ProductExists);
            }

            this._logger.LogDebug("Deleting product");

            await _sqlService.ExecuteAsync(ProductQuery.DELETE, new
            {
                IdProduct = product.ProdutoId,
                CreatedBy = _authenticatedService.Token().Subject
            });

            this._logger.LogDebug("Ending DeleteAsync");
        }

        public async Task<Product> GetAsync(int codProduct)
        {
            this._logger.LogDebug("Starting GetAsync");

            this._logger.LogDebug("Retriving a product");


            var product = await _sqlService.GetAsync<Product>(ProductQuery.GET, new
            {
                CodProduct = codProduct              
            });         

            this._logger.LogDebug("Checking if product exists");

            if (product == null)
            {
                this._logger.LogDebug("Product does not exists, triggering 404");

                throw new ArgumentNullException(nameof(codProduct));
            }

            this._logger.LogDebug("Ending GetAsync");

            return product;
        }

        public async Task<Pagination<Product>> ListAsync(int offset, int limit)
        {
            this._logger.LogDebug("Starting ListAsync");

            this._logger.LogDebug("Validating pagination parameters");

            if (limit - offset > 100)
            {
                this._validationService.Throw("Limit", "Too much items for pagination", limit, Validation.PaginationExceedsLimits);
            }

            this._logger.LogDebug("Retriving paginated list of products");

            var list = await _sqlService.ListAsync<Product>(ProductQuery.PAGINATE, new
            {
                CreatedBy = _authenticatedService.Token().Subject,
                Offset = offset,
                Limit = limit
            });

            this._logger.LogDebug("Retriving the number of product registered by the authenticated product");

            var total = await _sqlService.CountAsync(ProductQuery.TOTAL, new
            {
                CreatedBy = _authenticatedService.Token().Subject,
            });

            this._logger.LogDebug("Retriving the number of product registered by the authenticated product");

            var pagination = new Pagination<Product>()
            {
                Offset = offset,
                Limit = limit,
                Items = list,
                Total = total
            };

            this._logger.LogDebug("Ending ListAsync");

            return pagination;
        }

        public async Task<Product> UpdateAsync(int codProduct, Product product)
        {
            this._logger.LogDebug("Starting UpdateAsync");

            this._logger.LogDebug("Validating payload");

            //await _userValidator.ValidateAndThrowAsync(product);

            //this._logger.LogDebug("Retriving the user the user wants to update");

            var oldProduct = await GetAsync(codProduct);

            var existsProduct = await _sqlService.ExistsAsync(ProductQuery.EXIST_PRODUCT, new
            {
                CodProduct = oldProduct.Codigo                
            });

            this._logger.LogDebug("Checking if that product already exists");

            if (!existsProduct)
            {
                this._logger.LogDebug("Email already exists, triggering 400");

                this._validationService.Throw("Email", "There is already another user with that email", product.Codigo, Validation.ProductNotExists);
            }

            this._logger.LogDebug("Updating product");

            await _sqlService.ExecuteAsync(ProductQuery.UPDATE, new
            {
                IdProduct = product.ProdutoId,
                CodProduct = product.Codigo,
                ProductName = product.Nome,
                Descricao = product.Descricao,
                Quant = product.Quantidade,
                Value = product.Valor,
                ProdType = product.Tipo,
                Active = product.Ativo,
                CreatedBy = _authenticatedService.Token().Subject
            });

            product.ProdutoId = oldProduct.ProdutoId;

            this._logger.LogDebug("Ending UpdateAsync");

            return product;
        }
    }
}
