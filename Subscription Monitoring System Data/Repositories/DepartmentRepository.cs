using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System_Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DataContext _context;
        public DepartmentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Department?> Get(int id)
        {
            try
            {
                return await _context.Departments!.Where(p => p.Id == id).Include(p => p.Users).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Department?> Get(string name)
        {
            try
            {
                return await _context.Departments!.Where(p => p.Name == name).Include(p => p.Users).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(Department department)
        {
            try
            {
                await _context.Departments!.AddAsync(department);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(Department department)
        {
            try
            {
                Department? departmentUpdate = await Get(department.Id);
                departmentUpdate!.Name = department.Name;
                _context.Departments!.Update(departmentUpdate);
                await _context.SaveChangesAsync();
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
                return await _context.Departments!.Include(p => p.Users).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task HardDelete(int id)
        {
            try
            {
                Department? department = await Get(id);
                _context.Departments!.Remove(department!);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Department>> GetList(List<int> ids)
        {
            try
            {
                return await _context.Departments!.Where(p => ids.Contains(p.Id)).Include(p => p.Users).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task HardDelete(List<int> ids)
        {
            try
            {
                List<Department> departments = await GetList(ids);
                _context.Departments!.RemoveRange(departments);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DepartmentExists(Department department)
        {
            try
            {
                return await _context.Departments!.AnyAsync(p => p.Name == department.Name);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
