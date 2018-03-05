using System;

namespace RuhRoh.Samples.WebAPI.Domain
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }
    }
}