using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;

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
            return Ok(_backgroundJobs.Enqueue(() => _serverWebService.PostAsJsonAsync($"{_serverWebService.BaseAddress}{ServerWebServiceEndpoints.HELLO_WORLD}", string.Empty, null, default)));
        }

        [HttpPost]
        [Route("SortBinaryTreeWData")]
        public IActionResult SortBinaryTreeWData(ListStrings listStrings)
        {
            return Ok(_backgroundJobs.Enqueue(() => _serverWebService.PostAsJsonAsync($"{_serverWebService.BaseAddress}{ServerWebServiceEndpoints.SORT_BINARY_TREE_W_DATA}", listStrings, null, default)));
        }

        [HttpPost]
        [Route("SortBinaryTree")]
        public IActionResult SortBinaryTree()
        {
            return Ok(_backgroundJobs.Enqueue(() => _serverWebService.PostAsJsonAsync($"{_serverWebService.BaseAddress}{ServerWebServiceEndpoints.SORT_BINARY_TREE}", TestData.ListStringToSort, null, default)));
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
