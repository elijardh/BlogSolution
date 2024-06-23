using System.Text.Json;

namespace BlogWeb.Model.Response
{
    public class ArticleModel
    {

        public string? authorId { get; set; }
        public int? Id { get; set; }

        public required string title { get; set; }

        public required string body { get; set; }

        public static ArticleModel? FromJson(JsonElement json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<ArticleModel>(json);
        }
    }
}