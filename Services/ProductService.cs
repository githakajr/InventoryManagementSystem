using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Services
{
    public class ProductService
    {
        private readonly InventoryContext _context;

        public ProductService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework in a real application)
                throw new ApplicationException("Unable to retrieve products", ex);
            }
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log and handle the exception
                throw new ApplicationException("Error adding product", ex);
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(product.Id);
                if (existingProduct == null)
                {
                    throw new KeyNotFoundException("Product not found");
                }

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log and handle the exception
                throw new ApplicationException("Error updating product", ex);
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                return await _context.Products.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Log and handle the exception
                throw new ApplicationException($"Error retrieving product with id {id}", ex);
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException("Product not found");
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log and handle the exception
                throw new ApplicationException($"Error deleting product with id {id}", ex);
            }
        }
    }
}
