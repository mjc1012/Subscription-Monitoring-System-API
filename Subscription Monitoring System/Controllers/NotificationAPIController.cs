using Microsoft.AspNetCore.Mvc;
using Subscription_Monitoring_System_Data.Dtos;
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.NotificationHandler.CanDelete(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.NotificationService.HardDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("delete-notifications")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.NotificationHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.NotificationService.HardDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}
