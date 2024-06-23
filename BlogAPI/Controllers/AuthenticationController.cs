using BlogAPI.Models;
using BlogAPI.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.Server.Controllers
{
    [Route("api/user")]
    public class AuthenticationController : Controller
    {

        private readonly IAuthService _authService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthService authService, ILogger<AuthenticationController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Payload");

                var (status, response) = await _authService.Login(model);

                if (status == 0)
                    return BadRequest(new
                    {
                        success = false,
                        code = status,
                        message = response,
                    });

                return Ok(new
                {
                    success = true,
                    code = status,
                    message = "Login successful",
                    data = response,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }


        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model, string? keyword)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        success = false,
                        code = 0,
                        message = "Registration unsuccessful",
                    });

                var (status, response) = await _authService.Registration(model, keyword == null ? UserRoles.User : UserRoles.Admin);

                if (status == 0)
                    return BadRequest(new
                    {
                        success = false,
                        code = status,
                        message = response,
                    });

                return Ok(new
                {
                    success = false,
                    code = status,
                    message = "Registration successful",
                    data = response,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }
    }
}

