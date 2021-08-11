using ConsumeRestApiInMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsumeRestApiInMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userIds = new int[] { 1, 3, 10 };
            var users = GetUsers(userIds).Result;
            return View(users);
        }

        static async Task<List<User>> GetUsers(int[] userIds)
        {
            var users = new List<User>();
            string baseUri = "https://www.yourapiurlgoeshere.com/api/";

            using HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(baseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            foreach (var userId in userIds)
            {
                HttpResponseMessage response = await client.GetAsync($"users/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var person = response.Content.ReadAsStringAsync().Result;
                    User user = JsonConvert.DeserializeObject<User>(person);
                    users.Add(user);
                }
            }
            return users;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
