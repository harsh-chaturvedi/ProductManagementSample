using Microsoft.AspNetCore.Mvc;
using ProductManagement.Infrastructure.Model;
using ProductManagement.Infrastructure.Services;
using ProductManagment.Helpers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace ProductManagment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Add Products
        /// </summary>
        /// <param name="product">Product Details</param>
        /// <returns>Id of new product as API Response</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Add a product.", Description = "Add a product.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            var validationResult = ValidationHelper.ValidateProduct(product);

            if (!validationResult.Success)
                return BadRequest(validationResult.Message);

            var operationResult = await _productService.AddProductAsync(product);

            if (!operationResult.Success)
                return BadRequest(operationResult.Message);

            return StatusCode((int)HttpStatusCode.Created, (operationResult as AddProductResponse).Id);
        }

        /// <summary>
        /// Get all Products(s)
        /// </summary>
        /// <returns>List of all Products</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all products.", Description = "Get all products.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> Get()
        {
            var operationResult = await _productService.GetAsync();
            if (!operationResult.Success)
                return Ok();

            var response = (operationResult as GetAllProductResponse);
            return Ok(response.Products);
        }

        /// <summary>
        /// Get a Products By Id
        /// </summary>
        /// <param name="Id">Product Id</param>
        /// <returns>Product with given Id</returns>
        [HttpGet]
        [Route("{Id}")]
        [SwaggerOperation(Summary = "Get a Products By Id.", Description = "Get a Products By Id.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> GetById([FromRoute] int Id)
        {
            var result = await _productService.GetByIdAsync(Id);

            if (!result.Success)
                return NotFound();

            var response = (result as GetAllProductResponse).Products.FirstOrDefault();
            return Ok(response);
        }

        /// <summary>
        /// Search a list of products by name
        /// </summary>
        /// <param name="name">Search Query parameter</param>
        /// <returns>List of products matching search query</returns>
        [HttpGet]
        [Route("search")]
        [SwaggerOperation(Summary = "Search a list of products by name.", Description = "Search a list of products by name.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> Search([FromQuery] string? name)
        {
            var operationResult = await _productService.SearchAsync(name);

            if (!operationResult.Success)
                return BadRequest(operationResult.Message);

            return Ok((operationResult as GetAllProductResponse).Products);
        }

        /// <summary>
        /// Returns a total count of all products
        /// </summary>
        /// <returns>Count of all products</returns>
        [HttpGet]
        [Route("total-count")]
        [SwaggerOperation(Summary = "Returns a total count of all products.", Description = "Returns a total count of all products.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> TotalCount()
        {
            //Use Global Exception Handler to handle exceptions
            var operationResult = await _productService.TotalCountAsync();

            if (!operationResult.Success)
                return BadRequest(operationResult.Message);

            return Ok((operationResult as TotalCountResponse).Count);
        }

        /// <summary>
        /// Get all Products By category
        /// </summary>
        /// <param name="category">Categorny name for products</param>
        /// <returns>List of all products filtered on the query</returns>
        [HttpGet]
        [Route("category/{category}")]
        [SwaggerOperation(Summary = "Get a Products By Id.", Description = "Get a Products By Id.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> GetByCategory([FromRoute] string category)
        {
            var operationResult = await _productService.GetByCategoryAsync(category);

            if (!operationResult.Success)
                return BadRequest(operationResult.Message);

            return Ok((operationResult as GetAllProductResponse).Products);
        }

        /// <summary>
        /// Search a list of products sorted by name
        /// </summary>
        /// <param name="order">Sort order Query parameter</param>
        /// <returns>List of all products sorted in given order</returns>
        [HttpGet]
        [Route("sort")]
        [SwaggerOperation(Summary = "Search a list of products sorted by name.", Description = "Search a list of products sorted by name.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> GetSorted([FromQuery] string order)
        {
            if (!Enum.TryParse(order, out SortOrder sortOrder))
            {
                return BadRequest(Constants.InValidSortOrder);
            };

            var operationResult = await _productService.GetSortedAsync(sortOrder);

            if (!operationResult.Success)
                return BadRequest(operationResult.Message);

            return Ok((operationResult as GetAllProductResponse).Products);
        }

        /// <summary>
        /// Update a Products(s)
        /// </summary>
        /// <param name="product">Product Details</param>
        /// <returns>Success message</returns>
        [HttpPut]
        [SwaggerOperation(Summary = "Update a product.", Description = "Update a product.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var validationResult = ValidationHelper.ValidateProductUpdate(product);

            if (!validationResult.Success)
                return BadRequest(validationResult.Message);

            var operationResult = await _productService.UpdateProductAsync(product);

            if (!operationResult.Success)
                return BadRequest(operationResult.Message);

            return NoContent();
        }

        /// <summary>
        /// Delete all Products(s)
        /// </summary>
        /// <returns>No content</returns>
        [HttpDelete]
        [SwaggerOperation(Summary = "Delete all product.", Description = "Delete all product.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> DeleteAllProduct()
        {
            await _productService.DeleteAllProductAsync();
            return NoContent();
        }

        /// <summary>
        /// Delete all Products(s)
        /// </summary>
        /// <param name="Id">Product Id</param>
        /// <returns>No content</returns>
        [HttpDelete]
        [Route("{Id}")]
        [SwaggerOperation(Summary = "Delete all product.", Description = "Delete all product.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Success", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Data not available", typeof(ResponseBase))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unexpected error occured while processing.")]
        public async Task<IActionResult> DeleteProductById([FromRoute] int Id)
        {
            var result = await _productService.DeleteProductByIdAsync(Id);

            if (!result.Success)
                return NotFound();

            return NoContent();
        }
    }
}
