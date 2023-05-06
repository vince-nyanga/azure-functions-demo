using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TodoApi.Entities;
using TodoApi.Requests;
using System;
using TodoApi.Constants;

namespace TodoApi
{
    public static class CreateTodo
    {
        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "todos")] HttpRequest req,
            [Table(TableStorageConstants.TableName, Connection = "AzureWebJobsStorage")] IAsyncCollector<TodoEntity> todosCollector,
            ILogger log)
        {
            log.LogInformation("Creating a new todo.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            TodoEntity todoEntity = CreateTodoEntity(requestBody);

            await todosCollector.AddAsync(todoEntity);

            log.LogInformation("Todo created successfully.");

            return new OkObjectResult(todoEntity.RowKey);
        }

        private static TodoEntity CreateTodoEntity(string requestBody)
        {
            var request = JsonConvert.DeserializeObject<CreateTodoRequest>(requestBody);

            return new TodoEntity
            {
                TaskDescription = request.Task,
                IsComplete = false,
                PartitionKey = TableStorageConstants.PartitionKey,
                RowKey = Guid.NewGuid().ToString()
            };
        }
    }
}
