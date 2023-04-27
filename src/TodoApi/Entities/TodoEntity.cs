using System;
using Azure;
using Azure.Data.Tables;

namespace TodoApi.Entities
{
    public class TodoEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public string TaskDescription { get; set; }
        public bool IsComplete { get; set; }
    }
}