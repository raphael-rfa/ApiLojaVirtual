using Data;
using Data.Models.Entidade;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Repository.Usuarios
{
    public class UsuarioRepositorio : IUsuariosRepositorio
    {
        private readonly ApiLojaVirtualContext _context;

        public UsuarioRepositorio(ApiLojaVirtualContext context)
        {
            _context = context;
        }

        public bool Existe(string loginUsuario)
        {
            var exist = _context.Usuario.Where(x => x.Login == loginUsuario);

            return exist != null;
        }

        public void SalvarUsuario(Usuario usuario)
        {
            var user = _context.Usuario.Find(usuario.Id);

            if(user != null)
            {
                user.LastToken = usuario.LastToken;
                _context.Usuario.Update(user);
            }
            else
            {
                _context.Usuario.Add(usuario);
            }

            _context.SaveChanges();
        }

        public void CriarUsuario(Usuario usuario)
        {
            usuario.Ativo = true;
            usuario.Excluido = false;
            usuario.ChaveVerificacao = Guid.NewGuid().ToString();

            SalvarUsuario(usuario);

            EnviarEmail(usuario);
        }

        public void EnviarEmail(Usuario usuario)
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("raphaelbatealves@gmail.com", "Api Loja Virtual");
            var subject = "Chave de verificação de usuario";
            var to = new EmailAddress(usuario.Email, usuario.Nome);
            var plainTextContent = "Copie o codigo(" + usuario.ChaveVerificacao + ") e volte para a API para fazer a confirmação do usuario";
            var htmlContent = "<span>Copie o codigo( </span><em>" + usuario.ChaveVerificacao + "</em><span> ) e volte para a API para fazer a confirmação do usuario</span>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            client.SendEmailAsync(msg);
        }

        public bool VerificarEmail(string email, string chaveVerificacao)
        {
            var exist = _context.Usuario
                .Where(x => x.Email == email && x.ChaveVerificacao == chaveVerificacao)
                .Single();

            if (exist != null)
            {
                exist.IsVerificado = true;
                _context.SaveChanges();
                return true;
            }

            return false;
        }
        public bool VerificarUsuario(Usuario user)
        {
            Usuario autenticado = _context.Usuario.Find(user.Id)!;
            return user.Login == autenticado!.Login && user.Senha == autenticado.Senha;
        }

    }
}
