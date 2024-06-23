using System.ComponentModel.DataAnnotations;
namespace BlogAPI.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Username is required")]
		public String? UserName { get; set; }
		[Required(ErrorMessage = "Password is required")]
		public String? Password { get; set; }
	}
}

