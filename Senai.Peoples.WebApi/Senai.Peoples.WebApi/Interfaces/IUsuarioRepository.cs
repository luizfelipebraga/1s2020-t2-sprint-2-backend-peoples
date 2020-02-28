using senai.Peoples.WebApi.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace senai.Peoples.WebApi.Interfaces
{
    interface IUsuarioRepository
    {
        List<UsuarioDomain> Listar();

        void Cadastrar(UsuarioDomain novoUsuario);

        void Atualizar(int id, UsuarioDomain usuarioAtualizado);

        UsuarioDomain BuscarPorId(int id);

        void Deletar(int id);

        UsuarioDomain BuscarPorEmailSenha(string email, string senha);

    }
}
