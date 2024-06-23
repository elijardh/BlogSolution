using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI.Models.DatabaseSchema
{
    public class CommentModel
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required int ArticleId { get; set; }
        public required string Body { get; set; }
        public required DateTime DateCreated { get; set; }

    }
}