using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class RandomGen
    {
        private static readonly RNGCryptoServiceProvider _globol = new RNGCryptoServiceProvider();
        [ThreadStatic]
        private static Random _local;

        public static double NextDouble()
        {
            Random inst = _local;
            if (inst == null)
            {
                byte[] buffer = new byte[4];
                _globol.GetBytes(buffer);
                _local = inst = new Random(BitConverter.ToInt32(buffer, 0));
            }
            return inst.NextDouble();
        }
    }

    [ApiController]
    public class GreetingsController : ControllerBase
    {
        [Route("api/greetings")]
        [HttpGet("{name}")]
        public ActionResult<string> GetGreeting(string name)
        {
            return $"Hello, {name}";
        }

        [Route("api/cards")]
        [HttpPost]
        public async Task<ActionResult> ProcessCard([FromBody] string card)
        {
            var randomValue = RandomGen.NextDouble();
            var approved = randomValue > 0.1;
            await Task.Delay(1000);
            Console.WriteLine($"Card {card} processed");
            return Ok(new { Card = card, Approved = approved });
        }
    }
}