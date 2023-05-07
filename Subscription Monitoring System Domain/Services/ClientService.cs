using AutoMapper;
using Microsoft.AspNetCore.Http;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository clientRepository, IMapper mapper)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<List<ClientViewModel>> GetActiveList()
        {
            try
            {
                return _mapper.Map<List<ClientViewModel>>(await _clientRepository.GetActiveList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClientViewModel> GetActive(int id)
        {
            try
            {
                return _mapper.Map<ClientViewModel>(await _clientRepository.GetActive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClientViewModel> GetInactive(int id)
        {
            try
            {
                return _mapper.Map<ClientViewModel>(await _clientRepository.GetInactive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClientViewModel> GetActive(string name)
        {
            try
            {
                return _mapper.Map<ClientViewModel>(await _clientRepository.GetActive(name));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ListViewModel> GetList(ClientFilterViewModel filter)
        {
            try
            {
                if (filter.Page == 0) filter.Page = 1;
                List<ClientViewModel> clients = _mapper.Map<List<ClientViewModel>>(await _clientRepository.GetList());

                clients = clients.Where(p => (filter.Id == 0 || p.Id == filter.Id) &&
                (string.IsNullOrEmpty(filter.Name) || p.Name == filter.Name) && (string.IsNullOrEmpty(filter.EmailAddress)
                || p.EmailAddress == filter.EmailAddress) && p.IsActive == filter.IsActive).ToList();

                clients = (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.Equals(SortDirectionConstants.Descending)) ? SortDescending(filter.SortBy, clients) : SortAscending(filter.SortBy, clients); 

                int totalCount = clients.Count;
                int totalPages = (int)Math.Ceiling((double)totalCount / BaseConstants.PageSize);
                var pagination = new
                {
                    pages = totalPages,
                    size = totalCount
                };

                clients = clients.Skip(BaseConstants.PageSize * (filter.Page - 1)).Take(BaseConstants.PageSize).ToList();

                return new ListViewModel { Pagination = pagination, Data = clients };
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<ClientViewModel> SortAscending(string sortBy, List<ClientViewModel> clients)
        {
            return sortBy switch
            {
                ClientConstants.HeaderId => clients.OrderBy(p => p.Id).ToList(),
                ClientConstants.HeaderName => clients.OrderBy(p => p.Name).ToList(),
                ClientConstants.HeaderEmailAddress => clients.OrderBy(p => p.EmailAddress).ToList(),
                _ => clients.OrderBy(p => p.Id).ToList(),
            };
        }

        public List<ClientViewModel> SortDescending(string sortBy, List<ClientViewModel> clients)
        {
            return sortBy switch
            {
                ClientConstants.HeaderId => clients.OrderByDescending(p => p.Id).ToList(),
                ClientConstants.HeaderName => clients.OrderByDescending(p => p.Name).ToList(),
                ClientConstants.HeaderEmailAddress => clients.OrderByDescending(p => p.EmailAddress).ToList(),
                _ => clients.OrderByDescending(p => p.Id).ToList(),
            };
        }

        public async Task<List<ClientViewModel>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<ClientViewModel>>(await _clientRepository.GetList(ids));
            }
            catch(Exception)
            {
                throw;
            }
        }

        public async Task Create(ClientViewModel client)
        {
            try
            {
                await _clientRepository.Create(_mapper.Map<Client>(client));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(ClientViewModel client)
        {
            try
            {
                await _clientRepository.Update(_mapper.Map<Client>(client));
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
                await _clientRepository.HardDelete(id);
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
                await _clientRepository.SoftDelete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task HardDelete(RecordIdsViewModel records)
        {
            try
            {
                await _clientRepository.HardDelete(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SoftDelete(RecordIdsViewModel records)
        {
            try
            {
                await _clientRepository.SoftDelete(records.Ids);
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
                await _clientRepository.Restore(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Restore(RecordIdsViewModel records)
        {
            try
            {
                await _clientRepository.Restore(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ClientExists(ClientViewModel client)
        {
            try
            {
                return await _clientRepository.ClientExists(_mapper.Map<Client>(client));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
