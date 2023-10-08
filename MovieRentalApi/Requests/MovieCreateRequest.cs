using System.ComponentModel.DataAnnotations;

namespace MovieRentalApi.Requests;

public class MovieCreateRequest
{
	//[Required(ErrorMessage = "El campo {0} es obligatorio.")]
	//[StringLength(maximumLength: 20, MinimumLength = 2, ErrorMessage = "El campo {0} debe contener entre {2} y {1} carácteres de longitud.")]
	[Required] public string Title { get; set; }

	public string Description { get; set; }
	public int Year { get; set; }
}