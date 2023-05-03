using Microsoft.AspNetCore.Mvc;
using static Subscription_Monitoring_System_Data.Constants;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Data;
using System.Net.Mail;
using System.Globalization;

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                SubscriptionDto responseData = await _unitOfWork.SubscriptionService.GetActive(id);
                DateTime endDate = DateTime.ParseExact(responseData.EndDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                responseData.RemainingDays = (endDate - DateTime.Now.Date).Days;
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("subscriptions")]
        public async Task<IActionResult> GetList(SubscriptionFilterDto filter)
        {
            try
            {
                ListDto responseData = await _unitOfWork.SubscriptionService.GetList(filter);
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpGet("subscription-history/{id}")]
        public async Task<IActionResult> GetHistoryList(int id)
        {
            try
            {
                List<SubscriptionDto> responseData = await _unitOfWork.SubscriptionService.GetHistoryList(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SubscriptionDto subscription)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanAdd(subscription);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    ClientDto client = await _unitOfWork.ClientService.GetActive(subscription.ClientName);
                    ServiceDto service = await _unitOfWork.ServiceService.GetActive(subscription.ServiceName);
                    UserDto createdBy = await _unitOfWork.UserService.GetActive(subscription.CreatedByCode);
                    SubscriptionDto newSubscription = await _unitOfWork.SubscriptionService.Create(subscription, client, service, createdBy);

                    NotificationDto notification = new()
                    {
                        Description = NotificationConstants.SuccessAdd(newSubscription.Id),
                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        IsActive = true,
                        SubscriptionId = newSubscription.Id,
                    };
                    await _unitOfWork.NotificationService.Create(notification, subscription.UserIds);

                    foreach(ClientDto clientRecipient in await _unitOfWork.ClientService.GetList(subscription.ClientIds))
                    {
                        _unitOfWork.EmailService.SendEmail(new EmailDto(clientRecipient.EmailAddress, "Subscription Created", EmailBody.SendCreatedSubscriptionEmail(NotificationConstants.SuccessAdd(newSubscription.Id), newSubscription)));
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = SubscriptionConstants.SuccessAdd });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SubscriptionDto subscription)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanUpdate(subscription);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    SubscriptionDto oldSubscription = await _unitOfWork.SubscriptionService.CreateHistory(subscription.Id);
                    List<int> oldUserIds = oldSubscription.UserRecipients.Select(p => p.Id).ToList();
                    List<int> oldClientIds = oldSubscription.ClientRecipients.Select(p => p.Id).ToList();
                    ClientDto client = await _unitOfWork.ClientService.GetActive(subscription.ClientName);
                    ServiceDto service = await _unitOfWork.ServiceService.GetActive(subscription.ServiceName);
                    UserDto updatedBy = await _unitOfWork.UserService.GetActive(subscription.UpdatedByCode);
                    SubscriptionDto newSubscription = await _unitOfWork.SubscriptionService.Update(subscription, client, service, updatedBy);

                    NotificationDto notification = new()
                    {
                        Description = NotificationConstants.SuccessEdit(newSubscription.Id),
                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        IsActive = true,
                        SubscriptionId = newSubscription.Id,
                    };
                    await _unitOfWork.NotificationService.Create(notification, subscription.UserIds.Union(oldUserIds).ToList());

                    foreach (ClientDto clientRecipient in await _unitOfWork.ClientService.GetList(subscription.ClientIds.Union(oldClientIds).ToList()))
                    {
                        _unitOfWork.EmailService.SendEmail(new EmailDto(clientRecipient.EmailAddress, "Subscription Updated", EmailBody.SendUpdatedSubscriptionEmail(clientRecipient.EmailAddress, NotificationConstants.SuccessEdit(newSubscription.Id), newSubscription, await _unitOfWork.ClientService.GetList(subscription.ClientIds))));
                    }

                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = SubscriptionConstants.SuccessEdit });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpDelete("hard-deletion/{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDeleteInactive(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.SubscriptionService.HardDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = SubscriptionConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpDelete("soft-deletion/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDeleteActive(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.SubscriptionService.SoftDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = SubscriptionConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("soft-delete-subscriptions")]
        public async Task<IActionResult> SoftDelete([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.SubscriptionService.SoftDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = SubscriptionConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("hard-delete-subscriptions")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.SubscriptionService.HardDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = SubscriptionConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
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
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.SubscriptionService.Restore(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = SubscriptionConstants.SuccessRestore });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("restore-subscriptions")]
        public async Task<IActionResult> Restore([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.SubscriptionHandler.CanRestore(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.SubscriptionService.Restore(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = SubscriptionConstants.SuccessRestore });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}
