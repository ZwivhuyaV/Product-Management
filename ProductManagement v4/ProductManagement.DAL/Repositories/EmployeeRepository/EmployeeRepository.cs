using Microsoft.EntityFrameworkCore;
using ProductManagement.DAL.DataContext;
using ProductManagement.DAL.DTOs;
using ProductManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Repositories.EmployeeRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ProductManagementDBContext dbcontext;
        public EmployeeRepository(ProductManagementDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<List<Employee>> GetActiveEmployees()
        {
            try
            {
                return await dbcontext.Set<Employee>().Where(p => p.IsActive).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            try
            {
                return await dbcontext.Set<Employee>().ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Employee>> GetInActiveEmployees()
        {
            try
            {
                return await dbcontext.Set<Employee>().Where(p => !p.IsActive).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> SaveEmployee(Employee employee)
        {
            try
            {
                var savedRecord = dbcontext.Add(employee);
                return await dbcontext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UpdateEmployee(Employee employee)
        {
            try
            {
                dbcontext.Update(employee);
                return await dbcontext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> DeleteEmployee(Employee employee)
        {
            try
            {
                dbcontext.Update(employee);
                return await dbcontext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Employee>> SearchActiveEmployees(string searchField)
        {
            try
            {
                return await dbcontext.Set<Employee>().Where(e => e.IsActive &&
                                                                  (e.LastName.Contains(searchField) ||
                                                                  e.Firstname.Contains(searchField) ||
                                                                  e.Email.Contains(searchField))).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Employee>> SearchInActiveEmployee(string searchField)
        {
            try
            {
                return await dbcontext.Set<Employee>().Where(e => !e.IsActive &&
                                                                  (e.LastName.Contains(searchField) ||
                                                                  e.Firstname.Contains(searchField) ||
                                                                  e.Email.Contains(searchField))).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Employee> GetEmployeeById(int employeeId)
        {
            try
            {
                return await dbcontext.Set<Employee>().Where(p => p.IsActive && p.EmployeeId == employeeId).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        public Employee GetEmployeeByEmail(string email)
        {
            try
            {
                return dbcontext.Set<Employee>().Where(e => e.IsActive && e.Email.ToLower() == email.ToLower()).FirstOrDefault();
            }
            catch
            {
                throw;
            }
        }
    }
}
