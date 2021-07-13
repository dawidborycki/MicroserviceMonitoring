using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleWebApp.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace PeopleWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient _httpClient;
        private static int index = 0;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient("MockHttpClient");
        }

        public IActionResult Index()
        {
            _logger.LogInformation("[HomeController] [Index]");

            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("[HomeController] [Privacy]");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogInformation("[HomeController] [Error]");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> CallMockService()
        {
            _logger.LogInformation("[HomeController] [CallMockService]");

            var response = await _httpClient.GetAsync("/mock");

            _logger.LogInformation($"[HomeController] [CallMockService: {response.StatusCode}]");

            return View(response);
        }

        public IActionResult ExceptionTrigger()
        {
            string[] messages = {"I am having some issue", "Important one", "Severe issue" };

            switch(index++ % 3)
            {
                case 0:
                default:                    
                    _logger.LogError(messages[0]);
                    throw new Exception(messages[0]);

                case 1:                
                    _logger.LogError(messages[1]);
                    throw new Exception(messages[1]);

                case 2:
                    _logger.LogError(messages[2]);
                    throw new Exception(messages[2]);
            }
            
        }
    }
}
