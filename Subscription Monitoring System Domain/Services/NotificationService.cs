﻿using AutoMapper;
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
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, IMapper mapper)
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
        }

        public async Task<NotificationDto> Get(int id)
        {
            try
            {
                return _mapper.Map<NotificationDto>(await _notificationRepository.Get(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<NotificationDto>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<NotificationDto>>(await _notificationRepository.GetList(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(NotificationDto notification, List<int> userIds)
        {
            try
            {
                await _notificationRepository.Create(_mapper.Map<Notification>(notification), userIds);
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

        public async Task HardDelete(RecordIdsDto records)
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
    }
}