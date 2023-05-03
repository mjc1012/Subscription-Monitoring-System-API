using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
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
                return await _context.Services.Where(p => p.IsActive == true).Include(p => p.ServiceType).Include(p => p.Subscriptions).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Service> GetActive(int id)
        {
            try
            {
                return await _context.Services.Where(p => p.Id == id && p.IsActive == true).Include(p => p.ServiceType).Include(p => p.Subscriptions).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Service> GetInactive(int id)
        {
            try
            {
                return await _context.Services.Where(p => p.Id == id && p.IsActive == false).Include(p => p.ServiceType).Include(p => p.Subscriptions).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Service> GetActive(string name)
        {
            try
            {
                return await _context.Services.Where(p => p.Name == name && p.IsActive == true).Include(p => p.ServiceType).Include(p => p.Subscriptions).FirstOrDefaultAsync();
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
                return await _context.Services.Where(p => ids.Contains(p.Id)).Include(p => p.ServiceType).Include(p => p.Subscriptions).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Service>> GetList(ServiceFilterDto filter)
        {
            try
            {
                if (filter.Page == 0) filter.Page = 1;

                List<Service> services = await _context.Services.Where(p => (filter.Id == 0 || p.Id == filter.Id) && (string.IsNullOrEmpty(filter.Name) || p.Name == filter.Name) && 
                (string.IsNullOrEmpty(filter.Description) || p.Description == filter.Description) && (filter.Price == 0 || p.Price == filter.Price) &&
                (string.IsNullOrEmpty(filter.ServiceTypeName) || p.ServiceType.Name == filter.ServiceTypeName) && p.IsActive == filter.IsActive)
                    .Include(p => p.ServiceType).Include(p => p.Subscriptions).ToListAsync();

                return (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.Equals(SortDirectionConstants.Descending)) ? SortDescending(filter.SortBy, services) : SortAscending(filter.SortBy, services);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Service> SortAscending(string sortBy, List<Service> services)
        {
            return sortBy switch
            {
                ServiceConstants.HeaderId => services.OrderBy(p => p.Id).ToList(),
                ServiceConstants.HeaderName => services.OrderBy(p => p.Name).ToList(),
                ServiceConstants.HeaderDescription => services.OrderBy(p => p.Description).ToList(),
                ServiceConstants.HeaderPrice => services.OrderBy(p => p.Price).ToList(),
                ServiceConstants.HeaderServiceTypeName => services.OrderBy(p => p.ServiceType.Name).ToList(),
                _ => services.OrderBy(p => p.Id).ToList(),
            };
        }

        public List<Service> SortDescending(string sortBy, List<Service> services)
        {
            return sortBy switch
            {
                ServiceConstants.HeaderId => services.OrderByDescending(p => p.Id).ToList(),
                ServiceConstants.HeaderName => services.OrderByDescending(p => p.Name).ToList(),
                ServiceConstants.HeaderDescription => services.OrderByDescending(p => p.Description).ToList(),
                ServiceConstants.HeaderPrice => services.OrderByDescending(p => p.Price).ToList(),
                ServiceConstants.HeaderServiceTypeName => services.OrderByDescending(p => p.ServiceType.Name).ToList(),
                _ => services.OrderByDescending(p => p.Id).ToList(),
            };
        }

        public async Task Create(Service service)
        {
            try
            {
                service.IsActive = true;
                await _context.Services.AddAsync(service);
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
                var serviceUpdate = await GetActive(service.Id);
                serviceUpdate.Name = service.Name;
                serviceUpdate.Description = service.Description;
                serviceUpdate.Price = service.Price;
                serviceUpdate.ServiceTypeId = service.ServiceTypeId;
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
                Service service = await GetInactive(id);
                _context.Services.Remove(service);
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
                Service service = await GetActive(id);
                service.IsActive = false;
                _context.Services.Update(service);
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
                _context.Services.RemoveRange(services);
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
                _context.Services.UpdateRange(services);
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
                Service service = await GetInactive(id);
                service.IsActive = true;
                _context.Services.Update(service);
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
                _context.Services.UpdateRange(services);
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
                return await _context.Services.AnyAsync(p => (p.Name == service.Name || p.Description == service.Description) && p.Id != service.Id && p.IsActive == true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
