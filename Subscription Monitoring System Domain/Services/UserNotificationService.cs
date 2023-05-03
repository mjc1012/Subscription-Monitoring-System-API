using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IUserNotificationRepository _userNotificationRepository;

        public UserNotificationService(IUserNotificationRepository userNotificationRepository)
        {
            _userNotificationRepository = userNotificationRepository;
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

        public async Task<List<UserNotification>> GetList(int userId)
        {
            try
            {
                return await _userNotificationRepository.GetList(userId);
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

        public async Task HardDelete(RecordIdsDto records)
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

        public async Task SoftDelete(RecordIdsDto records)
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

        public async Task Restore(RecordIdsDto records)
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
