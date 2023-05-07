using Subscription_Monitoring_System_Data;
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
    public class UserHandler : IUserHandler
    {
        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly IEmailService _emailService;
        public UserHandler(IUserService userService, IEmailService emailService, IDepartmentService departmentService)
        {
            _userService = userService;
            _emailService = emailService;
            _departmentService = departmentService;
        }

        public List<string> CanFilter(UserFilterViewModel filter)
        {
            var validationErrors = new List<string>();

            if (!string.IsNullOrEmpty(filter.SortOrder) && (filter.SortOrder is not SortDirectionConstants.Ascending and not SortDirectionConstants.Descending))
            {
                validationErrors.Add(SortDirectionConstants.SortDirectionInvalid);
            }

            if (!string.IsNullOrEmpty(filter.SortBy) && (filter.SortBy is not UserConstants.HeaderId and not UserConstants.HeaderCode and not UserConstants.HeaderEmailAddress and not UserConstants.HeaderFirstName
                and not UserConstants.HeaderMiddleName and not UserConstants.HeaderLastName and not UserConstants.HeaderDepartmentName))
            {
                validationErrors.Add(UserConstants.SortByInvalid);
            }

            return validationErrors;
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

        public async Task<List<string>> CanAdd(UserViewModel user)
        {
            var validationErrors = new List<string>();

            if (user != null)
            {

                if (await _userService.UserExists(user))
                {
                    validationErrors.Add(UserConstants.Exists);
                }

                if (!await _departmentService.DepartmentExists(user.DepartmentName))
                {
                    validationErrors.Add(DepartmentConstants.DoesNotExist);
                }

                Match match = Regex.Match(user.FirstName, "[^A-Z]");
                if (match.Success)
                {
                    validationErrors.Add(UserConstants.FirstNameInvalid);
                }

                if(user.MiddleName != null)
                {
                    match = Regex.Match(user.MiddleName, "[^A-Z]");
                    if (match.Success)
                    {
                        validationErrors.Add(UserConstants.MiddleNameInvalid);
                    }
                }

                match = Regex.Match(user.LastName, "[^A-Z]");
                if (match.Success)
                {
                    validationErrors.Add(UserConstants.LastNameInvalid);
                }

                try
                {
                    _emailService.SendEmail(new EmailViewModel(user.EmailAddress, "Checking Email", EmailBody.CheckEmailBody()));
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

        public async Task<List<string>> CanUpdate(UserViewModel user)
        {
            var validationErrors = new List<string>();

            UserViewModel userFound = await _userService.GetActive(user.Id);
            if (user != null && userFound != null)
            {
                if (user.Code == userFound.Code && user.FirstName == userFound.FirstName && user.MiddleName == userFound.MiddleName && user.LastName == userFound.LastName &&
                    user.EmailAddress == userFound.EmailAddress && user.DepartmentName == userFound.DepartmentName)
                {
                    validationErrors.Add(UserConstants.NoChanges);
                }
                else
                {
                    if (await _userService.UserExists(user))
                    {
                        validationErrors.Add(UserConstants.Exists);
                    }

                    if (!await _departmentService.DepartmentExists(user.DepartmentName))
                    {
                        validationErrors.Add(DepartmentConstants.DoesNotExist);
                    }

                    Match match = Regex.Match(user.FirstName, "[^A-Z]");
                    if (match.Success)
                    {
                        validationErrors.Add(UserConstants.FirstNameInvalid);
                    }

                    if (user.MiddleName != null)
                    {
                        match = Regex.Match(user.MiddleName, "[^A-Z]");
                        if (match.Success)
                        {
                            validationErrors.Add(UserConstants.MiddleNameInvalid);
                        }
                    }

                    match = Regex.Match(user.LastName, "[^A-Z]");
                    if (match.Success)
                    {
                        validationErrors.Add(UserConstants.LastNameInvalid);
                    }

                    try
                    {
                        _emailService.SendEmail(new EmailViewModel(user.EmailAddress, "Checking Email", EmailBody.CheckEmailBody()));
                    }
                    catch (Exception)
                    {
                        validationErrors.Add(EmailConstants.CannotReachEmail);
                    }
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

            UserViewModel client = await _userService.GetActive(id);
            if (client == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            var validationErrors = new List<string>();

            UserViewModel client = await _userService.GetInactive(id);
            if (client == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<UserViewModel> clients = await _userService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            var validationErrors = new List<string>();

            UserViewModel client = await _userService.GetInactive(id);
            if (client == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            var validationErrors = new List<string>();

            List<UserViewModel> clients = await _userService.GetList(records.Ids);
            if (clients == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
