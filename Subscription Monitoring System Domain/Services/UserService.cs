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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<UserDto>> GetActiveList()
        {
            try
            {
                return _mapper.Map<List<UserDto>>(await _userRepository.GetActiveList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto> GetActive(int id)
        {
            try
            {
                return _mapper.Map<UserDto>(await _userRepository.GetActive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto> GetInactive(int id)
        {
            try
            {
                return _mapper.Map<UserDto>(await _userRepository.GetInactive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto> GetActive(string code)
        {
            try
            {
                return _mapper.Map<UserDto>(await _userRepository.GetActive(code));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> GetByEmail(string emailAddress)
        {
            try
            {
                return await _userRepository.GetActiveByEmail(emailAddress);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> Get(string name)
        {
            try
            {
                return await _userRepository.GetActive(name);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ListDto> GetList(UserFilterDto filter)
        {
            try
            {
                List<UserDto> users = _mapper.Map<List<UserDto>>(await _userRepository.GetList(filter));

                int totalCount = users.Count;
                int totalPages = (int)Math.Ceiling((double)totalCount / BaseConstants.PageSize);
                var pagination = new
                {
                    pages = totalPages,
                    size = totalCount
                };

                users = users.Skip(BaseConstants.PageSize * (filter.Page - 1)).Take(BaseConstants.PageSize).ToList();

                return new ListDto { Pagination = pagination, Data = users };
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<List<UserDto>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<UserDto>>(await _userRepository.GetList(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(UserDto user, DepartmentDto department)
        {
            try
            {
                User userMapped = _mapper.Map<User>(user);
                userMapped.DepartmentId = department.Id;
                await _userRepository.Create(userMapped);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(UserDto user, DepartmentDto department)
        {
            try
            {
                User userMapped = _mapper.Map<User>(user);
                userMapped.DepartmentId = department.Id;
                await _userRepository.Update(userMapped);
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
                await _userRepository.HardDelete(id);
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
                await _userRepository.SoftDelete(id);
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
                await _userRepository.HardDelete(records.Ids);
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
                await _userRepository.SoftDelete(records.Ids);
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
                await _userRepository.Restore(id);
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
                await _userRepository.Restore(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UserExists(UserDto user)
        {
            try
            {
                return await _userRepository.UserExists(_mapper.Map<User>(user));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> RefreshTokenExists(string refreshToken)
        {
            try
            {
                return await _userRepository.RefreshTokenExists(refreshToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
