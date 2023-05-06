using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using TodoApi.Entities;
using System.Collections.Generic;
using TodoApi.Extensions;
using TodoApi.Constants;

namespace TodoApi
{
    public static class GetTodos
    {
        [FunctionName("GetTodos")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "todos")] HttpRequest req,
            [Table(TableStorageConstants.TableName, Connection = "AzureWebJobsStorage")] TableClient tableClient,
            ILogger log)
        {
            log.LogInformation("Getting all todos.");

            var todoEntities = new List<TodoEntity>();

            var results = tableClient.Query<TodoEntity>(x =>
                x.PartitionKey == TableStorageConstants.PartitionKey);

            todoEntities.AddRange(results);

            return new OkObjectResult(todoEntities.ConvertAll(x => x.ToModel()));
        }
    }
}
