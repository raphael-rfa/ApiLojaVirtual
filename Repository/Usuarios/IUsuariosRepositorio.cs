using Data.Models.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Usuarios
{
    public interface IUsuariosRepositorio
    {
        bool Existe(string loginUsuario);
        bool VerificarEmail(string email, string chaveVerificacao);
        void CriarUsuario(Usuario usuario);
        void SalvarUsuario(Usuario usuario);
        void EnviarEmail(Usuario usuario);
        bool VerificarUsuario(Usuario user);

    }
}
