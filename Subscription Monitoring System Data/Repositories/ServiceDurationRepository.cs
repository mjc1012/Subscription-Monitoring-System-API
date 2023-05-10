using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System_Data.Repositories
{
    public class ServiceDurationRepository : IServiceDurationRepository
    {
        private readonly DataContext _context;
        public ServiceDurationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceDuration?> Get(int id)
        {
            try
            {
                return await _context.ServiceDurations!.Where(p => p.Id == id ).Include(p => p.Services).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceDuration?> Get(string name)
        {
            try
            {
                return await _context.ServiceDurations!.Where(p => p.Name == name).Include(p => p.Services).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceDuration>> GetList()
        {
            try
            {
                return await _context.ServiceDurations!.Include(p => p.Services).ToListAsync();  
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(ServiceDuration serviceDuration)
        {
            try
            {
                await _context.ServiceDurations!.AddAsync(serviceDuration);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(ServiceDuration serviceDuration)
        {
            try
            {
                ServiceDuration? serviceDurationUpdate = await Get(serviceDuration.Id);
                serviceDurationUpdate!.Name = serviceDuration.Name;
                serviceDurationUpdate!.Days = serviceDuration.Days;
                _context.ServiceDurations!.Update(serviceDurationUpdate);
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
                ServiceDuration? serviceDuration = await Get(id);
                _context.ServiceDurations!.Remove(serviceDuration!);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceDuration>> GetList(List<int> ids)
        {
            try
            {
                return await _context.ServiceDurations!.Where(p => ids.Contains(p.Id)).Include(p => p.Services).ToListAsync();
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
                List<ServiceDuration> serviceDurations = await GetList(ids);
                _context.ServiceDurations!.RemoveRange(serviceDurations);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ServiceDurationExists(ServiceDuration serviceDuration)
        {
            try
            {
                return await _context.ServiceDurations!.AnyAsync(p => p.Name == serviceDuration.Name || p.Days == serviceDuration.Days);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
