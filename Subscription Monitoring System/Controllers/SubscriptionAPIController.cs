using Microsoft.AspNetCore.Mvc;
using static Subscription_Monitoring_System_Data.Constants;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Data;
using Microsoft.AspNetCore.StaticFiles;
using Subscription_Monitoring_System_Data.ViewModels;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
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

        [Authorize]
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

        [Authorize]
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

                List<SubscriptionViewModel> subscriptions = await _unitOfWork.SubscriptionService.GetListForExcel(filter);
                string emailtemplatepath = Path.Combine(PathConstants.SubscriptionExcelTemplatePath);
                string htmldata = System.IO.File.ReadAllText(emailtemplatepath);

                string excelstring = "";
                foreach (SubscriptionViewModel subscription in subscriptions)
                {
                    excelstring += SubscriptionConstants.RowData(subscription);
                }
                htmldata = htmldata.Replace(ExcelConstants.ActualData, excelstring);

                string StoredFilePath = Path.Combine(PathConstants.ExcelFilesPath, DateTime.Now.Ticks.ToString() + ExcelConstants.FileType);
                System.IO.File.AppendAllText(StoredFilePath, htmldata);

                FileExtensionContentTypeProvider provider = new();
                if (!provider.TryGetContentType(StoredFilePath, out string contettype))
                {
                    contettype = ExcelConstants.Contettype;
                }

                byte[] bytes = await System.IO.File.ReadAllBytesAsync(StoredFilePath);

                return File(bytes, contettype, Path.Combine(StoredFilePath));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
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

        [Authorize]
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
                    UserIds = subscription.UserIds
                };
                await _unitOfWork.NotificationService.Create(notification);

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

        [Authorize]
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
                    UserIds = subscription.UserIds.Union(oldUserIds).ToList()
                };
                await _unitOfWork.NotificationService.Create(notification);

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

        [Authorize]
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

                SubscriptionViewModel subscription = await _unitOfWork.SubscriptionService.GetInactive(id);
                await _unitOfWork.SubscriptionService.HardDelete(id);
                if(subscription.SubscriptionHistoryId == null)
                {
                    NotificationViewModel notification = new()
                    {
                        Description = NotificationConstants.SuccessPermanentDelete(subscription.Id),
                        Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                        IsActive = true,
                        UserIds = subscription.UserRecipients.Select(p => p.Id).ToList()
                    };
                    await _unitOfWork.NotificationService.Create(notification);

                    foreach (ClientViewModel clientRecipient in subscription.ClientRecipients)
                    {
                        _unitOfWork.EmailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.PermanentDeleteSubject, EmailBody.SendSubscriptionEmail(NotificationConstants.PermanentDeleteSubject, NotificationConstants.SuccessPermanentDelete(subscription.Id))));
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
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

                SubscriptionViewModel subscription = await _unitOfWork.SubscriptionService.GetActive(id);
                await _unitOfWork.SubscriptionService.SoftDelete(id);
                if (subscription.SubscriptionHistoryId == null)
                {
                    NotificationViewModel notification = new()
                    {
                        Description = NotificationConstants.SuccessTemporaryDelete(subscription.Id),
                        Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                        IsActive = true,
                        SubscriptionId = subscription.Id,
                        UserIds = subscription.UserRecipients.Select(p => p.Id).ToList()
                    };
                    await _unitOfWork.NotificationService.Create(notification);

                    foreach (ClientViewModel clientRecipient in subscription.ClientRecipients)
                    {
                        _unitOfWork.EmailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.TemporaryDeleteSubject, EmailBody.SendSubscriptionEmail(NotificationConstants.TemporaryDeleteSubject, NotificationConstants.SuccessTemporaryDelete(subscription.Id))));
                    }
                }
                    

                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("soft-delete-subscriptions")]
        public async Task<IActionResult> SoftDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDeleteActive(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.SubscriptionService.SoftDelete(records);
                foreach (int id in records.Ids)
                {
                    SubscriptionViewModel subscription = await _unitOfWork.SubscriptionService.GetInactive(id);
                    if (subscription.SubscriptionHistoryId == null)
                    {
                        NotificationViewModel notification = new()
                        {
                            Description = NotificationConstants.SuccessTemporaryDelete(subscription.Id),
                            Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                            IsActive = true,
                            SubscriptionId = subscription.Id,
                            UserIds = subscription.UserRecipients.Select(p => p.Id).ToList()
                        };
                        await _unitOfWork.NotificationService.Create(notification);

                        foreach (ClientViewModel clientRecipient in subscription.ClientRecipients)
                        {
                            _unitOfWork.EmailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.TemporaryDeleteSubject, EmailBody.SendSubscriptionEmail(NotificationConstants.TemporaryDeleteSubject, NotificationConstants.SuccessTemporaryDelete(subscription.Id))));
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("hard-delete-subscriptions")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDeleteInactive(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                List<SubscriptionViewModel> subscriptions = await _unitOfWork.SubscriptionService.GetList(records.Ids);
                await _unitOfWork.SubscriptionService.HardDelete(records);

                foreach (SubscriptionViewModel subscription in subscriptions)
                {
                    if (subscription.SubscriptionHistoryId == null)
                    {
                        NotificationViewModel notification = new()
                        {
                            Description = NotificationConstants.SuccessPermanentDelete(subscription.Id),
                            Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                            IsActive = true,
                            UserIds = subscription.UserRecipients.Select(p => p.Id).ToList()
                        };
                        await _unitOfWork.NotificationService.Create(notification);

                        foreach (ClientViewModel clientRecipient in subscription.ClientRecipients)
                        {
                            _unitOfWork.EmailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.PermanentDeleteSubject, EmailBody.SendSubscriptionEmail(NotificationConstants.PermanentDeleteSubject, NotificationConstants.SuccessPermanentDelete(subscription.Id))));
                        }
                    }  
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
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

                SubscriptionViewModel subscription = await _unitOfWork.SubscriptionService.GetInactive(id);

                await _unitOfWork.SubscriptionService.Restore(id);
                if (subscription.SubscriptionHistoryId == null)
                {
                    NotificationViewModel notification = new()
                    {
                        Description = NotificationConstants.SuccessRestore(subscription.Id),
                        Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                        IsActive = true,
                        SubscriptionId = subscription.Id,
                        UserIds = subscription.UserRecipients.Select(p => p.Id).ToList()
                    };
                    await _unitOfWork.NotificationService.Create(notification);

                    foreach (ClientViewModel clientRecipient in subscription.ClientRecipients)
                    {
                        _unitOfWork.EmailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.RestoredSubject, EmailBody.SendSubscriptionEmail(NotificationConstants.RestoredSubject, NotificationConstants.SuccessRestore(subscription.Id))));
                    }
                }
                    
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessRestore });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
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
                foreach (int id in records.Ids)
                {
                    SubscriptionViewModel subscription = await _unitOfWork.SubscriptionService.GetActive(id);
                    if (subscription.SubscriptionHistoryId == null)
                    {
                        NotificationViewModel notification = new()
                        {
                            Description = NotificationConstants.SuccessRestore(subscription.Id),
                            Date = DateTime.Now.ToString(DateConstants.DateTimeFormat),
                            IsActive = true,
                            SubscriptionId = subscription.Id,
                            UserIds = subscription.UserRecipients.Select(p => p.Id).ToList()
                        };
                        await _unitOfWork.NotificationService.Create(notification);

                        foreach (ClientViewModel clientRecipient in subscription.ClientRecipients)
                        {
                            _unitOfWork.EmailService.SendEmail(new EmailViewModel(clientRecipient.EmailAddress, NotificationConstants.RestoredSubject, EmailBody.SendSubscriptionEmail(NotificationConstants.RestoredSubject, NotificationConstants.SuccessRestore(subscription.Id))));
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = SubscriptionConstants.SuccessRestore });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }
    }
}
