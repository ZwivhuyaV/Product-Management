using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductManagement.BLL.Services.EmployeeService;
using ProductManagement.BLL.Services.ProductService;
using ProductManagement.DAL.Common;
using ProductManagement.DAL.DTOs;
using ProductManagement.DAL.Enums;
using ProductManagement.DAL.Models;
using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductManagement.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IOptions<AppSettings> settings;

        public EmployeeController(IEmployeeService employeeService, IOptions<AppSettings> settings)
        {
            this.employeeService = employeeService;
            this.settings = settings;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(string searchField)
        {
            ViewData["GetEmployees"] = searchField;

            var employeesViewModel = new List<EmployeesViewModel>();

            var employees = await employeeService.SearchActiveEmployees(searchField);
            foreach (var employee in employees)
            {
                var employeeModel = new EmployeesViewModel()
                {
                    CreatedDate = employee.CreatedDate,
                    EmployeeId = employee.EmployeeId,
                    Firstname = employee.Firstname,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    RoleId = employee.RoleId,
                    IsActive = employee.IsActive,
                    IsAdmin = employee.RoleId == (int)Roles.Admin,
                };
                employeesViewModel.Add(employeeModel);
            }

            return View(employeesViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await employeeService.GetEmployeeById(id);

            if (employee == null)
                return RedirectToAction("Index");

            var viewModel = new EditEmployeeViewModel()
            {
                CreatedDate = employee.CreatedDate,
                EmployeeId = employee.EmployeeId,
                Firstname = employee.Firstname,
                LastName = employee.LastName,
                Email = employee.Email,
                RoleId = employee.RoleId,
                IsActive = employee.IsActive,
                IsAdmin = employee.RoleId == (int)Roles.Admin,
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditEmployeeViewModel editEmployeeRequest)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    EmployeeId = editEmployeeRequest.EmployeeId,
                    IsActive = true,
                    Email = editEmployeeRequest.Email,
                    RoleId = editEmployeeRequest.IsAdmin ? (int)Roles.Admin : (int)Roles.Normal,
                    Firstname = editEmployeeRequest.Firstname,
                    LastName = editEmployeeRequest.LastName,
                    UpdatedDate = DateTime.Now
                };

                var response = await employeeService.UpdateEmployee(employee);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(EditEmployeeViewModel editEmployee)
        {
            var response = await employeeService.DeleteEmployee(editEmployee.EmployeeId);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var employee = await employeeService.GetEmployeeById(id);

            if (employee == null)
                return RedirectToAction("Index");

            var viewModel = new EditEmployeeViewModel()
            {
                CreatedDate = employee.CreatedDate,
                EmployeeId = employee.EmployeeId,
                Firstname = employee.Firstname,
                LastName = employee.LastName,
                Email = employee.Email,
                RoleId = employee.RoleId,
                IsActive = employee.IsActive,
                IsAdmin = employee.RoleId == (int)Roles.Admin,
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(EditEmployeeViewModel editEmployee)
        {
            if (ModelState.IsValid)
            {
                var changePassword = new ChangePassword()
                {
                    Password = editEmployee.IsAdmin ? settings.Value.AdminDefaultPassword : settings.Value.DefaultPassword,
                    Email = editEmployee.Email
                };

                var response = await employeeService.ChangePassword(changePassword);
                if (!response.IsSuccessful)
                {
                    ViewData["ValidateMessage"] = response.Message;
                }
                else
                {
                    ViewData["ValidateMessage"] = "Password successfully reset.";
                }

                return View();
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    CreatedDate = DateTime.Now,
                    Email = addEmployeeRequest.Email,
                    RoleId = addEmployeeRequest.IsAdmin ? (int)Roles.Admin : (int)Roles.Normal,
                    IsActive = true,
                    Firstname = addEmployeeRequest.Firstname,
                    LastName = addEmployeeRequest.LastName,
                    Password = settings.Value.DefaultPassword,
                    UpdatedDate = null,
                };

                var response = await employeeService.CreateEmployee(employee);
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
        public IActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Password(PasswordViewModel passwordModel)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var email = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                var oldPasswordAuthorization = employeeService.AuthenticateEmployee(email, passwordModel.CurrentPassword);
                if(oldPasswordAuthorization == null)
                {
                    ViewData["ValidateMessage"] = "Current password incorrect.";
                    return View();
                }

                var changePassword = new ChangePassword()
                {
                    Password = passwordModel.Password,
                    Email = email
                };

                var response = await employeeService.ChangePassword(changePassword);
                if (!response.IsSuccessful)
                {
                    ViewData["ValidateMessage"] = response.Message;
                }
                else
                {
                    ViewData["ValidateMessage"] = "Password successfully changed.";
                }

                return View();
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InActiveEmployeeReport(string searchField)
        {
            ViewData["GetEmployees"] = searchField;

            var employeesViewModel = new List<EmployeesReportViewModel>();

            var employees = await employeeService.SearchInActiveEmployees(searchField);
            foreach (var employee in employees)
            {
                var employeeModel = new EmployeesReportViewModel()
                {
                    CreatedDate = employee.CreatedDate,
                    EmployeeId = employee.EmployeeId,
                    Firstname = employee.Firstname,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    RoleId = employee.RoleId,
                    IsActive = employee.IsActive,
                    Role = Enum.GetName(typeof(Roles), employee.RoleId),
                    IsAdmin = employee.RoleId == (int)Roles.Admin,
                };
                employeesViewModel.Add(employeeModel);
            }

            return View(employeesViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveEmployeeReport(string searchField)
        {
            ViewData["GetEmployees"] = searchField;

            var employeesViewModel = new List<EmployeesReportViewModel>();

            var employees = await employeeService.SearchActiveEmployees(searchField);
            foreach (var employee in employees)
            {
                var employeeModel = new EmployeesReportViewModel()
                {
                    CreatedDate = employee.CreatedDate,
                    EmployeeId = employee.EmployeeId,
                    Firstname = employee.Firstname,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    RoleId = employee.RoleId,
                    IsActive = employee.IsActive,
                    Role = Enum.GetName(typeof(Roles), employee.RoleId),
                    IsAdmin = employee.RoleId == (int)Roles.Admin,
                };
                employeesViewModel.Add(employeeModel);
            }

            return View(employeesViewModel);
        }
    }
}
