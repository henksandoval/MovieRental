namespace MovieRentalApi.Utilities;

public class SystemClock : IClock
{
	public DateTime GetCurrentTime()
	{
		return DateTime.Now;
	}
}