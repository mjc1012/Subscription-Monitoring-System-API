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
    public class ServiceTypeRepository : IServiceTypeRepository
    {
        private readonly DataContext _context;
        public ServiceTypeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceType?> Get(int id)
        {
            try
            {
                return await _context.ServiceTypes!.Where(p => p.Id == id ).Include(p => p.Services).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceType?> Get(string name)
        {
            try
            {
                return await _context.ServiceTypes!.Where(p => p.Name == name).Include(p => p.Services).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceType>> GetList()
        {
            try
            {
                return await _context.ServiceTypes!.Include(p => p.Services).ToListAsync();  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ServiceTypeExists(string name)
        {
            try
            {
                return await _context.ServiceTypes!.AnyAsync(p => p.Name == name);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
