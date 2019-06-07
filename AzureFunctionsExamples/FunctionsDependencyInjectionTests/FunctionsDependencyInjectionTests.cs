using FunctionsDependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace FunctionsDependencyInjectionTests
{
    public class FunctionsDependencyInjectionTests
    {
        private readonly ILogger _logger = TestFactory.CreateLogger();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_DependencyInjectionGreeter_Returns_Expected_Greeting()
        {
            var request = TestFactory.CreateHttpRequest("", "");
            var greeter = new Greeter();
            var function = new DependencyInjectionGreeter(greeter);
            var response = (OkObjectResult)function.Run(request, _logger);
            Assert.AreEqual("Greetings from Greeter!", response.Value);
        }
    }
}