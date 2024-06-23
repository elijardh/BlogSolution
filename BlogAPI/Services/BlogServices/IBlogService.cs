using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Models.Request;

namespace BlogAPI.Services.BlogServices
{
    public interface IBlogService
    {
        Task<(int, dynamic)> AddBlog(CreateArticleModel articleModel);
        Task<(int, dynamic)> DeleteBlog(int blogId, string userName);
        Task<(int, dynamic)> GetArticles(string? query);
    }
}