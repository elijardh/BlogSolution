using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Models.DatabaseSchema;
using BlogAPI.Models.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services.CommentService
{
    public class CommentServices : ICommentServices
    {
        private readonly UserManager<UserModel> userManager;
        private readonly ApplicationDbContext applicationDbContext;

        public CommentServices(UserManager<UserModel> userManager, ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            this.userManager = userManager;
        }
        public async Task<(int, dynamic)> AddComment(CreateCommentModel model)
        {
            if (model.Id == null)
            {
                CommentModel commentModel = new CommentModel
                {
                    ArticleId = model.ArticleId,
                    Body = model.Body,
                    DateCreated = DateTime.Now,
                    UserId = model.UserId,
                };

                await applicationDbContext.CommentModels.AddAsync(commentModel);

                await applicationDbContext.SaveChangesAsync();

                return (1, commentModel);
            }
            else
            {
                CommentModel? commentModel = await applicationDbContext.CommentModels.FirstOrDefaultAsync(comment => comment.Id == model.Id);

                if (commentModel == null)
                    return (0, "Comment not found");

                commentModel.Body = model.Body;

                applicationDbContext.CommentModels.Update(commentModel);

                await applicationDbContext.SaveChangesAsync();

                return (1, commentModel);
            }


        }

        public async Task<(int, dynamic)> DeleteComment(int commentId, string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                return (0, "Unauthorized");

            var userRoles = await userManager.GetRolesAsync(user);

            var commentModel = await applicationDbContext.CommentModels.FirstOrDefaultAsync(comment => comment.Id == commentId);

            if (commentModel == null)
                return (0, "Comment Not Found");

            if (user.Id != commentModel.UserId || userRoles.First() != UserRoles.Admin)
                return (0, "You are not authorized and not an admin");

            applicationDbContext.CommentModels.Remove(commentModel);

            await applicationDbContext.SaveChangesAsync();

            return (1, "Comment Removed");
        }

        public async Task<(int, dynamic)> GetComments(int articleId)
        {
            var comments = await applicationDbContext.CommentModels.Where(comments => comments.ArticleId == articleId).ToListAsync();

            return (1, comments);
        }
    }
}