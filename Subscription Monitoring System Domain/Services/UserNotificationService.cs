using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUserNotificationRepository _userNotificationRepository;
        private readonly IMapper _mapper;

        public UserNotificationService(IUserNotificationRepository userNotificationRepository, IMapper mapper)
        {
            _userNotificationRepository = userNotificationRepository;
            _mapper = mapper;
        }

        public async Task<UserNotification> GetActive(int id)
        {
            try
            {
                return await _userNotificationRepository.GetActive(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserNotification> GetInactive(int id)
        {
            try
            {
                return await _userNotificationRepository.GetInactive(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<UserNotification>> GetList(List<int> ids)
        {
            try
            {
                return await _userNotificationRepository.GetList(ids);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<NotificationViewModel>> GetList(int userId)
        {
            try
            {
                List<UserNotification> userNotifications = await _userNotificationRepository.GetList(userId);
                return _mapper.Map<List<NotificationViewModel>>(userNotifications);
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
                await _userNotificationRepository.HardDelete(id);
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
                await _userNotificationRepository.SoftDelete(id);
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
                await _userNotificationRepository.HardDelete(records.Ids);
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
                await _userNotificationRepository.SoftDelete(records.Ids);
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
                await _userNotificationRepository.Restore(id);
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
                await _userNotificationRepository.Restore(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
