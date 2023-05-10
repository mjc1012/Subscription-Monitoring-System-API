using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Subscription_Monitoring_System_Data.ViewModels;
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

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActive(int id)
        {
            try
            {
                ClientViewModel responseData = await _unitOfWork.ClientService.GetActive(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetActiveList()
        {
            try
            {
                List<ClientViewModel> responseData = await _unitOfWork.ClientService.GetActiveList();
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("clients")]
        public async Task<IActionResult> GetList(ClientFilterViewModel filter)
        {
            try
            {
                List<string> validationErrors = _unitOfWork.ClientHandler.CanFilter(filter);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                ListViewModel responseData = await _unitOfWork.ClientService.GetList(filter);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientViewModel client)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanAdd(client);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                
                await _unitOfWork.ClientService.Create(client);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessAdd });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ClientViewModel client)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanUpdate(client);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.ClientService.Update(client);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessEdit });
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
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanDeleteInactive(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.ClientService.HardDelete(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanDeleteActive(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.ClientService.SoftDelete(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("soft-delete-clients")]
        public async Task<IActionResult> SoftDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.ClientService.SoftDelete(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("hard-delete-clients")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.ClientService.HardDelete(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanRestore(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.ClientService.Restore(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessRestore });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("restore-clients")]
        public async Task<IActionResult> Restore([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.ClientHandler.CanRestore(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.ClientService.Restore(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = ClientConstants.SuccessRestore });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }
    }
}
