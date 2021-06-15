using Microsoft.AspNetCore.Mvc;
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
            return Ok(_backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!")));
        }

        [HttpPost]
        [Route("SortBinaryTreeWData")]
        public IActionResult SortBinaryTreeWData(ListStrings listStrings)
        {
            return Ok(_backgroundJobs.Enqueue(() => Testes.SortBinaryTree(listStrings.List)));
        }

        [HttpPost]
        [Route("SortBinaryTree")]
        public IActionResult SortBinaryTree()
        {
            return Ok(_backgroundJobs.Enqueue(() => Testes.SortBinaryTree()));
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
