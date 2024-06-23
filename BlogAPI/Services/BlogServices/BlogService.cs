using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Models.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services.BlogServices
{
    public class BlogService : IBlogService
    {

        private readonly UserManager<UserModel> userManager;
        private readonly ApplicationDbContext applicationDbContext;

        public BlogService(ApplicationDbContext applicationDbContext, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {

            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<(int, dynamic)> AddBlog(CreateArticleModel model)
        {
            if (model.Id == null)
            {
                var articleModel = new ArticleModel
                {
                    AuthorId = model.UserId,
                    Body = model.Body,
                    Title = model.Title,
                    PublicationDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };

                await applicationDbContext.ArticleModels.AddAsync(articleModel);

                await applicationDbContext.SaveChangesAsync();

                return (1, articleModel);
            }
            else
            {
                var articleModel = applicationDbContext.ArticleModels.FirstOrDefault(article => article.Id == model.Id);

                if (articleModel == null)
                    return (0, "Invalid blog Id");

                articleModel.Body = model.Body;
                articleModel.Title = model.Title;
                articleModel.UpdateDate = DateTime.Now;

                applicationDbContext.ArticleModels.Update(articleModel);

                await applicationDbContext.SaveChangesAsync();

                return (1, articleModel);
            }
        }

        public async Task<(int, dynamic)> DeleteBlog(int articleId, string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                return (0, "Unauthorized");

            var userRoles = await userManager.GetRolesAsync(user);

            var articleModel = await applicationDbContext.ArticleModels.FirstOrDefaultAsync(article => article.Id == articleId);

            if (articleModel == null)
                return (0, "Article Not Found");

            if (user.Id != articleModel.AuthorId || userRoles.First() != UserRoles.Admin)
                return (0, "You are not authorized and not an admin");

            applicationDbContext.ArticleModels.Remove(articleModel);

            await applicationDbContext.SaveChangesAsync();

            return (1, "Article Removed");
        }



        public async Task<(int, dynamic)> GetArticles(string? query)
        {
            var articles = await applicationDbContext.ArticleModels.ToListAsync();
            if (query == null)



                return (1, articles);
            return (1, articles.Where(article => article.Title.Contains(query) || article.Body.Contains(query)));

        }
    }
}