using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.Repositories;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IMapper _mapper;

        public ServiceService(IServiceRepository serviceRepository, IMapper mapper)
        {
            _mapper = mapper;
            _serviceRepository = serviceRepository;
        }

        public async Task<List<ServiceViewModel>> GetActiveList()
        {
            try
            {
                return _mapper.Map<List<ServiceViewModel>>(await _serviceRepository.GetActiveList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceViewModel> GetActive(int id)
        {
            try
            {
                return _mapper.Map<ServiceViewModel>(await _serviceRepository.GetActive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceViewModel> GetInactive(int id)
        {
            try
            {
                return _mapper.Map<ServiceViewModel>(await _serviceRepository.GetInactive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceViewModel> GetActive(string name)
        {
            try
            {
                return _mapper.Map<ServiceViewModel>(await _serviceRepository.GetActive(name));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ListViewModel> GetList(ServiceFilterViewModel filter)
        {
            try
            {
                if (filter.Page == 0) filter.Page = 1;

                List<ServiceViewModel> services = _mapper.Map<List<ServiceViewModel>>(await _serviceRepository.GetList());

                services = services.Where(p => (filter.Id == 0 || p.Id == filter.Id) && (string.IsNullOrEmpty(filter.Name) || p.Name == filter.Name) &&
                (string.IsNullOrEmpty(filter.Description) || p.Description == filter.Description) && (filter.Price == 0 || p.Price == filter.Price) &&
                (string.IsNullOrEmpty(filter.ServiceTypeName) || p.ServiceTypeName == filter.ServiceTypeName) && p.IsActive == filter.IsActive).ToList();

                services = (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.Equals(SortDirectionConstants.Descending)) ? SortDescending(filter.SortBy, services) : SortAscending(filter.SortBy, services);

                int totalCount = services.Count;
                int totalPages = (int)Math.Ceiling((double)totalCount / BaseConstants.PageSize);
                var pagination = new
                {
                    pages = totalPages,
                    size = totalCount
                };

                services = services.Skip(BaseConstants.PageSize * (filter.Page - 1)).Take(BaseConstants.PageSize).ToList();

                return new ListViewModel { Pagination = pagination, Data = services };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ServiceViewModel> SortAscending(string sortBy, List<ServiceViewModel> services)
        {
            return sortBy switch
            {
                ServiceConstants.HeaderId => services.OrderBy(p => p.Id).ToList(),
                ServiceConstants.HeaderName => services.OrderBy(p => p.Name).ToList(),
                ServiceConstants.HeaderDescription => services.OrderBy(p => p.Description).ToList(),
                ServiceConstants.HeaderPrice => services.OrderBy(p => p.Price).ToList(),
                ServiceConstants.HeaderServiceTypeName => services.OrderBy(p => p.ServiceTypeName).ToList(),
                _ => services.OrderBy(p => p.Id).ToList(),
            };
        }

        public List<ServiceViewModel> SortDescending(string sortBy, List<ServiceViewModel> services)
        {
            return sortBy switch
            {
                ServiceConstants.HeaderId => services.OrderByDescending(p => p.Id).ToList(),
                ServiceConstants.HeaderName => services.OrderByDescending(p => p.Name).ToList(),
                ServiceConstants.HeaderDescription => services.OrderByDescending(p => p.Description).ToList(),
                ServiceConstants.HeaderPrice => services.OrderByDescending(p => p.Price).ToList(),
                ServiceConstants.HeaderServiceTypeName => services.OrderByDescending(p => p.ServiceTypeName).ToList(),
                _ => services.OrderByDescending(p => p.Id).ToList(),
            };
        }

        public async Task<List<ServiceViewModel>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<ServiceViewModel>>(await _serviceRepository.GetList(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(ServiceViewModel service, ServiceTypeViewModel serviceType)
        {
            try
            {
                Service serviceMapped = _mapper.Map<Service>(service);
                serviceMapped.ServiceTypeId = serviceType.Id;
                await _serviceRepository.Create(serviceMapped);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(ServiceViewModel service, ServiceTypeViewModel serviceType)
        {
            try
            {
                Service serviceMapped = _mapper.Map<Service>(service);
                serviceMapped.ServiceTypeId = serviceType.Id;
                await _serviceRepository.Update(serviceMapped);
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
                await _serviceRepository.HardDelete(id);
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
                await _serviceRepository.SoftDelete(id);
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
                await _serviceRepository.HardDelete(records.Ids);
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
                await _serviceRepository.SoftDelete(records.Ids);
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
                await _serviceRepository.Restore(id);
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
                await _serviceRepository.Restore(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> ServiceExists(ServiceViewModel service)
        {
            try
            {
                return await _serviceRepository.ServiceExists(_mapper.Map<Service>(service));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
