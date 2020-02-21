using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senai.Peoples.WebApi.Domains;
using Senai.Peoples.WebApi.Interfaces;
using Senai.Peoples.WebApi.Repositories;

namespace Senai.Peoples.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FuncionariosController : ControllerBase
    {
        /// <summary>
        /// Cria um objeto _generoRepository que irá receber todos os métodos definidos na interface
        /// </summary>
        private IFuncionarioRepository funcionarioRepository { get; set; }

        /// <summary>
        /// Instancia este objeto para que haja a referência aos métodos no repositório
        /// </summary>
        public FuncionariosController()
        {
            funcionarioRepository = new FuncionarioRepository();
        }

        /// <summary>
        /// Lista todos os gêneros
        /// </summary>
        /// <returns>Retorna uma lista de gêneros</returns>
        /// dominio/api/Generos
        [HttpGet]
        public IEnumerable<FuncionarioDomain> Get()
        {
            // Faz a chamada para o método .Listar();
            return funcionarioRepository.Listar();
        }

        /// <summary>
        /// Cadastra um novo gênero
        /// </summary>
        /// <param name="novofuncionario">Objeto genero recebido na requisição</param>
        /// <returns>Retorna um status code 201 (created)</returns>
        /// dominio/api/Generos
        [HttpPost]
        public IActionResult Post(FuncionarioDomain novofuncionario)
        {
            // Faz a chamada para o método .Cadastrar();
            funcionarioRepository.Cadastrar(novofuncionario);

            // Retorna um status code 201 - Created
            return StatusCode(201);
        }

        /// <summary>
        /// Busca um gênero através do seu ID
        /// </summary>
        /// <param name="id">ID do gênero buscado</param>
        /// <returns>Retorna um gênero buscado</returns>
        /// dominio/api/Generos/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Cria um objeto generoBuscado que irá receber o gênero buscado no banco de dados
            FuncionarioDomain funcionarioBuscado = funcionarioRepository.BuscarPorId(id);

            // Verifica se nenhum gênero foi encontrado
            if (funcionarioBuscado == null)
            {
                // Caso não seja encontrado, retorna um status code 404 com a mensagem personalizada
                return NotFound("Nenhum funcionario encontrado");
            }

            // Caso seja encontrado, retorna o gênero buscado
            return Ok(funcionarioBuscado);
        }

        [HttpGet("Nome/{nome}")]
        public IEnumerable <FuncionarioDomain> GetByNome(string nome)
        {
            Console.WriteLine();
            Console.WriteLine(nome);
            Console.WriteLine();
            return funcionarioRepository.BuscarPorNome(nome);
        }

        /// <summary>
        /// Atualiza um gênero existente passando o ID no recurso
        /// </summary>
        /// <param name="id">ID do gênero que será atualizado</param>
        /// <param name="funcionarioAtualizado">Objeto gênero que será atualizado</param>
        /// <returns>Retorna um status code</returns>
        /// dominio/api/Generos/1
        /// 
        [HttpPut("{id}")]
        public IActionResult PutIdUrl(int id, FuncionarioDomain funcionarioAtualizado)
        {
            // Cria um objeto generoBuscado que irá receber o gênero buscado no banco de dados
            FuncionarioDomain funcionarioBuscado = funcionarioRepository.BuscarPorId(id);

            // Verifica se nenhum gênero foi encontrado
            if (funcionarioBuscado == null)
            {
                // Caso não seja encontrado, retorna NotFound com uma mensagem personalizada
                // e um bool para representar que houve erro
                return NotFound
                    (
                        new
                        {
                            mensagem = "Funcionario não encontrado",
                            erro = true
                        }
                    );
            }

            // Tenta atualizar o registro
            try
            {
                // Faz a chamada para o método .AtualizarIdUrl();
                funcionarioRepository.AtualizarIdUrl(id, funcionarioAtualizado);

                // Retorna um status code 204 - No Content
                return NoContent();
            }
            // Caso ocorra algum erro
            catch (Exception erro)
            {
                // Retorna BadRequest e o erro
                return BadRequest(erro);
            }
        }

        /// <summary>
        /// Deleta um gênero passando o ID
        /// </summary>
        /// <param name="id">ID do gênero que será deletado</param>
        /// <returns>Retorna um status code com uma mensagem personalizada</returns>
        /// dominio/api/Generos/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Faz a chamada para o método .Deletar();
            funcionarioRepository.Deletar(id);

            // Retorna um status code com uma mensagem personalizada
            return Ok("Funcionario deletado");
        }
    }
}