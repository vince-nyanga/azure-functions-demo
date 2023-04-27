namespace TodoApi.Models
{
    public class Todo
    {
        public string Id { get; set; }
        public string TaskDescription { get; set; }
        public bool IsComplete { get; set; }
    }
}