using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Hangfire.Client.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private IBackgroundJobClient _backgroundJobs { get; set; }
        private HttpClient _serverWebService { get; set; }

        public TestController(IBackgroundJobClient backgroundJobs, IHttpClientFactory factory)
        {
            _backgroundJobs = backgroundJobs;
            _serverWebService = factory.CreateClient(WebServices.SERVER_WEB_SERVICE);
        }

        [HttpPost]
        [Route("HelloWorld")]
        public IActionResult HelloWorld()
        {
            Log.Information("Queueing HelloWorld on Client");

            return Ok(_backgroundJobs.Enqueue(() => QueuePostAsync(ServerWebServiceEndpoints.HELLO_WORLD, null)));
        }

        [HttpPost]
        [Route("SortBinaryTreeWData")]
        public IActionResult SortBinaryTreeWData(ListStrings listStrings)
        {
            Log.Information("Queueing SortBinaryTreeWData on Client");

            return Ok(_backgroundJobs.Enqueue(() => QueuePostAsync(ServerWebServiceEndpoints.SORT_BINARY_TREE_W_DATA, listStrings)));
        }

        [HttpPost]
        [Route("SortBinaryTree")]
        public IActionResult SortBinaryTree()
        {
            Log.Information("Queueing SortBinaryTree on Client");

            return Ok(_backgroundJobs.Enqueue(() => QueuePostAsync(ServerWebServiceEndpoints.SORT_BINARY_TREE, TestData.ListStringToSort)));
        }

        [NonAction]
        public async Task<HttpResponseMessage> QueuePostAsync(string endpoint, object jsonPayload)
        {
            Log.Information("Executing QueuePostAsync in {Endpoint} and {Payload}", endpoint, jsonPayload);

            using var response = await _serverWebService.PostAsJsonAsync($"{_serverWebService.BaseAddress}{endpoint}", jsonPayload, null, default);
            response.EnsureSuccessStatusCode();

            return response;
        }
    }

    public class ListStrings
    {
        public IList<string> List { get; set; }
    }

    public static class Testes
    {
        public static IEnumerable<string> SortBinaryTree(IList<string> list)
        {
            return new SortedSet<string>(list);
        }

        public static IEnumerable<string> SortBinaryTree()
        {
            return new SortedSet<string>(TestData.ListStringToSort);
        }
    }
}
