using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System.Globalization;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class SubscriptionHandler : ISubscriptionHandler
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        private readonly IServiceService _serviceService;
        public SubscriptionHandler(ISubscriptionService subscriptionService, IUserService userService, IClientService clientService, IServiceService serviceService)
        {
            _subscriptionService = subscriptionService;
            _userService = userService;
            _clientService = clientService;
            _serviceService = serviceService;
        }

        public List<string> CanFilter(SubscriptionFilterViewModel filter)
        {
            List<string> validationErrors = new();

            if (!string.IsNullOrEmpty(filter.SortOrder) && (filter.SortOrder is not SortDirectionConstants.Ascending and not SortDirectionConstants.Descending))
            {
                validationErrors.Add(SortDirectionConstants.SortDirectionInvalid);
            }

            if (!string.IsNullOrEmpty(filter.SortBy) && (filter.SortBy is not SubscriptionConstants.HeaderId and not SubscriptionConstants.HeaderStartDate and not SubscriptionConstants.HeaderEndDate and
                not SubscriptionConstants.HeaderTotalPrice and not SubscriptionConstants.HeaderRemainingDays and not SubscriptionConstants.HeaderClientName and
                not SubscriptionConstants.HeaderServiceName and not SubscriptionConstants.HeaderCreatedByCode and not SubscriptionConstants.HeaderUpdatedByCode))
            {
                validationErrors.Add(SubscriptionConstants.SortByInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanAdd(SubscriptionViewModel subscription)
        {
            List<string> validationErrors = new();

            if (subscription != null && !string.IsNullOrEmpty(subscription.StartDate) && !string.IsNullOrEmpty(subscription.EndDate) && !string.IsNullOrEmpty(subscription.ClientName)
                && !string.IsNullOrEmpty(subscription.ServiceName) && !string.IsNullOrEmpty(subscription.CreatedByCode))
            {
                if (!DateTime.TryParseExact(subscription.StartDate, DateConstants.DateOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
                {
                    validationErrors.Add(SubscriptionConstants.InvalidStartDate);
                }

                if (!DateTime.TryParseExact(subscription.EndDate, DateConstants.DateOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
                {
                    validationErrors.Add(SubscriptionConstants.InvalidEndDate);
                }

                if(DateTime.Compare(endDate, startDate) <= 0)
                {
                    validationErrors.Add(SubscriptionConstants.InvalidDates);
                }

                if (await _subscriptionService.SubscriptionExists(subscription))
                {
                    validationErrors.Add(SubscriptionConstants.Exists);
                }

                if (!await _clientService.ClientExists(new ClientViewModel { Name = subscription.ClientName}))
                {
                    validationErrors.Add(ClientConstants.DoesNotExist);
                }

                if (!await _serviceService.ServiceExists(new ServiceViewModel { Name = subscription.ServiceName }))
                {
                    validationErrors.Add(ServiceConstants.DoesNotExist);
                }

                if (!await _userService.UserExists(new UserViewModel { Code = subscription.CreatedByCode }))
                {
                    validationErrors.Add(UserConstants.DoesNotExist);
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
            List<string> validationErrors = new();

            SubscriptionViewModel subscriptionFound = await _subscriptionService.GetActive(subscription.Id);
            if (subscription != null && subscriptionFound != null && !string.IsNullOrEmpty(subscription.StartDate) && !string.IsNullOrEmpty(subscription.EndDate) && !string.IsNullOrEmpty(subscription.ClientName)
                && !string.IsNullOrEmpty(subscription.ServiceName) && !string.IsNullOrEmpty(subscription.UpdatedByCode) && subscription.Id > 0)
            {
                if (subscription.StartDate == subscriptionFound.StartDate && subscription.EndDate == subscriptionFound.EndDate && subscription.ClientName == subscriptionFound.ClientName &&
                    subscription.ServiceName == subscriptionFound.ServiceName && subscriptionFound.ClientRecipients.Select(p => p.Id).ToList().Except(subscription.ClientIds).ToList().Any() &&
                    subscription.UserRecipients.Select(p => p.Id).ToList().Except(subscription.UserIds).ToList().Any())
                {
                    validationErrors.Add(SubscriptionConstants.NoChanges);
                }
                else
                {
                    if (!DateTime.TryParseExact(subscription.StartDate, DateConstants.DateOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
                    {
                        validationErrors.Add(SubscriptionConstants.InvalidStartDate);
                    }

                    if (!DateTime.TryParseExact(subscription.EndDate, DateConstants.DateOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
                    {
                        validationErrors.Add(SubscriptionConstants.InvalidEndDate);
                    }

                    if (DateTime.Compare(endDate, startDate) <= 0)
                    {
                        validationErrors.Add(SubscriptionConstants.InvalidDates);
                    }

                    if (await _subscriptionService.SubscriptionExists(subscription))
                    {
                        validationErrors.Add(SubscriptionConstants.Exists);
                    }

                    if (!await _clientService.ClientExists(new ClientViewModel { Name = subscription.ClientName }))
                    {
                        validationErrors.Add(ClientConstants.DoesNotExist);
                    }

                    if (!await _serviceService.ServiceExists(new ServiceViewModel { Name = subscription.ServiceName }))
                    {
                        validationErrors.Add(ServiceConstants.DoesNotExist);
                    }

                    if (!await _userService.UserExists(new UserViewModel { Code = subscription.UpdatedByCode }))
                    {
                        validationErrors.Add(UserConstants.DoesNotExist);
                    }
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
            List<string> validationErrors = new();

            SubscriptionViewModel subscription = await _subscriptionService.GetActive(id);
            if (subscription == null)
            {
                validationErrors.Add(SubscriptionConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            List<string> validationErrors = new();

            SubscriptionViewModel subscription = await _subscriptionService.GetInactive(id);
            if (subscription == null)
            {
                validationErrors.Add(SubscriptionConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteActive(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<SubscriptionViewModel> subscriptions = await _subscriptionService.GetList(records.Ids);

            if (subscriptions.Where(p => !p.IsActive).Any() || !subscriptions.Select(p => p.Id).ToList().OrderBy(x => x).SequenceEqual(records.Ids.OrderBy(x => x)))
            {
                validationErrors.Add(SubscriptionConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<SubscriptionViewModel> subscriptions = await _subscriptionService.GetList(records.Ids);
            if (subscriptions.Where(p => p.IsActive).Any() || !subscriptions.Select(p => p.Id).ToList().OrderBy(x => x).SequenceEqual(records.Ids.OrderBy(x => x)))
            {
                validationErrors.Add(SubscriptionConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            List<string> validationErrors = new();

            SubscriptionViewModel subscription = await _subscriptionService.GetInactive(id);
            if (subscription == null)
            {
                validationErrors.Add(SubscriptionConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<SubscriptionViewModel> subscriptions = await _subscriptionService.GetList(records.Ids);
            if (subscriptions.Where(p => p.IsActive).Any() || !subscriptions.Select(p => p.Id).ToList().OrderBy(x => x).SequenceEqual(records.Ids.OrderBy(x => x)))
            {
                validationErrors.Add(SubscriptionConstants.EntryInvalid);
            }

            return validationErrors;
        }
    }
}
