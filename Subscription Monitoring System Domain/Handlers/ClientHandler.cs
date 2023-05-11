using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System.Text.RegularExpressions;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class ClientHandler : IClientHandler
    {
        private readonly IClientService _clientService;
        private readonly IEmailService _emailService;
        public ClientHandler(IClientService clientService, IEmailService emailService)
        {
            _clientService = clientService;
            _emailService = emailService;
        }

        public List<string> CanFilter(ClientFilterViewModel filter)
        {
            List<string> validationErrors = new();

            if (!string.IsNullOrEmpty(filter.SortOrder) && (filter.SortOrder is not SortDirectionConstants.Ascending and not SortDirectionConstants.Descending))
            {
                validationErrors.Add(SortDirectionConstants.SortDirectionInvalid);
            }

            if (!string.IsNullOrEmpty(filter.SortBy) && (filter.SortBy is not ClientConstants.HeaderId and not ClientConstants.NameInvalid and not ClientConstants.HeaderEmailAddress))
            {
                validationErrors.Add(ClientConstants.SortByInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanAdd(ClientViewModel client)
        {
            List<string> validationErrors = new();

            if (client != null && !string.IsNullOrEmpty(client.Name) && !string.IsNullOrEmpty(client.EmailAddress))
            { 

                if (await _clientService.ClientExists(client))
                {
                    validationErrors.Add(ClientConstants.Exists);
                }

                if (Regex.IsMatch(client.Name, @"[\d\W\p{Ll}]"))
                {
                    validationErrors.Add(ClientConstants.NameInvalid);
                }

                try
                {
                    _emailService.SendEmail(new EmailViewModel(client.EmailAddress, EmailConstants.CheckingEmailSubject, EmailBody.CheckEmailBody()));
                }
                catch (Exception)
                {
                    validationErrors.Add(EmailConstants.CannotReachEmail);
                }
            }
            else
            {
                validationErrors.Add(ClientConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(ClientViewModel client)
        {
            List<string> validationErrors = new();

            ClientViewModel clientFound = await _clientService.GetActive(client.Id);
            if (client != null && clientFound != null && !string.IsNullOrEmpty(client.Name) && !string.IsNullOrEmpty(client.EmailAddress) && client.Id > 0)
            {
                if(client.Name == clientFound.Name && client.EmailAddress == clientFound.EmailAddress)
                {
                    validationErrors.Add(ClientConstants.NoChanges);
                }
                else
                {

                    if (await _clientService.ClientExists(client))
                    {
                        validationErrors.Add(ClientConstants.Exists);
                    }

                    if (Regex.IsMatch(client.Name, @"[\d\W\p{Ll}]"))
                    {
                        validationErrors.Add(ClientConstants.NameInvalid);
                    }

                    try
                    {
                        _emailService.SendEmail(new EmailViewModel(client.EmailAddress, EmailConstants.CheckingEmailSubject, EmailBody.CheckEmailBody()));
                    }
                    catch (Exception)
                    {
                        validationErrors.Add(EmailConstants.CannotReachEmail);
                    }
                }
            }
            else
            {
                validationErrors.Add(ClientConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteActive(int id)
        {
            List<string> validationErrors = new();

            ClientViewModel client = await _clientService.GetActive(id);
            if (client == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            List<string> validationErrors = new();

            ClientViewModel client = await _clientService.GetInactive(id);
            if (client == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<ClientViewModel> clients = await _clientService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            List<string> validationErrors = new();

            ClientViewModel client = await _clientService.GetInactive(id);
            if (client == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<ClientViewModel> clients = await _clientService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
