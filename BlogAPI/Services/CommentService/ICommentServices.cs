using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAPI.Models.Request;

namespace BlogAPI.Services.CommentService
{
    public interface ICommentServices
    {
        Task<(int, dynamic)> AddComment(CreateCommentModel model);
        Task<(int, dynamic)> DeleteComment(int commentId, string userName);
        Task<(int, dynamic)> GetComments(int articleId);
    }
}