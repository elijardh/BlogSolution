using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Models.Request
{
    public class CreateCommentModel
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Article Id is required")]
        public required int ArticleId { get; set; }
        [Required(ErrorMessage = "Body is required")]
        public required string Body { get; set; }
        [Required(ErrorMessage = "User Id is required")]
        public required string UserId { get; set; }
    }
}