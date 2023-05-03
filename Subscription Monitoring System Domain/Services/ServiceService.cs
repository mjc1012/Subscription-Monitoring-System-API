using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.Repositories;
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

        public async Task<List<ServiceDto>> GetActiveList()
        {
            try
            {
                return _mapper.Map<List<ServiceDto>>(await _serviceRepository.GetActiveList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceDto> GetActive(int id)
        {
            try
            {
                return _mapper.Map<ServiceDto>(await _serviceRepository.GetActive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceDto> GetInactive(int id)
        {
            try
            {
                return _mapper.Map<ServiceDto>(await _serviceRepository.GetInactive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceDto> GetActive(string name)
        {
            try
            {
                return _mapper.Map<ServiceDto>(await _serviceRepository.GetActive(name));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ListDto> GetList(ServiceFilterDto filter)
        {
            try
            {
                List<ServiceDto> services = _mapper.Map<List<ServiceDto>>(await _serviceRepository.GetList(filter));

                int totalCount = services.Count;
                int totalPages = (int)Math.Ceiling((double)totalCount / BaseConstants.PageSize);
                var pagination = new
                {
                    pages = totalPages,
                    size = totalCount
                };

                services = services.Skip(BaseConstants.PageSize * (filter.Page - 1)).Take(BaseConstants.PageSize).ToList();

                return new ListDto { Pagination = pagination, Data = services };
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<ServiceDto>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<ServiceDto>>(await _serviceRepository.GetList(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(ServiceDto service, ServiceTypeDto serviceType)
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

        public async Task Update(ServiceDto service, ServiceTypeDto serviceType)
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

        public async Task HardDelete(RecordIdsDto records)
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

        public async Task SoftDelete(RecordIdsDto records)
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

        public async Task Restore(RecordIdsDto records)
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
        public async Task<bool> ServiceExists(ServiceDto service)
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
