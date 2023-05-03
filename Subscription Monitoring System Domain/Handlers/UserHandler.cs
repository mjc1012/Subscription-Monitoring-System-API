using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public UserHandler(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        public async Task<List<string>> CanEmail(string email)
        {
            var validationErrors = new List<string>();

            if (await _userService.GetByEmail(email) == null)
            {
                validationErrors.Add(UserConstants.EmailAddressDoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanAdd(UserDto user)
        {
            var validationErrors = new List<string>();

            if (user != null)
            {

                if (await _userService.UserExists(user))
                {
                    validationErrors.Add(UserConstants.Exists);
                }

                try
                {
                    _emailService.SendEmail(new EmailDto(user.EmailAddress, "Checking Email", EmailBody.CheckEmailBody()));
                }
                catch (Exception)
                {
                    validationErrors.Add(EmailConstants.CannotReachEmail);
                }
            }
            else
            {
                validationErrors.Add(UserConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(UserDto user)
        {
            var validationErrors = new List<string>();

            if (user != null)
            {

                if (await _userService.UserExists(user))
                {
                    validationErrors.Add(UserConstants.Exists);
                }

                try
                {
                    _emailService.SendEmail(new EmailDto(user.EmailAddress, "Checking Email", EmailBody.CheckEmailBody()));
                }
                catch (Exception)
                {
                    validationErrors.Add(EmailConstants.CannotReachEmail);
                }
            }
            else
            {
                validationErrors.Add(UserConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteActive(int id)
        {
            var validationErrors = new List<string>();

            UserDto client = await _userService.GetActive(id);
            if (client == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            var validationErrors = new List<string>();

            UserDto client = await _userService.GetInactive(id);
            if (client == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsDto records)
        {
            var validationErrors = new List<string>();

            List<UserDto> clients = await _userService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            var validationErrors = new List<string>();

            UserDto client = await _userService.GetInactive(id);
            if (client == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsDto records)
        {
            var validationErrors = new List<string>();

            List<UserDto> clients = await _userService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
