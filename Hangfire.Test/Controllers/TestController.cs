using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;

namespace Hangfire.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private IBackgroundJobClient _backgroundJobs { get; set; }

        public TestController(IBackgroundJobClient backgroundJobs)
        {
            _backgroundJobs = backgroundJobs;
        }

        [HttpPost]
        [Route("HelloWorld")]
        public IActionResult HelloWorld()
        {
            Log.Information("Queueing HelloWorld");

            return Ok(_backgroundJobs.Enqueue(() => HelloWorldQueue()));
        }

        [NonAction]
        public void HelloWorldQueue()
        {
            Log.Information("Executing HelloWorld");

            Console.WriteLine("Hello world from Hangfire!");
        }

        [HttpPost]
        [Route("SortBinaryTreeWData")]
        public IActionResult SortBinaryTreeWData(ListStrings listStrings)
        {
            Log.Information("Queueing SortBinaryTreeWData");

            return Ok(_backgroundJobs.Enqueue(() => SortBinaryTreeWDataQueue(listStrings)));
        }

        [NonAction]
        public IEnumerable<string> SortBinaryTreeWDataQueue(ListStrings listStrings)
        {
            Log.Information("Executing SortBinaryTreeWData with {ListString}", listStrings);

            return Testes.SortBinaryTree(listStrings.List);
        }

        [HttpPost]
        [Route("SortBinaryTree")]
        public IActionResult SortBinaryTree()
        {
            Log.Information("Queueing SortBinaryTree");

            return Ok(_backgroundJobs.Enqueue(() => SortBinaryTreeQueue()));
        }

        [NonAction]
        public IEnumerable<string> SortBinaryTreeQueue()
        {
            Log.Information("Executing SortBinaryTree with pregenerated data");

            return Testes.SortBinaryTree();
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
            Log.Information("Sorting binary tree with values {ListStrings}", TestData.ListStringToSort);

            return new SortedSet<string>(TestData.ListStringToSort);
        }
    }
}
