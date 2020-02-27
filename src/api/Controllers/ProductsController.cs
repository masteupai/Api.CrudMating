using System.Threading.Tasks;
using API.Domains.Models;
using API.Domains.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("products")]
    [SwaggerTag("Create, edit, delete and retrieve products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [SwaggerOperation(
           Summary = "Retrieve a paginated list of products",
           Description = "Retrieves only products that were created by the authenticated product"
       )]
        [SwaggerResponse(200, "List of products filtered by the informed parameters", typeof(Pagination<Product>))]
        public async Task<ActionResult> List(
           [SwaggerParameter("Skip that many items before beginning to return items")][BindRequired]  int offset,
           [SwaggerParameter("Limit the number of items that are returned")][BindRequired] int limit)
        {
            var pagination = await _productService.ListAsync(offset, limit);

            return Ok(pagination);
        }

        [HttpGet("{codProduct}")]
        [SwaggerOperation(
            Summary = "Retrieve a product by their Cod",
            Description = "Retrieves product only if it were created by the authenticated product"
        )]
        [SwaggerResponse(200, "A product filtered by their codProduct", typeof(Product))]
        public async Task<ActionResult> Get([SwaggerParameter("Product's Cod")]int codProduct)
        {
            var user = await _productService.GetAsync(codProduct);

            return Ok(user);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new product",
            Description = "Creates a new product if all validations are succeded"
        )]
        [SwaggerResponse(201, "The product was successfully created", typeof(Product))]
        public async Task<ActionResult> Post([FromBody] Product product)
        {
            var created = await _productService.CreateAsync(product);

            return CreatedAtAction(nameof(Post), new { id = product.ProdutoId }, created);
        }

        [HttpPut("{codProduct}")]
        [SwaggerOperation(
            Summary = "Edits an existing product by their Cod",
            Description = "Edits an existing product if all validations are succeded and were created by the authenticated product"
        )]
        [SwaggerResponse(200, "The product was successfully edited", typeof(Product))]
        public async Task<ActionResult> Put(
            [SwaggerParameter("Product's Cod")] int codProduct,
            [FromBody] Product product)
        {
            var edited = await _productService.UpdateAsync(codProduct, product);

            return Ok(edited);
        }

        [HttpDelete("{codProduct}")]
        [SwaggerOperation(
            Summary = "Deletes a product by their Cod",
            Description = "Deletes a product if that user is deletable and were created by the authenticated product"
        )]
        [SwaggerResponse(204, "The product was successfully deleted")]
        public async Task<ActionResult> Delete(
            [SwaggerParameter("Product's Cod")]int codProduct)
        {
            await _productService.DeleteAsync(codProduct);

            return NoContent();
        }
    }
}