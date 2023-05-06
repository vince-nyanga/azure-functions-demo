using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TodoApi.Entities;
using TodoApi.Extensions;
using TodoApi.Constants;

namespace TodoApi
{
    public static class GetTodo
    {
        [FunctionName("GetTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todos/{id}")] HttpRequest req,
            [Table(TableStorageConstants.TableName, TableStorageConstants.PartitionKey, "{id}", Connection = "AzureWebJobsStorage")] TodoEntity todoEntity,
            ILogger log)
        {
            log.LogInformation("Getting todo by id.");

            if (todoEntity == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(todoEntity.ToModel());
        }
    }
}
