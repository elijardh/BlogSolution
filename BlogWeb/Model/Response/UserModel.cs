using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogWeb.Model.Response
{
    public class UserModel
    {
        public required string firstName { get; set; }
        public required string lastName { get; set; }
        public required string email { get; set; }
        public required string userType { get; set; }
        public required string token { get; set; }
        public required string id { get; set; }

        public static UserModel? FromJson(string json)
        {
            return JsonSerializer.Deserialize<UserModel>(json);
        }
    }
}