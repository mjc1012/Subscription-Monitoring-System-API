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
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<UserViewModel>> GetActiveList()
        {
            try
            {
                return _mapper.Map<List<UserViewModel>>(await _userRepository.GetActiveList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserViewModel> GetActive(int id)
        {
            try
            {
                return _mapper.Map<UserViewModel>(await _userRepository.GetActive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserViewModel> GetInactive(int id)
        {
            try
            {
                return _mapper.Map<UserViewModel>(await _userRepository.GetInactive(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserViewModel> GetActive(string code)
        {
            try
            {
                return _mapper.Map<UserViewModel>(await _userRepository.GetActive(code));
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

        public async Task<ListViewModel> GetList(UserFilterViewModel filter)
        {
            try
            {
                if (filter.Page == 0) filter.Page = 1;

                List<UserViewModel> users = _mapper.Map<List<UserViewModel>>(await _userRepository.GetList());

                users = users.Where(p => (filter.Id == 0 || p.Id == filter.Id) &&
                (string.IsNullOrEmpty(filter.Code) || p.Code == filter.Code) &&
                (string.IsNullOrEmpty(filter.FirstName) || p.FirstName == filter.FirstName) &&
                (string.IsNullOrEmpty(filter.MiddleName) || p.MiddleName == filter.MiddleName) &&
                (string.IsNullOrEmpty(filter.LastName) || p.LastName == filter.LastName) &&
                (string.IsNullOrEmpty(filter.EmailAddress) || p.EmailAddress == filter.EmailAddress) &&
                (string.IsNullOrEmpty(filter.DepartmentName) || p.DepartmentName == filter.DepartmentName) && p.IsActive == filter.IsActive).ToList();

                users = (!string.IsNullOrEmpty(filter.SortOrder) && filter.SortOrder.Equals(SortDirectionConstants.Descending)) ? SortDescending(filter.SortBy, users) : SortAscending(filter.SortBy, users);

                int totalCount = users.Count;
                int totalPages = (int)Math.Ceiling((double)totalCount / BaseConstants.PageSize);
                var pagination = new
                {
                    pages = totalPages,
                    size = totalCount
                };

                users = users.Skip(BaseConstants.PageSize * (filter.Page - 1)).Take(BaseConstants.PageSize).ToList();

                return new ListViewModel { Pagination = pagination, Data = users };
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<UserViewModel> SortAscending(string sortBy, List<UserViewModel> users)
        {
            return sortBy switch
            {
                UserConstants.HeaderId => users.OrderBy(p => p.Id).ToList(),
                UserConstants.HeaderCode => users.OrderBy(p => p.Code).ToList(),
                UserConstants.HeaderFirstName => users.OrderBy(p => p.FirstName).ToList(),
                UserConstants.HeaderMiddleName => users.OrderBy(p => p.MiddleName).ToList(),
                UserConstants.HeaderLastName => users.OrderBy(p => p.LastName).ToList(),
                UserConstants.HeaderEmailAddress => users.OrderBy(p => p.EmailAddress).ToList(),
                UserConstants.HeaderDepartmentName => users.OrderBy(p => p.DepartmentName).ToList(),
                _ => users.OrderBy(p => p.Id).ToList(),
            };
        }

        public List<UserViewModel> SortDescending(string sortBy, List<UserViewModel> users)
        {
            return sortBy switch
            {
                UserConstants.HeaderId => users.OrderByDescending(p => p.Id).ToList(),
                UserConstants.HeaderCode => users.OrderByDescending(p => p.Code).ToList(),
                UserConstants.HeaderFirstName => users.OrderByDescending(p => p.FirstName).ToList(),
                UserConstants.HeaderMiddleName => users.OrderByDescending(p => p.MiddleName).ToList(),
                UserConstants.HeaderLastName => users.OrderByDescending(p => p.LastName).ToList(),
                UserConstants.HeaderEmailAddress => users.OrderByDescending(p => p.EmailAddress).ToList(),
                UserConstants.HeaderDepartmentName => users.OrderByDescending(p => p.DepartmentName).ToList(),
                _ => users.OrderByDescending(p => p.Id).ToList(),
            };
        }

        public async Task<List<UserViewModel>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<UserViewModel>>(await _userRepository.GetList(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(UserViewModel user, DepartmentViewModel department)
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

        public async Task Update(UserViewModel user, UserViewModel oldUser, DepartmentViewModel department)
        {
            try
            {
                User userMapped = _mapper.Map<User>(user);
                userMapped.DepartmentId = department.Id;
                await _userRepository.Update(userMapped, _mapper.Map<User>(oldUser));
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

        public async Task HardDelete(RecordIdsViewModel records)
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

        public async Task SoftDelete(RecordIdsViewModel records)
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

        public async Task Restore(RecordIdsViewModel records)
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

        public async Task<bool> UserExists(UserViewModel user)
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
