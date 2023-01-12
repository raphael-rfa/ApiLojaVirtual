using Data;
using Data.Models.Entidade;
using Repository.Usuarios;
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
        private readonly IUsuariosRepositorio _repository;
        private readonly TokenService _tokenService;

        public UsuarioController(ApiLojaVirtualContext context, IConfiguration configuration)
        {
            _repository = new UsuarioRepositorio(context);
            _tokenService = new TokenService(context, configuration);
        }

        [HttpPost]
        public IActionResult CriarUsuario(Usuario usuario)
        {            

            if(_repository.Existe(usuario.Login!))
            {
                _repository.CriarUsuario(usuario);

                return Ok("Usuario Criado verifique a caixa de entrada do seu email: " + usuario.Email);
            }
            return Problem("Usuario já existe");
        }

        [HttpPatch]
        public IActionResult ConfirmarUsuario(string email, string chaveVerificacao)
        {
            return _repository.VerificarEmail(email, chaveVerificacao) ?
                Ok("Email Verificado") : Problem("Usuario não foi verificado");
        }

        [HttpPatch]
        public IActionResult AutenticarUsuario(Usuario user)
        {
            user.LastToken = _tokenService.Token(user);

            _repository.SalvarUsuario(user);

            return Ok(user.LastToken);
        }
    }
}
