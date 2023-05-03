using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext _context;
        public DepartmentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Department> Get(int id)
        {
            try
            {
                return await _context.Departments.Where(p => p.Id == id).Include(p => p.Users).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department> Get(string name)
        {
            try
            {
                return await _context.Departments.Where(p => p.Name == name).Include(p => p.Users).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Department>> GetList()
        {
            try
            {
                return await _context.Departments.Include(p => p.Users).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
