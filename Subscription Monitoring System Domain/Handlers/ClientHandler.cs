using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.Models;
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
    public class ClientHandler : IClientHandler
    {
        private readonly IClientService _clientService;
        private readonly IEmailService _emailService;
        public ClientHandler(IClientService clientService, IEmailService emailService)
        {
            _clientService = clientService;
            _emailService = emailService;
        }
        public async Task<List<string>> CanAdd(ClientViewModel client)
        {
            var validationErrors = new List<string>();

            if (client != null)
            {

                if (await _clientService.ClientExists(client))
                {
                    validationErrors.Add(ClientConstants.Exists);
                }

                try
                {
                    _emailService.SendEmail(new EmailViewModel(client.EmailAddress, "Checking Email", EmailBody.CheckEmailBody()));
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
            var validationErrors = new List<string>();

            if (client != null)
            {

                if (await _clientService.ClientExists(client))
                {
                    validationErrors.Add(ClientConstants.Exists);
                }

                try
                {
                    _emailService.SendEmail(new EmailViewModel(client.EmailAddress, "Checking Email", EmailBody.CheckEmailBody()));
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

        public async Task<List<string>> CanDeleteActive(int id)
        {
            var validationErrors = new List<string>();

            ClientViewModel client = await _clientService.GetActive(id);
            if (client == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            var validationErrors = new List<string>();

            ClientViewModel client = await _clientService.GetInactive(id);
            if (client == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<ClientViewModel> clients = await _clientService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            var validationErrors = new List<string>();

            ClientViewModel client = await _clientService.GetInactive(id);
            if (client == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<ClientViewModel> clients = await _clientService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(ClientConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
