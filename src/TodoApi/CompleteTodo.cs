using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TodoApi.Entities;
using Azure.Data.Tables;
using Azure;

namespace TodoApi
{
    public static class CompleteTodo
    {
        [FunctionName("CompleteTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "todos/{id}/complete")] HttpRequest req,
            [Table(Constants.TableName, Constants.PartitionKey, "{id}", Connection = "AzureWebJobsStorage")] TodoEntity todoEntity,
            [Table(Constants.TableName, Connection = "AzureWebJobsStorage")] TableClient todoTable,
            ILogger log)
        {
            log.LogInformation("Completing todo.");

            if (todoEntity == null)
            {
                return new NotFoundResult();
            }

            todoEntity.IsComplete = true;

            await todoTable.UpdateEntityAsync(todoEntity, ETag.All);

            log.LogInformation("Todo completed successfully.");

            return new OkResult();
        }
    }
}
