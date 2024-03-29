﻿using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.Repositories;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _mapper = mapper;
            _departmentRepository = departmentRepository;
        }

        public async Task<DepartmentViewModel> Get(int id)
        {
            try
            {
                return _mapper.Map<DepartmentViewModel>(await _departmentRepository.Get(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DepartmentViewModel> Get(string name)
        {
            try
            {
                return _mapper.Map<DepartmentViewModel>(await _departmentRepository.Get(name));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DepartmentViewModel>> GetList()
        {
            try
            {
                return _mapper.Map<List<DepartmentViewModel>>(await _departmentRepository.GetList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<DepartmentViewModel>> GetList(List<int> ids)
        {
            try
            {
                return _mapper.Map<List<DepartmentViewModel>>(await _departmentRepository.GetList(ids));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Create(DepartmentViewModel department)
        {
            try
            {
                await _departmentRepository.Create(_mapper.Map<Department>(department));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Update(DepartmentViewModel department)
        {
            try
            {
                await _departmentRepository.Update(_mapper.Map<Department>(department));
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
                await _departmentRepository.HardDelete(id);
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
                await _departmentRepository.HardDelete(records.Ids);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DepartmentExists(DepartmentViewModel department)
        {
            try
            {
                return await _departmentRepository.DepartmentExists(_mapper.Map<Department>(department));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
