using Data;
using Data.Models.Entidade;
using Microsoft.IdentityModel.Tokens;
using Repository.Usuarios;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiLojaVirtual.Controllers
{
    public class TokenService
    {
        private readonly IUsuariosRepositorio _repository = null!;
        private readonly IConfiguration _configuration = null!;

        public TokenService(ApiLojaVirtualContext context, IConfiguration configuration)
        {
            _repository = new UsuarioRepositorio(context);
            _configuration = configuration;
        }
        public string Token(Usuario user)
        {
            if (_repository.VerificarUsuario(user))
            {
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes
                (_configuration["Jwt:Key"]!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                            new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, user.Nome),
                            new Claim(JwtRegisteredClaimNames.Email, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString())
                        }),
                    Expires = DateTime.UtcNow.AddMinutes(10),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                var stringToken = tokenHandler.WriteToken(token);

                return stringToken;
            }
            return "Falha na autorização";
        }
    }
}
