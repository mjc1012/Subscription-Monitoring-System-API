using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System.Text.RegularExpressions;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class ServiceDurationHandler : IServiceDurationHandler
    {

        private readonly IServiceDurationService _serviceDurationService;
        public ServiceDurationHandler(IServiceDurationService serviceDurationService)
        {
            _serviceDurationService = serviceDurationService;
        }

        public async Task<List<string>> CanAdd(ServiceDurationViewModel serviceDuration)
        {
            List<string> validationErrors = new();

            if (serviceDuration != null && !string.IsNullOrEmpty(serviceDuration.Name))
            {
                if (Regex.IsMatch(serviceDuration.Name, "[a-z,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                {
                    validationErrors.Add(ServiceDurationConstants.NameInvalid);
                }

                if (await _serviceDurationService.ServiceDurationExists(serviceDuration))
                {
                    validationErrors.Add(ServiceDurationConstants.Exists);
                }
            }
            else
            {
                validationErrors.Add(ServiceDurationConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(ServiceDurationViewModel serviceDuration)
        {
            List<string> validationErrors = new();

            ServiceDurationViewModel serviceDurationFound = await _serviceDurationService.Get(serviceDuration.Id);
            if (serviceDuration != null && serviceDurationFound != null && !string.IsNullOrEmpty(serviceDuration.Name) && serviceDuration.Id > 0)
            {
                if (serviceDuration.Name == serviceDurationFound.Name)
                {
                    validationErrors.Add(ServiceDurationConstants.NoChanges);
                }
                else
                {

                    if (Regex.IsMatch(serviceDuration.Name, "[a-z,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                    {
                        validationErrors.Add(ServiceDurationConstants.NameInvalid);
                    }

                    if (await _serviceDurationService.ServiceDurationExists(serviceDuration))
                    {
                        validationErrors.Add(ServiceDurationConstants.Exists);
                    }
                }
            }
            else
            {
                validationErrors.Add(ServiceDurationConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(int id)
        {
            List<string> validationErrors = new();

            ServiceDurationViewModel service = await _serviceDurationService.Get(id);
            if (service == null)
            {
                validationErrors.Add(ServiceDurationConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<ServiceDurationViewModel> services = await _serviceDurationService.GetList(records.Ids);
            if (services == null)
            {
                validationErrors.Add(ServiceDurationConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
