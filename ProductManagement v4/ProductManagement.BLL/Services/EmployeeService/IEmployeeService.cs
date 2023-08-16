using ProductManagement.DAL.DTOs;
using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.BLL.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Access AuthenticateEmployee(string email, string password);
        Task<ResponseMessage> ChangePassword(ChangePassword changePassword);
        Task<ResponseMessage> CreateEmployee(Employee employee);
        Task<ResponseMessage> DeleteEmployee(int employeeId);
        Task<List<Employee>> GetActiveEmployees();
        Task<List<Employee>> GetAllEmployees();
        Employee GetEmployeeByEmail(string email);
        Task<Employee> GetEmployeeById(int employeeId);
        Task<List<Employee>> SearchActiveEmployees(string searchField);
        Task<List<Employee>> SearchInActiveEmployees(string searchField);
        Task<ResponseMessage> UpdateEmployee(Employee employee);
    }
}
