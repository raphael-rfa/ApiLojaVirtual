using ApiLojaVirtual.Data.Context;
using ApiLojaVirtual.Data.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiLojaVirtual.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/[Action]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ApiLojaVirtualContext _context;
        private readonly IConfiguration _configuration;

        public UsuarioController(ApiLojaVirtualContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> CriarUsuario(Usuario usuario)
        {
            var exist = _context.Usuario.Where(x => x.Login == usuario.Login);

            if(exist.IsNullOrEmpty())
            {
                usuario.Ativo = true;
                usuario.Excluido = false;
                usuario.ChaveVerificacao = Guid.NewGuid().ToString();

                _context.Usuario.Add(usuario);
                _context.SaveChanges();
                
                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("raphaelbatealves@gmail.com", "Api Loja Virtual");
                var subject = "Chave de verificação de usuario";
                var to = new EmailAddress(usuario.Email, usuario.Nome);
                var plainTextContent = "Copie o codigo(" + usuario.ChaveVerificacao + ") e volte para a API para fazer a confirmação do usuario";
                var htmlContent = "<span>Copie o codigo( </span><em>" + usuario.ChaveVerificacao + "</em><span> ) e volte para a API para fazer a confirmação do usuario</span>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                await client.SendEmailAsync(msg);

                return Ok("Usuario Criado verifique a caixa de entrada do seu email: " + usuario.Email);
            }
            return Problem("Usuario já existe");
        }

        [HttpPatch]
        public IActionResult ConfirmarUsuario(string email, string chaveVerificacao)
        {
            var exist = _context.Usuario
                .Where(x => x.Email == email && x.ChaveVerificacao == chaveVerificacao)
                .FirstOrDefault();

            if (exist != null)
            {
                exist.IsVerificado = true;
                _context.SaveChanges();
                return Ok("Usuario verificado parabéns");
            }
            return Problem("Não foi possivel verificar o usuario, email ou chave invalidos!");
        }

        [HttpPatch]
        public IActionResult AutenticarUsuario(Usuario user)
        {
            user.LastToken = Token(user);

            string Token(Usuario user)
            {
                Usuario? autenticado = _context.Usuario.Find(user.Id);

                if (user.Login == autenticado!.Login && user.Senha == autenticado.Senha)
                {
                    var issuer = _configuration["Jwt:Issuer"];
                    var audience = _configuration["Jwt:Audience"];
                    var key = Encoding.ASCII.GetBytes
                    (_configuration["Jwt:Key"]);
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
            _context.SaveChanges();
            return Ok(user.LastToken);
        }
    }
}
