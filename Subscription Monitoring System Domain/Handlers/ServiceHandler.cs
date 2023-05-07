using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class ServiceHandler : IServiceHandler
    {
        private readonly IServiceService _serviceService;
        private readonly IServiceTypeService _serviceTypeService;
        public ServiceHandler(IServiceService serviceService, IServiceTypeService serviceTypeService)
        {
            _serviceService = serviceService;
            _serviceTypeService = serviceTypeService;
        }

        public List<string> CanFilter(ServiceFilterViewModel filter)
        {
            var validationErrors = new List<string>();

            if (!string.IsNullOrEmpty(filter.SortOrder) && (filter.SortOrder is not SortDirectionConstants.Ascending and not SortDirectionConstants.Descending))
            {
                validationErrors.Add(SortDirectionConstants.SortDirectionInvalid);
            }

            if (!string.IsNullOrEmpty(filter.SortBy) && (filter.SortBy is not ServiceConstants.HeaderId and not ServiceConstants.NameInvalid and not ServiceConstants.HeaderDescription and not ServiceConstants.HeaderPrice 
                and not ServiceConstants.HeaderServiceTypeName))
            {
                validationErrors.Add(ServiceConstants.SortByInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanAdd(ServiceViewModel service)
        {
            var validationErrors = new List<string>();

            if (service != null)
            {

                if (await _serviceService.ServiceExists(service))
                {
                    validationErrors.Add(ServiceConstants.Exists);
                }

                Match match = Regex.Match(service.Name, "[^A-Z]");
                if (match.Success)
                {
                    validationErrors.Add(ServiceConstants.NameInvalid);
                }

                if (!await _serviceTypeService.ServiceTypeExists(service.ServiceTypeName))
                {
                    validationErrors.Add(ServiceTypeConstants.DoesNotExist);
                }
            }
            else
            {
                validationErrors.Add(ServiceConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(ServiceViewModel service)
        {
            var validationErrors = new List<string>();

            ServiceViewModel serviceFound = await _serviceService.GetActive(service.Id);
            if (service != null && serviceFound != null)
            {
                if (service.Name == serviceFound.Name && service.Price == serviceFound.Price && service.ServiceTypeName == serviceFound.ServiceTypeName && service.Description == serviceFound.Description)
                {
                    validationErrors.Add(ServiceConstants.NoChanges);
                }
                else
                {
                    if (await _serviceService.ServiceExists(service))
                    {
                        validationErrors.Add(ServiceConstants.Exists);
                    }

                    Match match = Regex.Match(service.Name, "[^A-Z]");
                    if (match.Success)
                    {
                        validationErrors.Add(ServiceConstants.NameInvalid);
                    }

                    if (!await _serviceTypeService.ServiceTypeExists(service.ServiceTypeName))
                    {
                        validationErrors.Add(ServiceTypeConstants.DoesNotExist);
                    }
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

            ServiceViewModel service = await _serviceService.GetActive(id);
            if (service == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            var validationErrors = new List<string>();

            ServiceViewModel service = await _serviceService.GetInactive(id);
            if (service == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<ServiceViewModel> services = await _serviceService.GetList(records.Ids);
            if (services == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            var validationErrors = new List<string>();

            ServiceViewModel service = await _serviceService.GetInactive(id);
            if (service == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<ServiceViewModel> services = await _serviceService.GetList(records.Ids);
            if (services == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
