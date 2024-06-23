using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogAPI.Models;
using BlogAPI.Models.Request;
using BlogAPI.Services.BlogServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BlogAPI.Controllers
{
    [Route("api/blog")]
    public class BlocController : Controller
    {
        private readonly IBlogService _blogServices;
        private readonly ILogger<BlocController> _logger;

        public BlocController(ILogger<BlocController> logger, IBlogService service)
        {
            _blogServices = service;
            _logger = logger;
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.User}")]
        public async Task<IActionResult> AddArticle([FromBody] CreateArticleModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new
                    {
                        success = false,
                        code = 0,
                        message = $"Invalid request",
                    });

                var (status, response) = await _blogServices.AddBlog(model);

                if (status == 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        code = status,
                        message = response,
                    });
                }
                else
                {
                    return Ok(new
                    {
                        success = false,
                        code = status,
                        message = "Article Created",
                        data = response,
                    });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.User}")]
        public async Task<IActionResult> DeleteArticle(int blogID)
        {
            try
            {
                var userName = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

                if (userName.IsNullOrEmpty())
                    return BadRequest(new
                    {
                        success = false,
                        code = 0,
                        message = "Unauthorized",
                    });

                var (status, response) = await _blogServices.DeleteBlog(blogID, userName!);

                return Ok(new
                {
                    success = true,
                    code = status,
                    message = response,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("articles")]
        public async Task<IActionResult> GetArticles([FromQuery] string? query)
        {
            try
            {
                var (status, response) = await _blogServices.GetArticles(query);

                return Ok(new
                {
                    success = true,
                    code = status,
                    message = "Articles Fetched",
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