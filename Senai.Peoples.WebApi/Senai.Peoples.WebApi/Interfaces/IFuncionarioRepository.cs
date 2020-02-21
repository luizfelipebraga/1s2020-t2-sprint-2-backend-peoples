using Senai.Peoples.WebApi.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Senai.Peoples.WebApi.Interfaces
{
    interface IFuncionarioRepository
    {
        /// <summary>
        /// Lista todos os gêneros
        /// </summary>
        /// <returns>Retorna uma lista de gêneros</returns>
        List<FuncionarioDomain> Listar();

        /// <summary>
        /// Cadastra um novo gênero
        /// </summary>
        /// <param name="funcionario">Objeto genero que será cadastrado</param>
        void Cadastrar(FuncionarioDomain funcionario);

        /// <summary>
        /// Atualiza um gênero existente passando o id pela url da requisição
        /// </summary>
        /// <param name="id">ID do filme que será atualizado</param>
        /// <param name="funcionario">Objeto filme que será atualizado</param>
        void AtualizarIdUrl(int id, FuncionarioDomain funcionario);

        /// <summary>
        /// Deleta um filme
        /// </summary>
        /// <param name="id">ID do filme que será deletado</param>
        void Deletar(int id);

        /// <summary>
        /// Busca um filme através do ID
        /// </summary>
        /// <param name="id">ID do filme que será buscado</param>
        /// <returns>Retorna um genero</returns>
        FuncionarioDomain BuscarPorId(int id);

        List<FuncionarioDomain> BuscarPorNome(string nome);

    }
}
