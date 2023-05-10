using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentAPIController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            try
            {
                List<DepartmentViewModel> responseData = await _unitOfWork.DepartmentService.GetList();
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                DepartmentViewModel responseData = await _unitOfWork.DepartmentService.Get(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DepartmentViewModel department)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.DepartmentHandler.CanAdd(department);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }


                await _unitOfWork.DepartmentService.Create(department);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = DepartmentConstants.SuccessAdd });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DepartmentViewModel department)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.DepartmentHandler.CanUpdate(department);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }


                await _unitOfWork.DepartmentService.Update(department);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = DepartmentConstants.SuccessAdd });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.DepartmentHandler.CanDelete(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.DepartmentService.HardDelete(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = DepartmentConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("hard-delete-departments")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.DepartmentHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.DepartmentService.HardDelete(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = DepartmentConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }
    }
}
