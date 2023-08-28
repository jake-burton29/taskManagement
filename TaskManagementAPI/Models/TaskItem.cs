namespace TaskManagementAPI.Models;


public class TaskItem
{
    public long Id {get; set;}
    public string? Title {get; set;}
    public string? Description {get; set;}
    public DateTime DueDate {get; set;}

    public bool isCompleted {get; set;}
}