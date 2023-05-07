using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationAPIController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.NotificationHandler.CanDelete(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.NotificationService.HardDelete(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("delete-notifications")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.NotificationHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.NotificationService.HardDelete(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }
    }
}
