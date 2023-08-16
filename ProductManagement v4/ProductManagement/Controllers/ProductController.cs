using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagement.BLL.Services.EmployeeService;
using ProductManagement.BLL.Services.ProductService;
using ProductManagement.DAL.DTOs;
using ProductManagement.DAL.Models;
using ProductManagement.DAL.Repositories.ProductRepository;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductManagement.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly IEmployeeService employeeService;
        public ProductController(IProductService productService, IEmployeeService employeeService)
        {
            this.productService = productService;
            this.employeeService = employeeService;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index(string searchField)
        {
            ViewData["GetProducts"] = searchField;

            var productsViewModel = new List<ProductsViewModel>();

            var products = await productService.SearchActiveProducts(searchField);
            foreach (var product in products)
            {
                var productModel = new ProductsViewModel()
                {
                    CreatedDate = product.CreatedDate,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    DiscountedPrice = product.DiscountedPrice,
                    ExpiryDate = product.ExpiryDate,
                    IsActive = product.IsActive,
                    Price = product.Price,
                    ProductCode = product.ProductCode,
                    ProductDescription = product.ProductDescription,
                    Quantity = product.Quantity,
                };
                productsViewModel.Add(productModel);
            }

            return View(productsViewModel);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProductViewModel addProductRequest)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var email = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var employee = employeeService.GetEmployeeByEmail(email);

                var product = new Product()
                {
                    CreatedDate = DateTime.Now,
                    ExpiryDate = addProductRequest.ExpiryDate,
                    IsActive = true,
                    Price = addProductRequest.Price,
                    ProductCode = addProductRequest.ProductCode,
                    ProductName = addProductRequest.ProductName,
                    ProductDescription = addProductRequest.ProductDescription,
                    Quantity = addProductRequest.Quantity,
                    UpdatedDate = null,
                    EmployeeId = employee.EmployeeId
                };

                var response = await productService.CreateProduct(product);
                if (!response.IsSuccessful)
                {
                    ViewData["ValidateMessage"] = response.Message;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await productService.GetProductById(id);

            if (product == null)
                return RedirectToAction("Index");

            var viewModel = new EditProductViewModel()
            {
                ExpiryDate = product.ExpiryDate,
                IsActive = product.IsActive,
                Price = product.Price,
                ProductCode = product.ProductCode,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Quantity = product.Quantity,
                ProductId = product.ProductId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductViewModel editProductRequest)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var email = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                var employee = employeeService.GetEmployeeByEmail(email);

                var product = new Product()
                {
                    ExpiryDate = editProductRequest.ExpiryDate,
                    IsActive = true,
                    Price = editProductRequest.Price,
                    ProductCode = editProductRequest.ProductCode,
                    ProductName = editProductRequest.ProductName,
                    ProductDescription = editProductRequest.ProductDescription,
                    Quantity = editProductRequest.Quantity,
                    ProductId = editProductRequest.ProductId,
                    EmployeeId = employee.EmployeeId
                };

                var response = await productService.UpdateProduct(product);
                if (!response.IsSuccessful)
                {
                    ViewData["ValidateMessage"] = response.Message;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditProductViewModel editProduct)
        {
            var response = await productService.DeleteProduct(editProduct.ProductId);
            if (!response.IsSuccessful)
            {
                ViewData["ValidateMessage"] = response.Message;
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> InActiveProductReport(string searchField)
        {
            ViewData["GetProducts"] = searchField;

            var productsViewModel = new List<ProductsReportViewModel>();

            var products = await productService.SearchInActiveProducts(searchField);
            foreach (var product in products)
            {
                var productModel = new ProductsReportViewModel()
                {
                    CreatedDate = product.CreatedDate,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    DiscountedPrice = product.DiscountedPrice,
                    ExpiryDate = product.ExpiryDate,
                    IsActive = product.IsActive,
                    Price = product.Price,
                    ProductCode = product.ProductCode,
                    ProductDescription = product.ProductDescription,
                    Quantity = product.Quantity,
                    Employee = product.Employee,
                    EmployeeId = product.EmployeeId
                };
                productsViewModel.Add(productModel);
            }

            return View(productsViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ActiveProductReport(string searchField)
        {
            ViewData["GetProducts"] = searchField;

            var productsViewModel = new List<ProductsReportViewModel>();

            var products = await productService.SearchActiveProducts(searchField);
            foreach (var product in products)
            {
                var productModel = new ProductsReportViewModel()
                {
                    CreatedDate = product.CreatedDate,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    DiscountedPrice = product.DiscountedPrice,
                    ExpiryDate = product.ExpiryDate,
                    IsActive = product.IsActive,
                    Price = product.Price,
                    ProductCode = product.ProductCode,
                    ProductDescription = product.ProductDescription,
                    Quantity = product.Quantity,
                    Employee = product.Employee,
                    EmployeeId = product.EmployeeId
                };
                productsViewModel.Add(productModel);
            }

            return View(productsViewModel);
        }
    }
}
