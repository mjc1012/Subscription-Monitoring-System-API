﻿using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using System.Text.RegularExpressions;
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
            List<string> validationErrors = new();

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
            List<string> validationErrors = new();

            if (await _userService.GetByEmail(email) == null)
            {
                validationErrors.Add(UserConstants.EmailAddressDoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanAdd(UserViewModel user)
        {
            List<string> validationErrors = new();

            if (user != null && !string.IsNullOrEmpty(user.Code) && !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)
                && !string.IsNullOrEmpty(user.EmailAddress) && !string.IsNullOrEmpty(user.DepartmentName))
            {

                if (await _userService.UserExists(user))
                {
                    validationErrors.Add(UserConstants.Exists);
                }

                if (!await _departmentService.DepartmentExists(new DepartmentViewModel { Name = user.DepartmentName }))
                {
                    validationErrors.Add(DepartmentConstants.DoesNotExist);
                }

                if (Regex.IsMatch(user.FirstName, "[a-z0-9,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                {
                    validationErrors.Add(UserConstants.FirstNameInvalid);
                }

                if(user.MiddleName != null)
                {
                    if (Regex.IsMatch(user.MiddleName, "[a-z0-9,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                    {
                        validationErrors.Add(UserConstants.MiddleNameInvalid);
                    }
                }

                if (Regex.IsMatch(user.LastName, "[a-z0-9,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                {
                    validationErrors.Add(UserConstants.LastNameInvalid);
                }

                try
                {
                    _emailService.SendEmail(new EmailViewModel(user.EmailAddress, EmailConstants.CheckingEmailSubject, EmailBody.CheckEmailBody()));
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
            List<string> validationErrors = new();

            UserViewModel userFound = await _userService.GetActive(user.Id);
            if (user != null && userFound != null && !string.IsNullOrEmpty(user.Code) && !string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)
                && !string.IsNullOrEmpty(user.EmailAddress) && !string.IsNullOrEmpty(user.DepartmentName) && user.Id > 0)
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

                    if (!await _departmentService.DepartmentExists(new DepartmentViewModel { Name = user.DepartmentName }))
                    {
                        validationErrors.Add(DepartmentConstants.DoesNotExist);
                    }

                    if (Regex.IsMatch(user.FirstName, "[a-z0-9,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                    {
                        validationErrors.Add(UserConstants.FirstNameInvalid);
                    }

                    if (user.MiddleName != null)
                    {
                        if (Regex.IsMatch(user.MiddleName, "[a-z0-9,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                        {
                            validationErrors.Add(UserConstants.MiddleNameInvalid);
                        }
                    }

                    if (Regex.IsMatch(user.LastName, "[a-z0-9,~,',!,@,#,$,%,^,&,*,(,),-,_,+,=,{,},\\[,\\],|,/,\\,:,;,\",`,<,>,,,.,?]"))
                    {
                        validationErrors.Add(UserConstants.LastNameInvalid);
                    }

                    try
                    {
                        _emailService.SendEmail(new EmailViewModel(user.EmailAddress, EmailConstants.CheckingEmailSubject, EmailBody.CheckEmailBody()));
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
            List<string> validationErrors = new();

            UserViewModel user = await _userService.GetActive(id);
            if (user == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(int id)
        {
            List<string> validationErrors = new();

            UserViewModel user = await _userService.GetInactive(id);
            if (user == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteActive(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<UserViewModel> users = await _userService.GetList(records.Ids);

            if (users.Where(p => !p.IsActive).Any() || !users.Select(p => p.Id).ToList().OrderBy(x => x).SequenceEqual(records.Ids.OrderBy(x => x)))
            {
                validationErrors.Add(UserConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDeleteInactive(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<UserViewModel> users = await _userService.GetList(records.Ids);

            if (users.Where(p => p.IsActive).Any() || !users.Select(p => p.Id).ToList().OrderBy(x => x).SequenceEqual(records.Ids.OrderBy(x => x)))
            {
                validationErrors.Add(UserConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(int id)
        {
            List<string> validationErrors = new();

            UserViewModel user = await _userService.GetInactive(id);
            if (user == null)
            {
                validationErrors.Add(UserConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanRestore(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<UserViewModel> users = await _userService.GetList(records.Ids);
            if (users.Where(p => p.IsActive).Any() || !users.Select(p => p.Id).ToList().OrderBy(x => x).SequenceEqual(records.Ids.OrderBy(x => x)))
            {
                validationErrors.Add(UserConstants.EntryInvalid);
            }

            return validationErrors;
        }
    }
}
