using TodoApi.Entities;
using TodoApi.Models;

namespace TodoApi.Extensions
{
    public static class TodoEntityExtensions
    {
        public static Todo ToModel(this TodoEntity todoEntity)
        {
            return new Todo(
                Id: todoEntity.RowKey,
                TaskDescription: todoEntity.TaskDescription,
                IsComplete: todoEntity.IsComplete);
        }
    }
}