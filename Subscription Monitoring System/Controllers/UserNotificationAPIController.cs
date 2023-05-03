using Microsoft.AspNetCore.Mvc;
using static Subscription_Monitoring_System_Data.Constants;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Data.Models;

namespace Subscription_Monitoring_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNotificationAPIController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserNotificationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetList(int id)
        {
            try
            {
                List<UserNotification> responseData = await _unitOfWork.UserNotificationService.GetList(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
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
                List<string> validationErrors = await _unitOfWork.UserNotificationHandler.CanDeleteInactive(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.UserNotificationService.HardDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = NotificationConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.UserNotificationHandler.CanDeleteActive(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.UserNotificationService.SoftDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = NotificationConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("soft-delete-clients")]
        public async Task<IActionResult> SoftDelete([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserNotificationHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.UserNotificationService.SoftDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = NotificationConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("hard-delete-clients")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserNotificationHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.UserNotificationService.HardDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = NotificationConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.UserNotificationHandler.CanRestore(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.UserNotificationService.Restore(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = NotificationConstants.SuccessRestore });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("restore-clients")]
        public async Task<IActionResult> Restore([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserNotificationHandler.CanRestore(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.UserNotificationService.Restore(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = NotificationConstants.SuccessRestore });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}
