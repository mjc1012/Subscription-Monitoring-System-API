using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Domain.Contracts;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAPIController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveList()
        {
            try
            {
                List<ClientDto> responseData = await _unitOfWork.ClientService.GetActiveList();
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost("clients")]
        public async Task<IActionResult> GetList(ClientFilterDto filter)
        {
            try
            {
                ListDto responseData = await _unitOfWork.ClientService.GetList(filter);
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientDto client)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanAdd(client);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.ClientService.Create(client);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessAdd });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ClientDto client)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanUpdate(client);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.ClientService.Update(client);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessEdit });
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
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanDeleteInactive(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.ClientService.HardDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanDeleteActive(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.ClientService.SoftDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.ClientService.SoftDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.ClientService.HardDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanRestore(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.ClientService.Restore(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessRestore });
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
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanRestore(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.ClientService.Restore(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = ClientConstants.SuccessRestore });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}
