using System.Security.Claims;
using BlogAPI.Models;
using BlogAPI.Models.Request;
using BlogAPI.Services.CommentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BlogAPI.Controllers
{
    [Route("comments")]
    public class CommentController : Controller
    {
        private readonly ICommentServices _commentService;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ILogger<CommentController> logger, ICommentServices commentServices)
        {
            _commentService = commentServices;
            _logger = logger;
        }

        [HttpPost]
        [Route("add")]
        [Authorize(Roles = $"{UserRoles.Admin}, {UserRoles.User}")]
        public async Task<IActionResult> AddComment([FromBody] CreateCommentModel model)
        {
            try
            {
                var (status, response) = await _commentService.AddComment(model);
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
                    message = "Comment Created",
                    data = response,
                });
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
        public async Task<IActionResult> DeleteComment(int commentId)
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

                var (status, response) = await _commentService.DeleteComment(commentId, userName!);

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
        [Route("fetch")]
        public async Task<IActionResult> GetComments(int articleId)
        {
            try
            {
                var (status, response) = await _commentService.GetComments(articleId);

                return Ok(new
                {
                    success = true,
                    code = status,
                    message = "Comments fetched",
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