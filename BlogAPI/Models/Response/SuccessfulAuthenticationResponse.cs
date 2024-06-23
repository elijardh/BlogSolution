namespace BlogAPI.Models.response
{
    public class SuccessfulAuthenticationResponse(UserModel userModel, String role)
    {
        public string FirstName { get; set; } = userModel.FirstName ?? "";
        public string LastName { get; set; } = userModel.LastName ?? "";
        public string Email { get; set; } = userModel.Email ?? "";
        public string UserType { get; set; } = role;
        public required string Token { get; set; }
        public string Id { get; set; } = userModel.Id ?? "0";
    }
}