using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Repositories.EmployeeRepository
{
    public interface IEmployeeRepository
    {
        Task<int> DeleteEmployee(Employee employee);
        Task<List<Employee>> GetActiveEmployees();
        Task<List<Employee>> GetAllEmployees();
        Employee GetEmployeeByEmail(string email);
        Task<Employee> GetEmployeeById(int employeeId);
        Task<List<Employee>> GetInActiveEmployees();
        Task<int> SaveEmployee(Employee employee);
        Task<List<Employee>> SearchActiveEmployees(string searchField);
        Task<List<Employee>> SearchInActiveEmployee(string searchField);
        Task<int> UpdateEmployee(Employee employee);
    }
}
