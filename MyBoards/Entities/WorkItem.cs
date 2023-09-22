using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBoards.Entities
{
    public class Epic : WorkItem
    {
        public DateTime? StartDate { get; set; }
        //[Precision(3)]
        public DateTime? EndDate { get; set; }
    }
    public class Issue : WorkItem
    {
        //[Column(TypeName = "decimal(5,2)")]
        public decimal Efford { get; set; }
    }
    public class Task : WorkItem
    {
        //[MaxLength(200)]
        public string Activity { get; set; }
        //[Precision(4,2)]
        public decimal RemaningWork { get; set; }
    }
    public abstract class WorkItem
    {
        public int Id { get; set; }
        public WorkItemState State { get; set; }
        public int StateId { get; set; }
        //[Column(TypeName = "varchar(200)")]
        public string Area { get; set; }
        //[Column("Iteration_Path")]
        public string IterationPath { get; set; }
        public int Priority { get; set; }


        public List<Comment> Comments { get; set; } = new List<Comment>(); 
        public User User { get; set; }
        public Guid UserId { get; set; }
        public List<Tag> Tags { get; set; }

    }
}
