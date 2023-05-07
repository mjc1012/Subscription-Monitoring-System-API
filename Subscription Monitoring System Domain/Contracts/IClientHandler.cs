﻿using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IClientHandler
    {
        List<string> CanFilter(ClientFilterViewModel filter);
        Task<List<string>> CanAdd(ClientViewModel client);
        Task<List<string>> CanUpdate(ClientViewModel client);
        Task<List<string>> CanDeleteActive(int id);
        Task<List<string>> CanDeleteInactive(int id);
        Task<List<string>> CanDelete(RecordIdsViewModel records);
        Task<List<string>> CanRestore(int id);
        Task<List<string>> CanRestore(RecordIdsViewModel records);
    }
}
