using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlogWeb.Model.Request;
using BlogWeb.Model.Response;
using Microsoft.AspNetCore.Components;

namespace BlogWeb.Services
{
    public class AuthenticationService(HttpClient httpClient, DialogService dialogService, UserService userService, NavigationManager NavigationManager)
    {
        public bool loading = false;
        private readonly HttpClient httpClient = httpClient;
        private readonly DialogService dialogService = dialogService;
        private readonly UserService userService = userService;

        private readonly NavigationManager navigationManager = NavigationManager;

        public async Task SignIn(LoginModel loginModel)
        {
            try
            {
                loading = true;

                var response = await httpClient.PostAsJsonAsync("api/user/login", loginModel);

                var stringifiedRes = await response.Content.ReadAsStringAsync();

                var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, dynamic>>(stringifiedRes);

                if (dict == null)
                {
                    throw new Exception("Wahala");
                }

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(dict["message"]);
                }

                Console.Write(dict["data"].ToString());

                userService.setUserModel(UserModel.FromJson(dict["data"].ToString()));
                navigationManager.NavigateTo("/");

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

        public async Task SignUp(RegisterModel registerModel)
        {
            try
            {
                loading = true;

                var response = await httpClient.PostAsJsonAsync("api/user/registration", registerModel);

                var stringifiedRes = await response.Content.ReadAsStringAsync();

                Console.WriteLine(stringifiedRes);

                var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, dynamic>>(stringifiedRes);

                if (dict == null)
                {
                    throw new Exception("Wahala");
                }

                Console.WriteLine(dict["message"] + response.StatusCode.ToString());

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(dict["message"].ToString());
                }

                userService.setUserModel(UserModel.FromJson(dict["data"]));

                navigationManager.NavigateTo("/");

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