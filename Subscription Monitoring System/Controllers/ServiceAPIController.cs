using Microsoft.AspNetCore.Mvc;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceAPIController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveList()
        {
            try
            {
                List<ServiceViewModel> responseData = await _unitOfWork.ServiceService.GetActiveList();
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("services")]
        public async Task<IActionResult> GetList(ServiceFilterViewModel filter)
        {
            try
            {
                ListViewModel responseData = await _unitOfWork.ServiceService.GetList(filter);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ServiceViewModel service)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ServiceHandler.CanAdd(service);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    ServiceTypeViewModel serviceType = await _unitOfWork.ServiceTypeService.Get(service.ServiceTypeName);
                    await _unitOfWork.ServiceService.Create(service, serviceType);
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ServiceConstants.SuccessAdd });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ServiceViewModel service)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ServiceHandler.CanUpdate(service);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    ServiceTypeViewModel serviceType = await _unitOfWork.ServiceTypeService.Get(service.ServiceTypeName);
                    await _unitOfWork.ServiceService.Update(service, serviceType);
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ServiceConstants.SuccessEdit });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpDelete("hard-deletion/{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ServiceHandler.CanDeleteInactive(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.ServiceService.HardDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ServiceConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpDelete("soft-deletion/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ServiceHandler.CanDeleteActive(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.ServiceService.SoftDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ServiceConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("soft-delete-services")]
        public async Task<IActionResult> SoftDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ServiceHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.ServiceService.SoftDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ServiceConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("hard-delete-services")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ServiceHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.ServiceService.HardDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ServiceConstants.SuccessDelete });
                }
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
                List<string> validationErrors = await _unitOfWork.ServiceHandler.CanRestore(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.ServiceService.Restore(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ServiceConstants.SuccessRestore });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("restore-services")]
        public async Task<IActionResult> Restore([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ServiceHandler.CanRestore(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.ServiceService.Restore(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ServiceConstants.SuccessRestore });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }
    }
}
