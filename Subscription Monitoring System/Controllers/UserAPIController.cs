using Microsoft.AspNetCore.Mvc;
using static Subscription_Monitoring_System_Data.Constants;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Data;
using Subscription_Monitoring_System_Data.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Subscription_Monitoring_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAPIController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetActive(int id)
        {
            try
            {
                UserViewModel responseData = await _unitOfWork.UserService.GetActive(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("user/{code}")]
        public async Task<IActionResult> GetActive(string code)
        {
            try
            {
                UserViewModel responseData = await _unitOfWork.UserService.GetActive(code);
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
                List<UserViewModel> responseData = await _unitOfWork.UserService.GetActiveList();
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("users")]
        public async Task<IActionResult> GetList(UserFilterViewModel filter)
        {
            try
            {
                List<string> validationErrors = _unitOfWork.UserHandler.CanFilter(filter);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                ListViewModel responseData = await _unitOfWork.UserService.GetList(filter);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = BaseConstants.RetrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] UserViewModel user)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanAdd(user);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                user.ProfilePictureImageName = (user.ImageFile == null)? ImageConstants.DefaultImage : _unitOfWork.ImageService.SaveImage(user.ImageFile);
                DepartmentViewModel department = await _unitOfWork.DepartmentService.Get(user.DepartmentName);
                await _unitOfWork.UserService.Create(user, department);
                _unitOfWork.EmailService.SendEmail(new EmailViewModel(user.EmailAddress, UserConstants.AccountCredentials, EmailBody.CreateCredentialsEmailBody(user.Code, PasswordConstants.CreateTemporaryPassword(user.LastName))));
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = UserConstants.SuccessAdd });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromForm] UserViewModel newUser)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanUpdate(newUser);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }
                UserViewModel oldUser = await _unitOfWork.UserService.GetActive(newUser.Id);
                if (newUser.ImageFile != null)
                {
                    if (newUser.ProfilePictureImageName != ImageConstants.DefaultImage)
                    {
                        _unitOfWork.ImageService.DeleteImage(oldUser);
                    }
                    newUser.ProfilePictureImageName = _unitOfWork.ImageService.SaveImage(newUser.ImageFile);
                }
                DepartmentViewModel department = await _unitOfWork.DepartmentService.Get(newUser.DepartmentName);
                await _unitOfWork.UserService.Update(newUser, oldUser, department);
                if(newUser.EmailAddress != oldUser.EmailAddress) _unitOfWork.EmailService.SendEmail(new EmailViewModel(newUser.EmailAddress, UserConstants.UpdatedAccountCredentials, EmailBody.UpdateCredentialsEmailBody(newUser.Code, PasswordConstants.CreateTemporaryPassword(newUser.LastName))));
                else if (newUser.Code != oldUser.Code) _unitOfWork.EmailService.SendEmail(new EmailViewModel(newUser.EmailAddress, UserConstants.UpdatedAccountCredentials, EmailBody.UpdateCredentialsEmailBody(newUser.Code)));
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = UserConstants.SuccessEdit });
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
                List<string> validationErrors = await _unitOfWork.UserHandler.CanDeleteInactive(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                UserViewModel user = await _unitOfWork.UserService.GetInactive(id);
                if (user.ProfilePictureImageName != ImageConstants.DefaultImage) _unitOfWork.ImageService.DeleteImage(user);
                await _unitOfWork.UserService.HardDelete(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = UserConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.UserHandler.CanDeleteActive(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.UserService.SoftDelete(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = UserConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("soft-delete-users")]
        public async Task<IActionResult> SoftDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.UserService.SoftDelete(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = UserConstants.SuccessDelete });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("hard-delete-users")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                List<UserViewModel> users = await _unitOfWork.UserService.GetList(records.Ids);
                foreach(UserViewModel user in users)
                {
                    if (user.ProfilePictureImageName != ImageConstants.DefaultImage) _unitOfWork.ImageService.DeleteImage(user);
                }
                await _unitOfWork.UserService.HardDelete(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = UserConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.UserHandler.CanRestore(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.UserService.Restore(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = UserConstants.SuccessRestore });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("restore-users")]
        public async Task<IActionResult> Restore([FromBody] RecordIdsViewModel records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanRestore(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = false, Message = BaseConstants.ErrorList, Value = validationErrors });
                }

                await _unitOfWork.UserService.Restore(records);
                return StatusCode(StatusCodes.Status200OK, new ResponseViewModel() { Status = true, Message = UserConstants.SuccessRestore });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseViewModel() { Status = false, Message = ex.Message });
            }
        }
    }
}
