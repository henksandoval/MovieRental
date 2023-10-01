namespace MovieRentalApi.Data.Entities;

public class RentalEntity
{
	public int Id { get; set; }
	public DateTime RentalDate { get; set; }

	public virtual MovieEntity Movie { get; set; }
}