using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataContext _context;
        public ClientRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetActiveList()
        {
            try
            {
                return await _context.Clients.Where(p => p.IsActive == true).Include(p => p.InvolvedSubscriptions).Include(p => p.SubscriptionClients).ThenInclude(p => p.Subscription).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Client> GetActive(int id)
        {
            try
            {
                return await _context.Clients.Where(p => p.Id == id && p.IsActive == true).Include(p => p.InvolvedSubscriptions).Include(p => p.SubscriptionClients).ThenInclude(p => p.Subscription).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Client> GetInactive(int id)
        {
            try
            {
                return await _context.Clients.Where(p => p.Id == id && p.IsActive == false).Include(p => p.InvolvedSubscriptions).Include(p => p.SubscriptionClients).ThenInclude(p => p.Subscription).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Client> GetActive(string name)
        {
            try
            {
                return await _context.Clients.Where(p => p.Name == name && p.IsActive == true).Include(p => p.InvolvedSubscriptions).Include(p => p.SubscriptionClients).ThenInclude(p => p.Subscription).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Client>> GetList(ClientFilterViewModel filter)
        {
            try
            {
                if (filter.Page == 0) filter.Page = 1;

                List<Client> clients = await _context.Clients.Where(p => (filter.Id == 0 || p.Id == filter.Id) &&
                (string.IsNullOrEmpty(filter.Name) || p.Name == filter.Name) && (string.IsNullOrEmpty(filter.EmailAddress) 
                || p.EmailAddress == filter.EmailAddress) && p.IsActive == filter.IsActive).Include(p => p.InvolvedSubscriptions).Include(p => p.SubscriptionClients).ThenInclude(p => p.Subscription).ToListAsync();

                return  (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.Equals(SortDirectionConstants.Descending)) ? SortDescending(filter.SortBy, clients) : SortAscending(filter.SortBy, clients);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Client> SortAscending(string sortBy, List<Client> clients)
        {
            return sortBy switch
            {
                ClientConstants.HeaderId => clients.OrderBy(p => p.Id).ToList(),
                ClientConstants.HeaderName => clients.OrderBy(p => p.Name).ToList(),
                ClientConstants.HeaderEmailAddress => clients.OrderBy(p => p.EmailAddress).ToList(),
                _ => clients.OrderBy(p => p.Id).ToList(),
            };
        }

        public List<Client> SortDescending(string sortBy, List<Client> clients)
        {
            return sortBy switch
            {
                ClientConstants.HeaderId => clients.OrderByDescending(p => p.Id).ToList(),
                ClientConstants.HeaderName => clients.OrderByDescending(p => p.Name).ToList(),
                ClientConstants.HeaderEmailAddress => clients.OrderByDescending(p => p.EmailAddress).ToList(),
                _ => clients.OrderByDescending(p => p.Id).ToList(),
            };
        }

        public async Task Create(Client client)
        {
            try
            {
                client.IsActive = true;
                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task Update(Client client)
        {
            try
            {
                Client clientUpdate = await GetActive(client.Id);
                clientUpdate.Name = client.Name;
                clientUpdate.EmailAddress = client.EmailAddress;
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
                Client client = await GetInactive(id);
                _context.Clients.Remove(client);
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
                Client client = await GetActive(id);
                client.IsActive = false;
                _context.Clients.Update(client);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Client>> GetList(List<int> ids)
        {
            try
            {
                return await _context.Clients.Where(p => ids.Contains(p.Id)).Include(p => p.InvolvedSubscriptions).Include(p => p.SubscriptionClients).ThenInclude(p => p.Subscription).ToListAsync();
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
                List<Client> clients = await GetList(ids);
                _context.Clients.RemoveRange(clients);
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
                List<Client> clients = await GetList(ids);
                clients.ForEach(p => p.IsActive = false);
                _context.Clients.UpdateRange(clients);
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
                Client client = await GetInactive(id);
                client.IsActive = true;
                _context.Clients.Update(client);
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
                List<Client> clients = await GetList(ids);
                clients.ForEach(p => p.IsActive = true);
                _context.Clients.UpdateRange(clients);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ClientExists(Client client)
        {
            try
            {
                return await _context.Clients.AnyAsync(p => (p.Name == client.Name || p.EmailAddress == client.EmailAddress) && p.Id != client.Id && p.IsActive == true);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
