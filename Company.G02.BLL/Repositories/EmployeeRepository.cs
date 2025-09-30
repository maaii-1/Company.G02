using Company.G02.BLL.Interfaces;
using Company.G02.DAL.Data.Contexts;
using Company.G02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;
        #region -
        //private readonly CompanyDbContext _context;

        //public EmployeeRepository(CompanyDbContext context)
        //{
        //    _context = context;
        //}
        //public IEnumerable<Employee> GetAll()
        //{
        //    return _context.Employees.ToList();
        //}


        //public Employee? GetById(int id)
        //{
        //    return _context.Employees.Find(id);
        //}


        //public int Add(Employee employee)
        //{
        //    _context.Employees.Add(employee);
        //    return _context.SaveChanges();
        //}


        //public int Update(Employee employee)
        //{
        //    _context.Employees.Update(employee);
        //    return _context.SaveChanges();
        //}


        //public int Delete(Employee employee)
        //{
        //    _context.Employees.Remove(employee);
        //    return _context.SaveChanges();
        //} 
        #endregion
        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetByNameAsync(string name)
        {
            return await _context.Employees.Include(E => E.WorkFor).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
