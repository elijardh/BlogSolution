using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogAPI.Models;
using BlogAPI.Models.response;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace BlogAPI.Services
{
    public class AuthService : IAuthService
    {

        private readonly UserManager<UserModel> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IConfiguration configuration;

        public AuthService(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task<(int, dynamic)> Login(LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName!);
            if (user == null)
                return (0, "User not found");

            if (!await userManager.CheckPasswordAsync(user, model.Password!))
                return (0, "Incorrect Password");

            var userRoles = await userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var item in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            string token = GenerateToken(authClaims);

            SuccessfulAuthenticationResponse response = new(user, role: userRoles.First())
            {
                Token = token
            };

            return (1, response);
        }

        private string GenerateToken(List<Claim> authClaims)
        {
            var AuthSignInkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTKey:Secret"]!));

            var TokenExpiryTimeInHours = Convert.ToInt64(configuration["JWTKey:TokenExpiryTimeInHour"]);

            var TokenDescriptor = new SecurityTokenDescriptor()
            {
                Issuer = configuration["JWTKey:ValidIssuer"],
                Audience = configuration["JWTKey:ValidAudience"],
                Expires = DateTime.UtcNow.AddMinutes(40),
                SigningCredentials = new SigningCredentials(AuthSignInkey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(authClaims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(TokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<(int, dynamic)> Registration(RegistrationModel model, string role)
        {
            var userExist = await userManager.FindByNameAsync(model.UserName!);
            Console.WriteLine("Works");
            if (userExist != null)
                return (0, "User already exist");

            UserModel applicationUser = new()
            {
                UserName = model.UserName,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var createUserResult = await userManager.CreateAsync(applicationUser, model.Password!);

            Console.WriteLine(createUserResult.Errors.ToString());
            if (!createUserResult.Succeeded)
                return (0, createUserResult.Errors.First().Description);

            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

            if (await roleManager.RoleExistsAsync(role))
                await userManager.AddToRoleAsync(applicationUser, role);

            var user = await userManager.FindByNameAsync(model.UserName!);

            if (user == null)
                return (0, "User not found");

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var item in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, item));
            }

            string token = GenerateToken(authClaims);

            SuccessfulAuthenticationResponse response = new(user, role: userRoles.First())
            {
                Token = token
            };

            return (1, response);

        }
    }
}

