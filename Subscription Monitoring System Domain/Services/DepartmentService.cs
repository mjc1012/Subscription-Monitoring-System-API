using AutoMapper;
using Subscription_Monitoring_System_Data.Contracts;
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
        private readonly IDepartmentRepository _roleRepository;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository roleRepository, IMapper mapper)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
        }

        public async Task<DepartmentViewModel> Get(int id)
        {
            try
            {
                return _mapper.Map<DepartmentViewModel>(await _roleRepository.Get(id));
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
                return _mapper.Map<DepartmentViewModel>(await _roleRepository.Get(name));
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
                return _mapper.Map<List<DepartmentViewModel>>(await _roleRepository.GetList());
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
