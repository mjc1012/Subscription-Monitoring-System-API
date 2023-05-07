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

        public async Task<bool> DepartmentExists(string name)
        {
            try
            {
                return await _departmentRepository.DepartmentExists(name);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
