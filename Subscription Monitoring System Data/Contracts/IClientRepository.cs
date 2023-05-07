﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Subscription_Monitoring_System_Data.Contracts
{
    public interface IClientRepository
    {
        Task<List<Client>> GetActiveList();
        Task<Client?> GetActive(int id);
        Task<Client?> GetInactive(int id);
        Task<Client?> GetActive(string name);
        Task<List<Client>> GetList();
        Task<List<Client>> GetList(List<int> ids);
        Task Create(Client client);
        Task Update(Client client);
        Task SoftDelete(int id);
        Task HardDelete(int id);
        Task SoftDelete(List<int> ids);
        Task HardDelete(List<int> ids);
        Task Restore(int id);
        Task Restore(List<int> ids);
        Task<bool> ClientExists(Client client);
    }
}
