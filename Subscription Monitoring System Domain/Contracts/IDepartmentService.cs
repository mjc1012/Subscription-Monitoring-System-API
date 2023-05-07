﻿using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IDepartmentService
    {
        Task<DepartmentViewModel> Get(int id);
        Task<DepartmentViewModel> Get(string name);
        Task<List<DepartmentViewModel>> GetList();
        Task<bool> DepartmentExists(string name);
    }
}
