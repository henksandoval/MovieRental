namespace MovieRentalApi.Data.Entities;

public class MovieEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
    public virtual CategoryEntity Category { get; set; }
}