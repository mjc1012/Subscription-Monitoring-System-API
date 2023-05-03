using Microsoft.AspNetCore.Mvc;
using static Subscription_Monitoring_System_Data.Constants;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Domain.Contracts;

namespace Subscription_Monitoring_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeAPIController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceTypeAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                List<ServiceTypeDto> responseData = await _unitOfWork.ServiceTypeService.GetList();
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}
