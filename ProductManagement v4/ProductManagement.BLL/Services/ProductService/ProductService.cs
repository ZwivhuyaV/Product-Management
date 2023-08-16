using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProductManagement.BLL.Services.AuditService;
using ProductManagement.BLL.Services.EmployeeService;
using ProductManagement.DAL.Common;
using ProductManagement.DAL.Constants;
using ProductManagement.DAL.DTOs;
using ProductManagement.DAL.Models;
using ProductManagement.DAL.Repositories.ProductRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly IOptions<AppSettings> settings;
        private readonly IAuditService auditService;
        private readonly IEmployeeService employeeService;

        public ProductService(IProductRepository productRepository,
                              IOptions<AppSettings> settings,
                              IAuditService auditService,
                              IEmployeeService employeeService)
        {
            this.productRepository = productRepository;
            this.settings = settings;
            this.auditService = auditService;
            this.employeeService = employeeService;
        }

        public async Task<ResponseMessage> CreateProduct(Product product)
        {
            try
            {
                var existingRecord = productRepository.GetProductByCodeOrName(product.ProductCode, product.ProductName);

                if (existingRecord != null)
                    return new ResponseMessage(false, ConstantsValues.ProductAlreadyExists);

                var results = await productRepository.SaveProduct(product);
                if (results > 0)
                {
                    return new ResponseMessage(true);
                }
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredCreatingProduct);
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ConstantsValues.ErrorOccurredCreatingProduct} - {ex.Message}",
                    Payload = JsonConvert.SerializeObject(product),
                    Source = nameof(CreateProduct),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredCreatingProduct);
            }
        }

        public async Task<ResponseMessage> UpdateProduct(Product product)
        {
            try
            {
                var existingRecord = await productRepository.GetProductById(product.ProductId);

                if (existingRecord == null)
                    return new ResponseMessage(false, ConstantsValues.ProductNotFound);

                existingRecord.ExpiryDate = product.ExpiryDate;
                existingRecord.UpdatedDate = DateTime.Now;
                existingRecord.Price = product.Price;
                existingRecord.ProductName = product.ProductName;
                existingRecord.ProductDescription = product.ProductDescription;
                existingRecord.ProductCode = product.ProductCode;
                existingRecord.IsActive = product.IsActive;
                existingRecord.Quantity = product.Quantity;
                existingRecord.EmployeeId = product.EmployeeId;

                var results = await productRepository.UpdateProduct(existingRecord);
                if (results > 0)
                {
                    return new ResponseMessage(true);
                }
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredUpdatingProduct);
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ConstantsValues.ErrorOccurredUpdatingProduct} - {ex.Message}",
                    Payload = JsonConvert.SerializeObject(product),
                    Source = nameof(UpdateProduct),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredUpdatingProduct);
            }
        }

        public async Task<ResponseMessage> DeleteProduct(int productId)
        {
            try
            {
                var existingRecord = await productRepository.GetProductById(productId);

                if (existingRecord == null)
                    return new ResponseMessage(false, ConstantsValues.ProductNotFound);

                existingRecord.UpdatedDate = DateTime.Now;
                existingRecord.IsActive = false;

                var results = await productRepository.DeleteProduct(existingRecord);
                if (results > 0)
                {
                    return new ResponseMessage(true);
                }
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredDeletingProduct);
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ConstantsValues.ErrorOccurredDeletingProduct} - {ex.Message}",
                    Payload = productId.ToString(),
                    Source = nameof(DeleteProduct),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredDeletingProduct);
            }
        }

        public async Task<List<ProductDto>> GetActiveProducts()
        {
            try
            {
                var products = await productRepository.GetActiveProducts();
                var updatedProducts = UpdateProductDiscountPrice(products);
                return updatedProducts;
            }
            catch(Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = "",
                    Source = nameof(GetActiveProducts),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });

                return new List<ProductDto>();
            }
        }

        public async Task<List<ProductDto>> SearchActiveProducts(string searchField)
        {
            try
            {
                var employees = await employeeService.GetAllEmployees();
                var products = new List<Product>();
                if (string.IsNullOrEmpty(searchField))
                {
                    products = await productRepository.GetActiveProducts();
                }
                else
                {
                    products = await productRepository.SearchActiveProduct(searchField);
                }

                var updatedProducts = UpdateProductDiscountPrice(products);
                updatedProducts?.ForEach(p => p.Employee = employees.Where(e => e.EmployeeId == p.EmployeeId).Select(e => $"{e.Firstname} {e.LastName}").FirstOrDefault());
                return updatedProducts;
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = searchField,
                    Source = nameof(SearchActiveProducts),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });

                return new List<ProductDto>();
            }
        }

        public async Task<List<ProductDto>> SearchInActiveProducts(string searchField)
        {
            try
            {
                var employees = await employeeService.GetAllEmployees();
                var products = new List<Product>();
                if (string.IsNullOrEmpty(searchField))
                {
                    products = await productRepository.GetInActiveProducts();
                }
                else
                {
                    products = await productRepository.SearchInActiveProduct(searchField);
                }

                var updatedProducts = UpdateProductDiscountPrice(products);
                updatedProducts?.ForEach(p => p.Employee = employees.Where(e => e.EmployeeId == p.EmployeeId).Select(e => $"{e.Firstname} {e.LastName}").FirstOrDefault());
                return updatedProducts;
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = searchField,
                    Source = nameof(SearchInActiveProducts),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });

                return new List<ProductDto>();
            }
        }

        public async Task<Product> GetProductById(int productId)
        {
            try
            {
                return await productRepository.GetProductById(productId);
            }
            catch(Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = productId.ToString(),
                    Source = nameof(GetProductById),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });

                return null;
            }
        }

        public ProductDto GetProductByCode(string code)
        {
            try
            {
                var product = productRepository.GetProductByCode(code);
                var updatedProduct = ComputeDiscountedPriceIfApplicableForProduct(product);
                return updatedProduct;
            }
            catch(Exception ex)
            {
                auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = code,
                    Source = nameof(GetProductByCode),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });

                return null;
            }
        }

        public ProductDto GetProductByName(string name)
        {
            try
            {
                var product = productRepository.GetProductsByName(name);
                var updatedProduct = ComputeDiscountedPriceIfApplicableForProduct(product);
                return updatedProduct;
            }
            catch(Exception ex)
            {
                auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = name,
                    Source = nameof(GetProductByName),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });

                return null;
            }
        }

        private List<ProductDto> UpdateProductDiscountPrice(List<Product> products)
        {
            try
            {
                var updatedProducts = new List<ProductDto>();
                foreach (var product in products)
                {
                    updatedProducts.Add(ComputeDiscountedPriceIfApplicableForProduct(product));
                }

                return updatedProducts;
            }
            catch
            {
                throw;
            }
        }

        private ProductDto ComputeDiscountedPriceIfApplicableForProduct(Product product)
        {
            try
            {
                TimeSpan timeSpan = DateTime.Now.Subtract((DateTime)product.ExpiryDate);
                var totalWeeks = (int)timeSpan.Days / settings.Value.NumberOfDaysToNotify;
                decimal discountedPrice = 0;

                if (totalWeeks >= ConstantsValues.TotalWeeksMoreThan)
                {
                    discountedPrice = product.Price * settings.Value.DiscountPercentage;
                }

                return new ProductDto(product.ProductId, product.ProductName, product.ProductDescription,
                                      product.ProductCode, product.Quantity, product.Price, discountedPrice,
                                      product.ExpiryDate, product.IsActive, product.CreatedDate, product.UpdatedDate,
                                      product.EmployeeId);
            }
            catch
            {
                throw;
            }
        }
    }
}
