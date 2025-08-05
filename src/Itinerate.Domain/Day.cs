namespace Itinerate.Domain;

public class Day
{
    public Guid Id { get; set; }
    public int DayNumber { get; set; }
    public List<Event> Events { get; set; } = new(); // Navigation property
    public Guid ItineraryId { get; set; } // Foreign Key
}