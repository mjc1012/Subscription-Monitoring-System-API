﻿using Microsoft.AspNetCore.Mvc;
using static Subscription_Monitoring_System_Data.Constants;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data;
using System.Net.Mail;
using System.Globalization;
using Microsoft.AspNetCore.StaticFiles;
using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionAPIController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                SubscriptionViewModel responseData = await _unitOfWork.SubscriptionService.GetActive(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("subscriptions")]
        public async Task<IActionResult> GetList(SubscriptionFilterViewModel filter)
        {
            try
            {
                List<string> validationErrors = _unitOfWork.SubscriptionHandler.CanFilter(filter);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                ListViewModel responseData = await _unitOfWork.SubscriptionService.GetList(filter);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("export-excel")]
        public async Task<IActionResult> GetExcel(SubscriptionFilterViewModel filter)
        {
            try
            {
                List<string> validationErrors = _unitOfWork.SubscriptionHandler.CanFilter(filter);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                ListViewModel responseData = await _unitOfWork.SubscriptionService.GetList(filter);
                string emailtemplatepath = Path.Combine(PathConstants.SubscriptionExcelTemplatePath);
                string htmldata = System.IO.File.ReadAllText(emailtemplatepath);

                string excelstring = "";
                foreach (SubscriptionViewModel subscription in (List<SubscriptionViewModel>)responseData.Data)
                {
                    excelstring += SubscriptionConstants.RowData(subscription);
                }
                htmldata = htmldata.Replace(ExcelConstants.ActualData, excelstring);

                string StoredFilePath = Path.Combine(PathConstants.ExcelFilesPath, DateTime.Now.Ticks.ToString() + ExcelConstants.FileType);
                System.IO.File.AppendAllText(StoredFilePath, htmldata);

                var provider = new FileExtensionContentTypeProvider();
                if (!provider.TryGetContentType(StoredFilePath, out var contettype))
                {
                    contettype = ExcelConstants.Contettype;
                }

                var bytes = await System.IO.File.ReadAllBytesAsync(StoredFilePath);

                return File(bytes, contettype, Path.Combine(StoredFilePath));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpGet("subscription-history/{id}")]
        public async Task<IActionResult> GetHistoryList(int id)
        {
            try
            {
                List<SubscriptionViewModel> responseData = await _unitOfWork.SubscriptionService.GetHistoryList(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubscriptionViewModel subscription)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanAdd(subscription);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                ClientViewModel client = await _unitOfWork.ClientService.GetActive(subscription.ClientName);
                ServiceViewModel service = await _unitOfWork.ServiceService.GetActive(subscription.ServiceName);
                UserViewModel createdBy = await _unitOfWork.UserService.GetActive(subscription.CreatedByCode);
                SubscriptionViewModel newSubscription = await _unitOfWork.SubscriptionService.Create(subscription, client, service, createdBy);

                NotificationViewModel notification = new()
                {
                    Description = NotificationConstants.SuccessAdd(newSubscription.Id),
                    Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                    IsActive = true,
                    SubscriptionId = newSubscription.Id,
                };
                await _unitOfWork.NotificationService.Create(notification, subscription.UserIds);

                foreach(ClientViewModel clientRecipient in await _unitOfWork.ClientService.GetList(subscription.ClientIds))
                {
                    _unitOfWork.EmailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.CreateSubject, EmailBody.SendCreatedSubscriptionEmail(NotificationConstants.SuccessAdd(newSubscription.Id), newSubscription)));
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessAdd });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SubscriptionViewModel subscription)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanUpdate(subscription);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                SubscriptionViewModel oldSubscription = await _unitOfWork.SubscriptionService.CreateHistory(subscription.Id);
                List<int> oldUserIds = oldSubscription.UserRecipients.Select(p => p.Id).ToList();
                List<int> oldClientIds = oldSubscription.ClientRecipients.Select(p => p.Id).ToList();
                ClientViewModel client = await _unitOfWork.ClientService.GetActive(subscription.ClientName);
                ServiceViewModel service = await _unitOfWork.ServiceService.GetActive(subscription.ServiceName);
                UserViewModel updatedBy = await _unitOfWork.UserService.GetActive(subscription.UpdatedByCode);
                SubscriptionViewModel newSubscription = await _unitOfWork.SubscriptionService.Update(subscription, client, service, updatedBy);

                NotificationViewModel notification = new()
                {
                    Description = NotificationConstants.SuccessEdit(newSubscription.Id),
                    Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                    IsActive = true,
                    SubscriptionId = newSubscription.Id,
                };
                await _unitOfWork.NotificationService.Create(notification, subscription.UserIds.Union(oldUserIds).ToList());

                foreach (ClientViewModel clientRecipient in await _unitOfWork.ClientService.GetList(subscription.ClientIds.Union(oldClientIds).ToList()))
                {
                    _unitOfWork.EmailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.UpdateSubject, EmailBody.SendUpdatedSubscriptionEmail(clientRecipient.EmailAddress, NotificationConstants.SuccessEdit(newSubscription.Id), newSubscription, await _unitOfWork.ClientService.GetList(subscription.ClientIds))));
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessEdit });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpDelete("hard-delete/{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDeleteInactive(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.SubscriptionService.HardDelete(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpDelete("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDeleteActive(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.SubscriptionService.SoftDelete(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("soft-delete-subscriptions")]
        public async Task<IActionResult> SoftDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.SubscriptionService.SoftDelete(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("hard-delete-subscriptions")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.SubscriptionService.HardDelete(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanRestore(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.SubscriptionService.Restore(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessRestore });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("restore-subscriptions")]
        public async Task<IActionResult> Restore([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanRestore(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.SubscriptionService.Restore(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessRestore });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }
    }
}
