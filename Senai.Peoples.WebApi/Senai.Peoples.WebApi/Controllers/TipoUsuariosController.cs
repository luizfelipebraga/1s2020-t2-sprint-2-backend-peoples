using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using senai.Peoples.WebApi.Domains;
using senai.Peoples.WebApi.Interfaces;
using senai.Peoples.WebApi.Repositories;

namespace senai.Peoples.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoUsuariosController : ControllerBase
    {
        private ITipoUsuarioRepository tipousuarioRepository { get; set; }

        public TipoUsuariosController()
        {
            tipousuarioRepository = new TipoUsuarioRepository();
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Get()
        {
            // Faz a chamada para o método .Listar()
            // Retorna a lista e um status code 200 - Ok
            return Ok(tipousuarioRepository.Listar());
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Cria um objeto usuarioBuscado que irá receber o funcionário buscado no banco de dados
            TipoUsuarioDomain tipousuarioBuscado = tipousuarioRepository.BuscarPorId(id);

            // Verifica se algum funcionário foi encontrado
            if (tipousuarioBuscado != null)
            {
                // Caso seja, retorna os dados buscados e um status code 200 - Ok
                return Ok(tipousuarioBuscado);
            }

            // Caso não seja, retorna um status code 404 - NotFound com a mensagem
            return NotFound("Nenhum tipo usuário encontrado para o identificador informado");
        }

        /// <summary>
        /// Atualiza um funcionário existente
        /// </summary>
        /// <param name="id">ID do funcionário que será atualizado</param>
        /// <param name="usuarioAtualizado">Objeto usuarioAtualizado que será atualizado</param>
        /// <returns>Retorna um status code</returns>
        /// dominio/api/usuarios/1
        /// 
        [Authorize(Roles = "Comum")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, TipoUsuarioDomain tipousuarioAtualizado)
        {
            // Cria um objeto usuarioBuscado que irá receber o funcionário buscado no banco de dados
            TipoUsuarioDomain tipousuarioBuscado = tipousuarioRepository.BuscarPorId(id);

            // Verifica se algum funcionário foi encontrado
            if (tipousuarioBuscado != null)
            {
                // Tenta atualizar o registro
                try
                {
                    // Faz a chamada para o método .Atualizar();
                    tipousuarioRepository.Atualizar(id, tipousuarioAtualizado);

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

            // Caso não seja encontrado, retorna NotFound com uma mensagem personalizada
            // e um bool para representar que houve erro
            return NotFound
                (
                    new
                    {
                        mensagem = "TipoUsuario não encontrado",
                        erro = true
                    }
                );
        }

        /// <summary>
        /// Deleta um funcionário
        /// </summary>
        /// <param name="id">ID do funcionário que será deletado</param>
        /// <returns>Retorna um status code com uma mensagem de sucesso ou erro</returns>
        /// dominio/api/usuarios/1
        /// 
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Cria um objeto usuarioBuscado que irá receber o funcionário buscado no banco de dados
            TipoUsuarioDomain tipousuarioBuscado = tipousuarioRepository.BuscarPorId(id);

            // Verifica se o funcionário foi encontrado
            if (tipousuarioBuscado != null)
            {
                // Caso seja, faz a chamada para o método .Deletar()
                tipousuarioRepository.Deletar(id);

                // e retorna um status code 200 - Ok com uma mensagem de sucesso
                return Ok($"O tipo usuário {id} foi deletado com sucesso!");
            }

            // Caso não seja, retorna um status code 404 - NotFound com a mensagem
            return NotFound("Nenhum tipo usuário encontrado para o identificador informado");
        }
    }
}