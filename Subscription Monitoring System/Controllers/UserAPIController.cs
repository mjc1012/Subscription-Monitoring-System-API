using Microsoft.AspNetCore.Mvc;
using static Subscription_Monitoring_System_Data.Constants;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Domain.Contracts;
using Subscription_Monitoring_System_Data;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActive(int id)
        {
            try
            {
                UserDto responseData = await _unitOfWork.UserService.GetActive(id);
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetActiveList()
        {
            try
            {
                List<UserDto> responseData = await _unitOfWork.UserService.GetActiveList();
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("users")]
        public async Task<IActionResult> GetList(UserFilterDto filter)
        {
            try
            {
                ListDto responseData = await _unitOfWork.UserService.GetList(filter);
                return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = BaseConstants.retrievedData, Value = responseData });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] UserDto user)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanAdd(user);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    user.ProfilePictureImageName = (user.ImageFile == null)? ImageConstants.DefaultImage : _unitOfWork.ImageService.SaveImage(user.ImageFile);
                    DepartmentDto department = await _unitOfWork.DepartmentService.Get(user.DepartmentName);
                    await _unitOfWork.UserService.Create(user, department);
                    _unitOfWork.EmailService.SendEmail(new EmailDto(user.EmailAddress, "Account Credentials", EmailBody.CreateCredentialsEmailBody(user.Code, PasswordHasher.CreateTemporaryPassword(user.LastName))));
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = UserConstants.SuccessAdd });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] UserDto user)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanUpdate(user);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    if (user.ImageFile != null)
                    {
                        if (user.ProfilePictureImageName != ImageConstants.DefaultImage)
                        {
                            UserDto imageOwner = await _unitOfWork.UserService.GetActive(user.Id);
                            _unitOfWork.ImageService.DeleteImage(imageOwner);
                        }
                        user.ProfilePictureImageName = _unitOfWork.ImageService.SaveImage(user.ImageFile);
                    }
                    DepartmentDto department = await _unitOfWork.DepartmentService.Get(user.DepartmentName);
                    await _unitOfWork.UserService.Update(user, department);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = UserConstants.SuccessEdit });
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
                List<string> validationErrors = await _unitOfWork.UserHandler.CanDeleteInactive(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    UserDto user = await _unitOfWork.UserService.GetInactive(id);
                    if (user.ProfilePictureImageName != ImageConstants.DefaultImage) _unitOfWork.ImageService.DeleteImage(user);
                    await _unitOfWork.UserService.HardDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = UserConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.UserHandler.CanDeleteActive(id);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.UserService.SoftDelete(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = UserConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("soft-delete-users")]
        public async Task<IActionResult> SoftDelete([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.UserService.SoftDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = UserConstants.SuccessDelete });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("hard-delete-users")]
        public async Task<IActionResult> HardDelete([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanDelete(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    List<UserDto> users = await _unitOfWork.UserService.GetList(records.Ids);
                    foreach(UserDto user in users)
                    {
                        if (user.ProfilePictureImageName != ImageConstants.DefaultImage) _unitOfWork.ImageService.DeleteImage(user);
                    }
                    await _unitOfWork.UserService.HardDelete(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = UserConstants.SuccessDelete });
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
                List<string> validationErrors = await _unitOfWork.UserHandler.CanRestore(id);


                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {
                    await _unitOfWork.UserService.Restore(id);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = UserConstants.SuccessRestore });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }

        [HttpPut("restore-users")]
        public async Task<IActionResult> Restore([FromBody] RecordIdsDto records)
        {
            try
            {
                List<string> validationErrors = await _unitOfWork.UserHandler.CanRestore(records);

                if (validationErrors.Any())
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = false, Message = BaseConstants.errorList, Value = validationErrors });
                }
                else
                {

                    await _unitOfWork.UserService.Restore(records);
                    return StatusCode(StatusCodes.Status200OK, new ResponseDto() { Status = true, Message = UserConstants.SuccessRestore });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto() { Status = false, Message = ex.Message });
            }
        }
    }
}
