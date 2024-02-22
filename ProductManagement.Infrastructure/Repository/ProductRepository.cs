using Microsoft.EntityFrameworkCore;
using ProductManagement.Infrastructure.Context;
using ProductManagement.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Infrastructure.Repository
{
    public interface IProductRepository
    {
        Task<int> AddProductAsync(Product product);
        Task<bool> DeleteAllProductAsync();
        Task<bool> DeleteProductByIdAsync(int Id);
        Task<IEnumerable<Product>> GetAsync();
        Task<IEnumerable<Product>> GetByCategoryAsync(string category);
        Task<Product> GetByIdAsync(int Id);
        Task<int> TotalCountAsync();
        Task<IEnumerable<Product>> SearchAsync(string name);
        Task<IEnumerable<Product>> GetSortedAsync(SortOrder order);
        Task<bool> UpdateProductAsync(Product product);

        Task<ValidationResult> ValidateProductNameAsync(string productName);

        Task<ValidationResult> ValidateProductNameUpdateAsync(int id, string productName);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<int> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<bool> DeleteAllProductAsync()
        {
            var count = await _context.Products.ExecuteDeleteAsync();
            return count > 0;
        }

        public async Task<bool> DeleteProductByIdAsync(int Id)
        {
            var entity = await _context.Products.FindAsync(Id);
            if (entity != null)
            {
                _context.Products.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            else
                return false;
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            var entity = await _context.Products.ToListAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string category)
        {
            var entity = await _context.Products.Where(x => x.Category.ToLower() == category.ToLower()).ToListAsync();
            return entity;
        }

        public async Task<Product> GetByIdAsync(int Id)
        {
            var entity = await _context.Products.FindAsync(Id);
            return entity;
        }

        public async Task<IEnumerable<Product>> GetSortedAsync(SortOrder order)
        {
            var query = _context.Products;

            switch (order)
            {
                case SortOrder.Ascending:
                case SortOrder.Asc:
                case SortOrder.ascending:
                case SortOrder.asc:
                    query.OrderBy(x => x.Name);
                    break;
                case SortOrder.Desc:
                case SortOrder.Descending:
                case SortOrder.descending:
                case SortOrder.desc:
                    query.OrderByDescending(x => x.Name);
                    break;
            }

            var entity = await query.ToListAsync();
            return entity;
        }

        public async Task<IEnumerable<Product>> SearchAsync(string name)
        {
            IEnumerable<Product> entity;
            var query = _context.Products;
            if (string.IsNullOrEmpty(name))
            {
                entity = await query.ToListAsync();
            }
            else
            {
                entity = await query.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            }
            return entity;
        }

        public async Task<int> TotalCountAsync()
        {
            var count = await _context.Products.CountAsync();
            return count;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var entity = _context.Products.Find(product.Id);

            entity.Price = product.Price;
            entity.Category = product.Category;
            entity.Name = product.Name;
            entity.Description = product.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ValidationResult> ValidateProductNameAsync(string productName)
        {
            var item = await _context.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == productName.ToLower());
            return item == null ? new ValidationResult(true) : new ValidationResult(false);
        }

        public async Task<ValidationResult> ValidateProductNameUpdateAsync(int id, string productName)
        {
            var item = await _context.Products.FirstOrDefaultAsync(p => p.Id != id && p.Name.ToLower() == productName.ToLower());
            return item == null ? new ValidationResult(true) : new ValidationResult(false);
        }
    }
}
