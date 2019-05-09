using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace FunctionsDependencyInjection
{
    public class DependencyInjectionGreeter
    {
        private readonly IGreeter _greeter;

        public DependencyInjectionGreeter(IGreeter greeter)
        {
            _greeter = greeter;
        }

        [FunctionName("DependencyInjectionGreeter")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var greeting = _greeter.Greet();
            log.LogInformation($"Got Greeting: {greeting}");

            return new OkObjectResult(greeting);
        }
    }

}
