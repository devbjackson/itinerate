public class Event
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TimeSpan StartTime { get; set; }
    public Guid DayId { get; set; } // Foreign Key
}