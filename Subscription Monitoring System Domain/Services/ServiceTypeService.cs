using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Repositories;
using Subscription_Monitoring_System_Data.ViewModels;
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

        public async Task<ServiceTypeViewModel> Get(int id)
        {
            try
            {
                return _mapper.Map<ServiceTypeViewModel>(await _serviceTypeRepository.Get(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceTypeViewModel> Get(string name)
        {
            try
            {
                return _mapper.Map<ServiceTypeViewModel>(await _serviceTypeRepository.Get(name));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceTypeViewModel>> GetList()
        {
            try
            {
                return _mapper.Map<List<ServiceTypeViewModel>>(await _serviceTypeRepository.GetList());
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
                return await _serviceTypeRepository.ServiceTypeExists(name);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
