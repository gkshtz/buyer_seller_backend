using BuyerSeller.Service__Business_Logic_Layer_.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BuyerSeller.Data__Data_Access_Layer_.DTOs;
using BuyerSeller.Models;
using BuyerSeller.Application.Validations;
using BuyerSeller.Application.DTO;

namespace BuyerSeller.Application.Controllers
{
    /// <summary>
    /// AuthController
    /// </summary>
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        /// <summary>
        /// Auth Controller Constructor
        /// </summary>
        /// <param name="logger">logger dependency</param>
        /// <param name="authService">Auth service dependency</param>
        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("signup")]
        [HttpPost]
        public ActionResult<User> Add(UserDTO user)
        {
            try
            {
                UserDTOValidator validator = new UserDTOValidator();
                var validationResult = validator.Validate(user);
                if (validationResult != null)
                {
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }   
                }  

                var resultUser = _authService.Add(user);

                CustomResponse<User> response = new CustomResponse<User>()
                {
                    StatusCode = 200,
                    ResponseData = resultUser,
                    ResponseMessage = "Success"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginRequest">Login credentials</param>
        /// <returns>Token</returns>
        [Route("login")]
        [HttpPost]
        public ActionResult Login(LoginRequestDTO loginRequest) 
        {
            try
            {
                LoginDTOValidator validator = new LoginDTOValidator();
                var validationResult = validator.Validate(loginRequest);
                if (validationResult != null)
                {
                    if (!validationResult.IsValid)
                    {
                        return BadRequest(validationResult.Errors);
                    }
                }

                var token = _authService.Login(loginRequest);
                if (token != null)
                {
                    CustomResponse<string> response = new CustomResponse<string>()
                    {
                        StatusCode = 200,
                        ResponseData = token,
                        ResponseMessage = "Success"
                    };

                    return Ok(response);
                }

                CustomResponse<string?> errorResponse = new CustomResponse<string?>()
                {
                    StatusCode = 404,
                    ResponseData = null,
                    ErrorMessage = "Invalid credentials"
                };

                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(LogEvents.ServerError, ex, "Internal Server Error");
                return StatusCode(500);
            }
        }
    }
}
