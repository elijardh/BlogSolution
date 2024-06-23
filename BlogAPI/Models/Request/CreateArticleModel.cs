using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Models.Request
{
    public class CreateArticleModel
    {
        [Required(ErrorMessage = "User ID is required")]
        public required string UserId { get; set; }
        public int? Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public required string Title { get; set; }
        [Required(ErrorMessage = "Body is required")]
        public required string Body { get; set; }
    }
}