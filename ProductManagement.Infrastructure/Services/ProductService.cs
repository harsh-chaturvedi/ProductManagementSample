using Microsoft.EntityFrameworkCore.Diagnostics;
using ProductManagement.Infrastructure.Model;
using ProductManagement.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Services
{
    public interface IProductService
    {
        Task<ResponseBase> AddProductAsync(Product product);
        Task<ResponseBase> DeleteAllProductAsync();
        Task<ResponseBase> DeleteProductByIdAsync(int Id);
        Task<ResponseBase> GetAsync();
        Task<ResponseBase> GetByCategoryAsync(string category);
        Task<ResponseBase> GetByIdAsync(int Id);
        Task<ResponseBase> TotalCountAsync();
        Task<ResponseBase> SearchAsync(string name);
        Task<ResponseBase> GetSortedAsync(SortOrder order);
        Task<ResponseBase> UpdateProductAsync(Product product);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ResponseBase> AddProductAsync(Product product)
        {
            if (string.IsNullOrEmpty(product.Name))
                throw new ArgumentException(Constants.ProductNameEmpty);

            var validationResult = await _productRepository.ValidateProductNameAsync(product.Name);

            if (!validationResult.Success)
            {
                return new ResponseBase(validationResult.Success, Constants.ProductNameDuplicate);
            }

            var id = await _productRepository.AddProductAsync(product);
            return new AddProductResponse(true) { Id = id };
        }

        public async Task<ResponseBase> DeleteAllProductAsync()
        {
            var result = await _productRepository.DeleteAllProductAsync();
            return new ResponseBase(result);
        }

        public async Task<ResponseBase> DeleteProductByIdAsync(int Id)
        {
            var currentProduct = await this.GetByIdAsync(Id);
            if (!currentProduct.Success || !(currentProduct as GetAllProductResponse).Products.Any())
            {
                return new ResponseBase(false);
            }

            var result = await _productRepository.DeleteProductByIdAsync(Id);

            return new ResponseBase(result);
        }

        public async Task<ResponseBase> GetAsync()
        {
            var result = await _productRepository.GetAsync();
            if (!result.Any())
                return new ResponseBase(false, Constants.ProductNotFound);

            var response = new GetAllProductResponse(true);
            response.Products = result;
            return response;
        }

        public async Task<ResponseBase> GetByCategoryAsync(string category)
        {
            if (string.IsNullOrEmpty(category))
                return new ResponseBase(false, Constants.ProductCategoryEmpty);

            var result = await _productRepository.GetByCategoryAsync(category);
            if (!result.Any())
                return new ResponseBase(false, Constants.ProductNotFound);

            var response = new GetAllProductResponse(true);
            response.Products = result;
            return response;
        }

        public async Task<ResponseBase> GetByIdAsync(int Id)
        {
            if (Id < 0)
                return new ResponseBase(false, Constants.ProductIdLessThanZero);

            var result = await _productRepository.GetByIdAsync(Id);

            if (result == null)
                return new ResponseBase(false, Constants.ProductNotFound);

            var response = new GetAllProductResponse(true);
            response.Products = new Product[] { result };
            return response;
        }

        public async Task<ResponseBase> GetSortedAsync(SortOrder order)
        {
            var result = await _productRepository.GetSortedAsync(order);
            if (!result.Any())
                return new ResponseBase(false, Constants.ProductNotFound);

            var response = new GetAllProductResponse(true);
            response.Products = result;
            return response;
        }

        public async Task<ResponseBase> SearchAsync(string name)
        {
            var result = await _productRepository.SearchAsync(name);

            if (result == null)
                return new ResponseBase(false, Constants.ProductNotFound);

            var response = new GetAllProductResponse(true);
            response.Products = result;
            return response;
        }

        public async Task<ResponseBase> TotalCountAsync()
        {
            var result = await _productRepository.TotalCountAsync();

            if (result == 0)
                return new ResponseBase(false, Constants.ProductNotFound);

            var response = new TotalCountResponse(true);
            response.Count = result;
            return response;
        }

        public async Task<ResponseBase> UpdateProductAsync(Product product)
        {
            if (product.Id < 0)
                return new ResponseBase(false, Constants.ProductIdLessThanZero);

            var currentProduct = await this.GetByIdAsync(product.Id);
            if (currentProduct == null || !(currentProduct as GetAllProductResponse).Products.Any())
            {
                return new ResponseBase(false, Constants.ProductNotFound);
            }

            var validationResult = await _productRepository.ValidateProductNameUpdateAsync(product.Id, product.Name);

            if (!validationResult.Success)
            {
                return new ResponseBase(validationResult.Success, Constants.ProductNameDuplicate);
            }

            await _productRepository.UpdateProductAsync(product);
            return new ResponseBase(true);
        }
    }
}
