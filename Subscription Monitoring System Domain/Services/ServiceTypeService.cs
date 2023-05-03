using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IMapper _mapper;

        public ServiceTypeService(IServiceTypeRepository serviceTypeRepository, IMapper mapper)
        {
            _mapper = mapper;
            _serviceTypeRepository = serviceTypeRepository;
        }

        public async Task<ServiceTypeDto> Get(int id)
        {
            try
            {
                return _mapper.Map<ServiceTypeDto>(await _serviceTypeRepository.Get(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceTypeDto> Get(string name)
        {
            try
            {
                return _mapper.Map<ServiceTypeDto>(await _serviceTypeRepository.Get(name));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceTypeDto>> GetList()
        {
            try
            {
                return _mapper.Map<List<ServiceTypeDto>>(await _serviceTypeRepository.GetList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
