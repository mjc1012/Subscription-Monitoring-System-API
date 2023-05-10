using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.Repositories;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task<List<NotificationViewModel>> GetList()
        {
            try
            {
                return _mapper.Map<List<NotificationViewModel>>(await _notificationRepository.GetList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<NotificationViewModel> Get(int id)
        {
            try
            {
                return _mapper.Map<NotificationViewModel>(await _notificationRepository.Get(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<NotificationViewModel>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<NotificationViewModel>>(await _notificationRepository.GetList(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(NotificationViewModel notification)
        {
            try
            {
                await _notificationRepository.Create(_mapper.Map<Notification>(notification), notification.UserIds);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(NotificationViewModel notification)
        {
            try
            {
                await _notificationRepository.Update(_mapper.Map<Notification>(notification), notification.UserIds);
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
                await _notificationRepository.HardDelete(id);
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
                await _notificationRepository.HardDelete(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> NotificationExists(NotificationViewModel notification)
        {
            try
            {
                return await _notificationRepository.NotificationExists(_mapper.Map<Notification>(notification));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
