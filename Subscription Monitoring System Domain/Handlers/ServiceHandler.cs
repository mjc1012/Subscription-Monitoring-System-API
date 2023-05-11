using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System.Text.RegularExpressions;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class ServiceHandler : IServiceHandler
    {
        private readonly IServiceService _serviceService;
        private readonly IServiceDurationService _serviceDurationService;
        public ServiceHandler(IServiceService serviceService, IServiceDurationService serviceDurationService)
        {
            _serviceService = serviceService;
            _serviceDurationService = serviceDurationService;
        }

        public List<string> CanFilter(ServiceFilterViewModel filter)
        {
            List<string> validationErrors = new();

            if (!string.IsNullOrEmpty(filter.SortOrder) && (filter.SortOrder is not SortDirectionConstants.Ascending and not SortDirectionConstants.Descending))
            {
                validationErrors.Add(SortDirectionConstants.SortDirectionInvalid);
            }

            if (!string.IsNullOrEmpty(filter.SortBy) && (filter.SortBy is not ServiceConstants.HeaderId and not ServiceConstants.NameInvalid and not ServiceConstants.HeaderDescription and not ServiceConstants.HeaderPrice 
                and not ServiceConstants.HeaderServiceDurationName))
            {
                validationErrors.Add(ServiceConstants.SortByInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanAdd(ServiceViewModel service)
        {
            List<string> validationErrors = new();

            if (service != null && !string.IsNullOrEmpty(service.Name) && !string.IsNullOrEmpty(service.Description) && !string.IsNullOrEmpty(service.ServiceDurationName) && service.Price > 0)
            {

                if (await _serviceService.ServiceExists(service))
                {
                    validationErrors.Add(ServiceConstants.Exists);
                }

                if (Regex.IsMatch(service.Name, "[a-z0-9,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                {
                    validationErrors.Add(ServiceConstants.NameInvalid);
                }

                if (!await _serviceDurationService.ServiceDurationExists(new ServiceDurationViewModel { Name = service.ServiceDurationName }))
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
            List<string> validationErrors = new();

            ServiceViewModel serviceFound = await _serviceService.GetActive(service.Id);

            if (service != null && serviceFound != null && service.Id > 0 && !string.IsNullOrEmpty(service.Name) && !string.IsNullOrEmpty(service.Description) && !string.IsNullOrEmpty(service.ServiceDurationName) && service.Price > 0)
            {
                if (service.Name == serviceFound.Name && service.Price == serviceFound.Price && service.ServiceDurationName == serviceFound.ServiceDurationName && service.Description == serviceFound.Description)
                {
                    validationErrors.Add(ServiceConstants.NoChanges);
                }
                else
                {
                    if (await _serviceService.ServiceExists(service))
                    {
                        validationErrors.Add(ServiceConstants.Exists);
                    }

                    if (Regex.IsMatch(service.Name, "[a-z0-9,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                    {
                        validationErrors.Add(ServiceConstants.NameInvalid);
                    }

                    if (!await _serviceDurationService.ServiceDurationExists(new ServiceDurationViewModel { Name = service.ServiceDurationName }))
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
            List<string> validationErrors = new();

            ServiceViewModel service = await _serviceService.GetActive(id);
            if (service == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            List<string> validationErrors = new();

            ServiceViewModel service = await _serviceService.GetInactive(id);
            if (service == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<ServiceViewModel> services = await _serviceService.GetList(records.Ids);
            if (services == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            List<string> validationErrors = new();

            ServiceViewModel service = await _serviceService.GetInactive(id);
            if (service == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<ServiceViewModel> services = await _serviceService.GetList(records.Ids);
            if (services == null)
            {
                validationErrors.Add(ServiceConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
