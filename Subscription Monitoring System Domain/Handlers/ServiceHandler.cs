using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class ServiceHandler : IServiceHandler
    {
        private readonly IServiceService _serviceService;
        public ServiceHandler(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }
        public async Task<List<string>> CanAdd(ServiceDto service)
        {
            var validationErrors = new List<string>();

            if (service != null)
            {

                if (await _serviceService.ServiceExists(service))
                {
                    validationErrors.Add(ServiceConstants.Exists);
                }
            }
            else
            {
                validationErrors.Add(ServiceConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(ServiceDto service)
        {
            var validationErrors = new List<string>();

            if (service != null)
            {

                if (await _serviceService.ServiceExists(service))
                {
                    validationErrors.Add(ServiceConstants.Exists);
                }
            }
            else
            {
                validationErrors.Add(ServiceConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteActive(int id)
        {
            var validationErrors = new List<string>();

            ServiceDto service = await _serviceService.GetActive(id);
            if (service == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            var validationErrors = new List<string>();

            ServiceDto service = await _serviceService.GetInactive(id);
            if (service == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsDto records)
        {
            var validationErrors = new List<string>();

            List<ServiceDto> services = await _serviceService.GetList(records.Ids);
            if (services == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            var validationErrors = new List<string>();

            ServiceDto service = await _serviceService.GetInactive(id);
            if (service == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsDto records)
        {
            var validationErrors = new List<string>();

            List<ServiceDto> services = await _serviceService.GetList(records.Ids);
            if (services == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
