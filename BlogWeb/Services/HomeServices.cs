using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BlogWeb.Model.Response;

namespace BlogWeb.Services
{
    public class HomeServices(HttpClient httpClient, DialogService dialogService)
    {
        public bool loading = false;
        private readonly HttpClient httpClient = httpClient;
        private readonly DialogService dialogService = dialogService;
        public List<ArticleModel> articles = [];

        public async Task GetArticles()
        {
            try
            {
                loading = true;

                List<ArticleModel> rawArticles = [];

                Debug.WriteLine(httpClient.BaseAddress + "api/blog/article");
                Console.Write(httpClient.BaseAddress + "api/blog/article");

                var response = await httpClient.GetAsync("api/blog/articles");

                var stringifiedRes = await response.Content.ReadAsStringAsync();

                var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, dynamic>>(stringifiedRes) ?? throw new Exception("Wahala");


                rawArticles = JsonSerializer.Deserialize<List<ArticleModel>>(dict["data"]);
                Console.Write(rawArticles.Count);

                articles = rawArticles;

            }
            catch (Exception ex)
            {
                dialogService.Show("Something went wrong", ex.Message);

                Console.WriteLine(ex.Message);
            }
            finally
            {
                loading = false;
            }
        }
    }
}