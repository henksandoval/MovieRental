namespace MovieRentalApi.Utilities;

public class SystemClock : IClock
{
	public DateTime GetCurrentTime() => DateTime.Now;
}