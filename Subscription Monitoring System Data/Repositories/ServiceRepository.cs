using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Data.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly DataContext _context;
        public ServiceRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetActiveList()
        {
            try
            {
                return await _context.Services!.Where(p => p.IsActive).Include(p => p.ServiceType).Include(p => p.Subscriptions).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Service?> GetActive(int id)
        {
            try
            {
                return await _context.Services!.Where(p => p.Id == id && p.IsActive).Include(p => p.ServiceType).Include(p => p.Subscriptions).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Service?> GetInactive(int id)
        {
            try
            {
                return await _context.Services!.Where(p => p.Id == id && !p.IsActive).Include(p => p.ServiceType).Include(p => p.Subscriptions).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Service?> GetActive(string name)
        {
            try
            {
                return await _context.Services!.Where(p => p.Name == name && p.IsActive).Include(p => p.ServiceType).Include(p => p.Subscriptions).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Service>> GetList(List<int> ids)
        {
            try
            {
                return await _context.Services!.Where(p => ids.Contains(p.Id)).Include(p => p.ServiceType).Include(p => p.Subscriptions).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Service>> GetList()
        {
            try
            {
                return await _context.Services!.Include(p => p.ServiceType).Include(p => p.Subscriptions).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(Service service)
        {
            try
            {
                service.IsActive = true;
                await _context.Services!.AddAsync(service);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task Update(Service service)
        {
            try
            {
                Service? serviceUpdate = await GetActive(service.Id);
                serviceUpdate!.Name = service.Name;
                serviceUpdate.Description = service.Description;
                serviceUpdate.Price = service.Price;
                serviceUpdate.ServiceTypeId = service.ServiceTypeId;
                _context.Services!.Update(serviceUpdate);
                await _context.SaveChangesAsync();
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
                Service? service = await GetInactive(id);
                _context.Services!.Remove(service!);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SoftDelete(int id)
        {
            try
            {
                Service? service = await GetActive(id);
                service!.IsActive = false;
                _context.Services!.Update(service);
                await _context.SaveChangesAsync();
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
                List<Service> services = await GetList(ids);
                _context.Services!.RemoveRange(services);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SoftDelete(List<int> ids)
        {
            try
            {
                List<Service> services = await GetList(ids);
                services.ForEach(p => p.IsActive = false);
                _context.Services!.UpdateRange(services);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Restore(int id)
        {
            try
            {
                Service? service = await GetInactive(id);
                service!.IsActive = true;
                _context.Services!.Update(service);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Restore(List<int> ids)
        {
            try
            {
                List<Service> services = await GetList(ids);
                services.ForEach(p => p.IsActive = true);
                _context.Services!.UpdateRange(services);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> ServiceExists(Service service)
        {
            try
            {
                return await _context.Services!.AnyAsync(p => (p.Name == service.Name || p.Description == service.Description) && p.Id != service.Id && p.IsActive);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
