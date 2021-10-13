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
    public static class CreateRating
    {
        [FunctionName("CreateRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(databaseName: "products", collectionName: "product-ratings", ConnectionStringSetting = "CosmosDbConnectionString")]IAsyncCollector<dynamic> documentsOut,
            ILogger log)
        {
            log.LogInformation("Function CreateRatings is running");

            string userid = null;
            string productid = null;
            string id_random = null;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            userid = userid ?? data?.userId;
            productid = productid ?? data?.productId;
            id_random = System.Guid.NewGuid().ToString(); 

            if (!string.IsNullOrEmpty(productid))
            {
               await documentsOut.AddAsync(new
               {
                    id = id_random, // create a random ID
                    userId = userid,
                    productId = productid,
                    locationName = data?.locationName,
                    rating = data?.rating,
                    userNotes = data?.userNotes,
                    timeStamp = DateTime.Now.ToString()

               });
            } 

            string responseMessage = string.IsNullOrEmpty(productid)
                ? "Error, need to pass productId"
                : $"The product id, {productid} was processed with ID {id_random}";

            return new OkObjectResult(responseMessage);
        }
    }
}
