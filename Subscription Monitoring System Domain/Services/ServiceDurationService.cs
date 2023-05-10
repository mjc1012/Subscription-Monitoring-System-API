using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class ServiceDurationService : IServiceDurationService
    {
        private readonly IServiceDurationRepository _serviceDurationRepository;
        private readonly IMapper _mapper;

        public ServiceDurationService(IServiceDurationRepository serviceDurationRepository, IMapper mapper)
        {
            _mapper = mapper;
            _serviceDurationRepository = serviceDurationRepository;
        }

        public async Task<ServiceDurationViewModel> Get(int id)
        {
            try
            {
                return _mapper.Map<ServiceDurationViewModel>(await _serviceDurationRepository.Get(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceDurationViewModel> Get(string name)
        {
            try
            {
                return _mapper.Map<ServiceDurationViewModel>(await _serviceDurationRepository.Get(name));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<ServiceDurationViewModel>> GetList()
        {
            try
            {
                return _mapper.Map<List<ServiceDurationViewModel>>(await _serviceDurationRepository.GetList());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ServiceDurationViewModel>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<ServiceDurationViewModel>>(await _serviceDurationRepository.GetList(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(ServiceDurationViewModel serviceDuration)
        {
            try
            {
                await _serviceDurationRepository.Create(_mapper.Map<ServiceDuration>(serviceDuration));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(ServiceDurationViewModel serviceDuration)
        {
            try
            {
                await _serviceDurationRepository.Update(_mapper.Map<ServiceDuration>(serviceDuration));
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
                await _serviceDurationRepository.HardDelete(id);
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
                await _serviceDurationRepository.HardDelete(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> ServiceDurationExists(ServiceDurationViewModel serviceDuration)
        {
            try
            {
                return await _serviceDurationRepository.ServiceDurationExists(_mapper.Map<ServiceDuration>(serviceDuration));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
