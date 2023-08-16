using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProductManagement.BLL.Extensions;
using ProductManagement.BLL.Services.AuditService;
using ProductManagement.DAL.Common;
using ProductManagement.DAL.Constants;
using ProductManagement.DAL.DTOs;
using ProductManagement.DAL.Enums;
using ProductManagement.DAL.Models;
using ProductManagement.DAL.Repositories.EmployeeRepository;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IAuditService auditService;
        private readonly IOptions<AppSettings> settings;

        public EmployeeService(IEmployeeRepository employeeRepository,
                               IAuditService auditService,
                               IOptions<AppSettings> settings)
        {
            this.employeeRepository = employeeRepository;
            this.auditService = auditService;
            this.settings = settings;
        }

        public async Task<List<Employee>> GetActiveEmployees()
        {
            try
            {
                return await employeeRepository.GetActiveEmployees();
            }
            catch(Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = "",
                    Source = nameof(GetActiveEmployees),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new List<Employee>();
            }
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                return await employeeRepository.GetAllEmployees();
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = "",
                    Source = nameof(GetAllEmployees),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new List<Employee>();
            }
        }

        public async Task<ResponseMessage> CreateEmployee(Employee employee)
        {
            try
            {
                var existingRecord = employeeRepository.GetEmployeeByEmail(employee.Email);

                if (existingRecord != null)
                    return new ResponseMessage(false, ConstantsValues.UserAlreadyExists);

                employee.Password = SecretHasher.Hash(employee.Password);
                var results = await employeeRepository.SaveEmployee(employee);
                if (results > 0)
                {
                    return new ResponseMessage(true);
                }
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredCreatingUser);
            }
            catch(Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ConstantsValues.ErrorOccurredCreatingUser} - {ex.Message}",
                    Payload = JsonConvert.SerializeObject(employee),
                    Source = nameof(CreateEmployee),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredCreatingUser);
            }
        }

        public async Task<ResponseMessage> UpdateEmployee(Employee employee)
        {
            try
            {
                var existingRecord = await employeeRepository.GetEmployeeById(employee.EmployeeId);

                if (existingRecord == null)
                    return new ResponseMessage(false, ConstantsValues.UserNotFound);

                existingRecord.UpdatedDate = DateTime.Now;
                existingRecord.Firstname = employee.Firstname;
                existingRecord.LastName = employee.LastName;
                existingRecord.IsActive = employee.IsActive;
                existingRecord.Email = employee.Email;
                existingRecord.RoleId = employee.RoleId;

                var results = await employeeRepository.UpdateEmployee(existingRecord);
                if (results > 0)
                {
                    return new ResponseMessage(true);
                }
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredUpdatingUser);
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ConstantsValues.ErrorOccurredUpdatingUser} - {ex.Message}",
                    Payload = JsonConvert.SerializeObject(employee),
                    Source = nameof(UpdateEmployee),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredUpdatingUser);
            }
        }

        public async Task<ResponseMessage> DeleteEmployee(int employeeId)
        {
            try
            {
                var existingRecord = await employeeRepository.GetEmployeeById(employeeId);

                if (existingRecord == null)
                    return new ResponseMessage(false, ConstantsValues.UserNotFound);

                existingRecord.UpdatedDate = DateTime.Now;
                existingRecord.IsActive = false;

                var results = await employeeRepository.DeleteEmployee(existingRecord);
                if (results > 0)
                {
                    return new ResponseMessage(true);
                }
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredDeletingUser);
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ConstantsValues.ErrorOccurredDeletingUser} - {ex.Message}",
                    Payload = employeeId.ToString(),
                    Source = nameof(DeleteEmployee),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredDeletingUser);
            }
        }

        public async Task<List<Employee>> SearchActiveEmployees(string searchField)
        {
            try
            {
                var products = new List<Employee>();
                if (string.IsNullOrEmpty(searchField))
                {
                    return await employeeRepository.GetActiveEmployees();
                }
                
                return await employeeRepository.SearchActiveEmployees(searchField);
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = searchField,
                    Source = nameof(SearchActiveEmployees),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new List<Employee>();
            }
        }

        public async Task<List<Employee>> SearchInActiveEmployees(string searchField)
        {
            try
            {
                var products = new List<Employee>();
                if (string.IsNullOrEmpty(searchField))
                {
                    return await employeeRepository.GetInActiveEmployees();
                }

                return await employeeRepository.SearchInActiveEmployee(searchField);
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = searchField,
                    Source = nameof(SearchInActiveEmployees),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new List<Employee>();
            }
        }

        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            try
            {
                return await employeeRepository.GetEmployeeById(employeeId);
            }
            catch(Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = employeeId.ToString(),
                    Source = nameof(GetEmployeeById),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return null;
            }
        }

        public Employee GetEmployeeByEmail(string email)
        {
            try
            {
                return employeeRepository.GetEmployeeByEmail(email);
            }
            catch(Exception ex)
            {
                auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = email,
                    Source = nameof(GetEmployeeByEmail),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });

                return null;
            }
        }

        public async Task<ResponseMessage> ChangePassword(ChangePassword changePassword)
        {
            try
            {
                var existingRecord = employeeRepository.GetEmployeeByEmail(changePassword.Email);

                if (existingRecord == null)
                    return new ResponseMessage(false, ConstantsValues.UserNotFound);

                existingRecord.UpdatedDate = DateTime.Now;
                existingRecord.Password = SecretHasher.Hash(changePassword.Password);

                var results = await employeeRepository.UpdateEmployee(existingRecord);
                if (results > 0)
                {
                    return new ResponseMessage(true);
                }
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredChangingPassword);
            }
            catch (Exception ex)
            {
                await auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ConstantsValues.ErrorOccurredChangingPassword} - {ex.Message}",
                    Payload = JsonConvert.SerializeObject(changePassword),
                    Source = nameof(ChangePassword),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return new ResponseMessage(false, ConstantsValues.ErrorOccurredChangingPassword);
            }
        }

        public Access AuthenticateEmployee(string email, string password)
        {
            try
            {
                var employee = employeeRepository.GetEmployeeByEmail(email);
                if (employee != null)
                {
                    var isPasswordCorrect = IsAdminInitialAuth(email, password);
                    if (!isPasswordCorrect)
                    {
                        isPasswordCorrect = SecretHasher.Verify(password, employee.Password);
                    }

                    if (isPasswordCorrect)
                    {
                        return new Access()
                        {
                            Email = employee.Email,
                            Role = Enum.GetName(typeof(Roles), employee.RoleId),
                            LastName = employee.LastName,
                            FirstName = employee.Firstname
                        };
                    }
                }

                return null;
            }
            catch(Exception ex) 
            {
                auditService.CreateAudit(new AuditEvent()
                {
                    CreatedDate = DateTime.Now,
                    Description = $"{ex.Message}",
                    Payload = email,
                    Source = nameof(AuthenticateEmployee),
                    IsActive = true,
                    Type = ConstantsValues.Error
                });
                return null;
            }
        }

        private bool IsAdminInitialAuth(string email, string password)
        {
            var isAdminInitialAuth = settings.Value.AdminDefaultEmail.ToLower() == email.ToLower() &&
                                     settings.Value.AdminDefaultPassword == password;
            return isAdminInitialAuth;
        }
    }
}
