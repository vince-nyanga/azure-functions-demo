using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TodoApi.Entities;
using Azure.Data.Tables;
using Azure;
using TodoApi.Constants;

namespace TodoApi
{
    public static class CompleteTodo
    {
        [FunctionName("CompleteTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "patch", Route = "todos/{id}/complete")] HttpRequest req,
            [Table(TableStorageConstants.TableName, TableStorageConstants.PartitionKey, "{id}", Connection = "AzureWebJobsStorage")] TodoEntity todoEntity,
            [Table(TableStorageConstants.TableName, Connection = "AzureWebJobsStorage")] TableClient todoTable,
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
