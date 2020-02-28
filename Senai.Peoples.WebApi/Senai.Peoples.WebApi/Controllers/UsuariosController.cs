using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using senai.Peoples.WebApi.Domains;
using senai.Peoples.WebApi.Interfaces;
using senai.Peoples.WebApi.Repositories;

namespace senai.Peoples.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        /// <summary>
        /// Cria um objeto _usuarioRepository que irá receber todos os métodos definidos na interface
        /// </summary>
        private IUsuarioRepository usuarioRepository { get; set; }

        /// <summary>
        /// Instancia este objeto para que haja a referência aos métodos no repositório
        /// </summary>
        public UsuariosController()
        {
            usuarioRepository = new UsuarioRepository();
        }

        /// <summary>
        /// Lista todos os usuarios
        /// </summary>
        /// <returns>Retorna uma lista de usuarios e um status code 200 - Ok</returns>
        /// dominio/api/usuarios
        /// 
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Get()
        {
            // Faz a chamada para o método .Listar()
            // Retorna a lista e um status code 200 - Ok
            return Ok(usuarioRepository.Listar());
        }

        /// <summary>
        /// Cadastra um novo funcionário
        /// </summary>
        /// <param name="novousuario">Objeto novousuario que será cadastrado</param>
        /// <returns>Retorna os dados que foram enviados para cadastro e um status code 201 - Created</returns>
        /// dominio/api/usuarios
        /// 
        [Authorize(Roles = "Comum")]
        [HttpPost]
        public IActionResult Post(UsuarioDomain novoUsuario)
        {
            if (novoUsuario.Email == null)
            {
                return BadRequest("O email do usuario é obrigatório!");
            }
            // Faz a chamada para o método .Cadastrar();
            usuarioRepository.Cadastrar(novoUsuario);

            // Retorna o status code 201 - Created com a URI e o objeto cadastrado
            return Created("http://localhost:5000/api/Usuarios", novoUsuario);
        }

        /// <summary>
        /// Busca um funcionário através do seu ID
        /// </summary>
        /// <param name="id">ID do funcionário que será buscado</param>
        /// <returns>Retorna um funcionário buscado ou NotFound caso nenhum seja encontrado</returns>
        /// dominio/api/usuarios/1
        /// 
        [Authorize(Roles = "Administrador")]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            // Cria um objeto usuarioBuscado que irá receber o funcionário buscado no banco de dados
            UsuarioDomain usuarioBuscado = usuarioRepository.BuscarPorId(id);

            // Verifica se algum funcionário foi encontrado
            if (usuarioBuscado != null)
            {
                // Caso seja, retorna os dados buscados e um status code 200 - Ok
                return Ok(usuarioBuscado);
            }

            // Caso não seja, retorna um status code 404 - NotFound com a mensagem
            return NotFound("Nenhum usuário encontrado para o identificador informado");
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
        public IActionResult Put(int id, UsuarioDomain usuarioAtualizado)
        {
            // Cria um objeto usuarioBuscado que irá receber o funcionário buscado no banco de dados
            UsuarioDomain usuarioBuscado = usuarioRepository.BuscarPorId(id);

            // Verifica se algum funcionário foi encontrado
            if (usuarioBuscado != null)
            {
                // Tenta atualizar o registro
                try
                {
                    // Faz a chamada para o método .Atualizar();
                    usuarioRepository.Atualizar(id, usuarioAtualizado);

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
                        mensagem = "Funcionário não encontrado",
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
            UsuarioDomain usuarioBuscado = usuarioRepository.BuscarPorId(id);

            // Verifica se o funcionário foi encontrado
            if (usuarioBuscado != null)
            {
                // Caso seja, faz a chamada para o método .Deletar()
                usuarioRepository.Deletar(id);

                // e retorna um status code 200 - Ok com uma mensagem de sucesso
                return Ok($"O funcionário {id} foi deletado com sucesso!");
            }

            // Caso não seja, retorna um status code 404 - NotFound com a mensagem
            return NotFound("Nenhum funcionário encontrado para o identificador informado");
        }

        [HttpPost("Login/{login}")]
        public IActionResult Login (UsuarioDomain login)
        {
            // Busca o usuário pelo e-mail e senha
            UsuarioDomain usuarioBuscado = usuarioRepository.BuscarPorEmailSenha(login.Email, login.Senha);

            // Caso não encontre nenhum usuário com o e-mail e senha informados
            if (usuarioBuscado == null)
            {
                // Retorna NotFound com uma mensagem de erro
                return NotFound("E-mail ou senha inválidos");
            }

            // Caso o usuário seja encontrado, prossegue para a criação do token

            // Define os dados que serão fornecidos no token - Payload
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, usuarioBuscado.IdTipoUsuario.ToString()),
                new Claim("Claim personalizada", "Valor teste")
            };

            // Define a chave de acesso ao token
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("funcionarios-chave-autenticacao"));

            // Define as credenciais do token - Header
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Gera o token
            var token = new JwtSecurityToken(
                issuer: "Funcionarios.WebApi",                // emissor do token
                audience: "Funcionarios.WebApi",              // destinatário do token
                claims: claims,                         // dados definidos acima
                expires: DateTime.Now.AddMinutes(30),   // tempo de expiração
                signingCredentials: creds               // credenciais do token
            );

            // Retorna Ok com o token
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

    }
}