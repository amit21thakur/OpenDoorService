using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrawShapesService.Controllers
{
    [Route("api/")]
    public class OpenDoorController : Controller
    {
        private readonly ILogger _logger;
        private readonly IComponentContext _container;

        private Dictionary<int, int> _fibCache = new Dictionary<int, int>();

        public OpenDoorController(IComponentContext container, ILogger<OpenDoorController> logger)
        {
            _container = container;
            _logger = logger;
        }

        [HttpGet("Token")]
        public async Task<IActionResult> GetToken()
        {
            return Ok("03792a61-9104-4b4d-87d2-e975f475212d");

        }

        [HttpGet("TriangleType")]
        public async Task<IActionResult> GetTriangleType(int a, int b, int c)
        {
            try
            {
                if (a + b <= c || b + c <= a || c + a <= b)             //Check whether its a valid possible triangle. Check whether the sum of any two sides is greater then the third side
                    return Ok("Error");

                bool isSideAbSame = a == b;                             //Compare sides once and later on no need to compare the same side again.
                bool isSideBcSame = b == c;
                if (isSideAbSame && isSideBcSame)                       //Atleaset two comparions are required to figure out whether its a equilateral traingle.
                {
                    return Ok("Equilateral");
                }
                if (isSideAbSame || isSideBcSame || c == a)                       //Nearly two-third Isosceles scenarios whill be handled here without comparing the Side C and Side A.
                {
                    return Ok("Isosceles");
                }

                return Ok("Scalene");
            }
            catch (Exception)                                        //We can get exceptions due to external factors like OOM etc.
            {
                return Ok("Error");
            }

        }


        [HttpGet("ReverseWords")]
        public async Task<IActionResult> ReverseWords(string sentence)
        {
            var stack = new Stack<char>();

            char[] input = sentence.ToCharArray();
            char[] result = new char[input.Length];
            int j = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == ' ')
                {
                    while (stack.Count > 0)
                    {
                        result[j++] = stack.Pop();
                    }
                    result[j++] = input[i];
                }
                else
                {
                    stack.Push(input[i]);
                }
            }
            while (stack.Count > 0)
            {
                result[j++] = stack.Pop();
            }

            return Ok(new string(result));
        }

        [HttpGet("Fibonacci")]
        public async Task<IActionResult> Fibonacci(int n)
        {
            if (n < 0 || n>92)
                return StatusCode(400);

            return Ok(Fib(n, new Dictionary<int, long>()));
        }
        private long Fib(int n, Dictionary<int, long> cache)
        {
            if (n == 0 || n == 1)
                return n;

            if (cache.ContainsKey(n))
                return cache[n];

            long result = Fib(n - 1, cache) + Fib(n - 2, cache);
            cache.Add(n, result);
            return result;
        }

    }

    //    public class Stack
}
