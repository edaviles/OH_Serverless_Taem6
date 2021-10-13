using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OH.Team6
{
    public static class ohchallenge2
    {
        [FunctionName("ohchallenge2")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string product = req.Query["productId"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            product = product ?? data?.productId;

            string responseMessage = string.IsNullOrEmpty(product)
                ? "Error, need to pass productId"
                : $"The product name for your product id {product} is Starfruit Explosion";

            return new OkObjectResult(responseMessage);
        }
    }
}
