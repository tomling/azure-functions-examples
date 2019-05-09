using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace DurableFunctionChaining
{
    public static class SentenceBuilderDurableFunction
    {
        [FunctionName("SentenceBuilder_Orchestrator_Function")]
        public static async Task<string> RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context)
        {
            var sb = new StringBuilder();

            sb = await context.CallActivityAsync<StringBuilder>("Function_Activity_Fox", sb);
            sb = await context.CallActivityAsync<StringBuilder>("Function_Activity_Jumps", sb);
            sb = await context.CallActivityAsync<StringBuilder>("Function_Activity_Dog", sb);

            return sb.ToString();
        }

        [FunctionName("Function_Activity_Fox")]
        public static StringBuilder AddQuickBrownFox([ActivityTrigger] StringBuilder sb, ILogger log)
        {
            //log.LogInformation($"Saying hello to {name}.");
            sb.Append("The quick brown fox");
            return sb;
        }

        [FunctionName("Function_Activity_Jumps")]
        public static StringBuilder AddJumps([ActivityTrigger] StringBuilder sb, ILogger log)
        {
            sb.Append(" jumps over ");
            return sb;
        }

        [FunctionName("Function_Activity_Dog")]
        public static StringBuilder AddDog([ActivityTrigger] StringBuilder sb, ILogger log)
        {
            sb.Append("the lazy dog.");
            return sb;
        }

        [FunctionName("SentenceBuilder_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("SentenceBuilder_Orchestrator_Function", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}