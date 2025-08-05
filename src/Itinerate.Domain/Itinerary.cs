public class Itinerary
{
    public Guid Id { get; set; } // Using Guid for unique, non-sequential IDs
    public string Name { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Day> Days { get; set; } = new(); // Navigation property
}