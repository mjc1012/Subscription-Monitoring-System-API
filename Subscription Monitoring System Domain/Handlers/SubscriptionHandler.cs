using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class SubscriptionHandler : ISubscriptionHandler
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionHandler(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }
        public async Task<List<string>> CanAdd(SubscriptionViewModel subscription)
        {
            var validationErrors = new List<string>();

            if (subscription != null)
            {

                if (await _subscriptionService.SubscriptionExists(subscription))
                {
                    validationErrors.Add(SubscriptionConstants.Exists);
                }
            }
            else
            {
                validationErrors.Add(SubscriptionConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(SubscriptionViewModel subscription)
        {
            var validationErrors = new List<string>();

            if (subscription != null)
            {

                if (await _subscriptionService.SubscriptionExists(subscription))
                {
                    validationErrors.Add(SubscriptionConstants.Exists);
                }
            }
            else
            {
                validationErrors.Add(SubscriptionConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteActive(int id)
        {
            var validationErrors = new List<string>();

            SubscriptionViewModel subscription = await _subscriptionService.GetActive(id);
            if (subscription == null)
            {
                validationErrors.Add(SubscriptionConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            var validationErrors = new List<string>();

            SubscriptionViewModel subscription = await _subscriptionService.GetInactive(id);
            if (subscription == null)
            {
                validationErrors.Add(SubscriptionConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<SubscriptionViewModel> subscriptions = await _subscriptionService.GetList(records.Ids);
            if (subscriptions == null)
            {
                validationErrors.Add(SubscriptionConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            var validationErrors = new List<string>();

            SubscriptionViewModel subscription = await _subscriptionService.GetInactive(id);
            if (subscription == null)
            {
                validationErrors.Add(SubscriptionConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<SubscriptionViewModel> subscriptions = await _subscriptionService.GetList(records.Ids);
            if (subscriptions == null)
            {
                validationErrors.Add(SubscriptionConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
