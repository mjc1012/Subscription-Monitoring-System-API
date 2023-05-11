using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Domain.Services;
using System.Text.RegularExpressions;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Handlers
{
    public class DepartmentHandler : IDepartmentHandler
    {

        private readonly IDepartmentService _departmentService;
        public DepartmentHandler(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        public async Task<List<string>> CanAdd(DepartmentViewModel department)
        {
            List<string> validationErrors = new();

            if (department != null && !string.IsNullOrEmpty(department.Name))
            {
                if (Regex.IsMatch(department.Name, @"[\p{Ll}\d\W&&[^ ]]"))
                {
                    validationErrors.Add(DepartmentConstants.NameInvalid);
                }

                if (await _departmentService.DepartmentExists(department))
                {
                    validationErrors.Add(DepartmentConstants.Exists);
                }
            }
            else
            {
                validationErrors.Add(DepartmentConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanUpdate(DepartmentViewModel department)
        {
            List<string> validationErrors = new();

            DepartmentViewModel departmentFound = await _departmentService.Get(department.Id);
            if (department != null && departmentFound != null && !string.IsNullOrEmpty(department.Name) && department.Id > 0)
            {
                if (department.Name == departmentFound.Name)
                {
                    validationErrors.Add(DepartmentConstants.NoChanges);
                }
                else
                {

                    if (Regex.IsMatch(department.Name, @"[\p{Ll}\d\W&&[^ ]]"))
                    {
                        validationErrors.Add(DepartmentConstants.NameInvalid);
                    }

                    if (await _departmentService.DepartmentExists(department))
                    {
                        validationErrors.Add(DepartmentConstants.Exists);
                    }
                }
            }
            else
            {
                validationErrors.Add(DepartmentConstants.EntryInvalid);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(int id)
        {
            List<string> validationErrors = new();

            DepartmentViewModel service = await _departmentService.Get(id);
            if (service == null)
            {
                validationErrors.Add(DepartmentConstants.DoesNotExist);
            }

            return validationErrors;
        }

        public async Task<List<string>> CanDelete(RecordIdsViewModel records)
        {
            List<string> validationErrors = new();

            List<DepartmentViewModel> services = await _departmentService.GetList(records.Ids);
            if (services == null)
            {
                validationErrors.Add(DepartmentConstants.DoesNotExist);
            }

            return validationErrors;
        }
    }
}
