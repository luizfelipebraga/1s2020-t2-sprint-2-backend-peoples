using senai.Peoples.WebApi.Domains;
using senai.Peoples.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace senai.Peoples.WebApi.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private string stringConexao = "Data Source=DEV301\\SQLEXPRESS; initial catalog=Peoples; user Id=sa; pwd=sa@132";

        public UsuarioDomain BuscarPorEmailSenha(string email, string senha)
        {
            // Define a conexão passando a string
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                // Define a query a ser executada no banco
                string querySelect = "SELECT IdUsuario, Email, Senha, IdTipoUsuario FROM Usuarios WHERE Email = @Email AND Senha = @Senha";

                // Define o comando passando a query e a conexão
                using (SqlCommand cmd = new SqlCommand(querySelect, con))
                {
                    // Define o valor dos parâmetros
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Senha", senha);

                    // Abre a conexão com o banco
                    con.Open();

                    // Executa o comando e armazena os dados no objeto rdr
                    SqlDataReader rdr = cmd.ExecuteReader();

                    // Caso dados tenham sido obtidos
                    if (rdr.HasRows)
                    {
                        // Cria um objeto usuario
                        UsuarioDomain usuario = new UsuarioDomain();

                        // Enquanto estiver percorrendo as linhas
                        while (rdr.Read())
                        {
                            // Atribui à propriedade IdUsuario o valor da coluna IdUsuario
                            usuario.IdUsuario = Convert.ToInt32(rdr["IdUsuario"]);

                            // Atribui à propriedade Email o valor da coluna Email
                            usuario.Email = rdr["Email"].ToString();

                            // Atribui à propriedade Senha o valor da coluna Senha
                            usuario.Senha = rdr["Senha"].ToString();

                            // Atribui à propriedade Permissao o valor da coluna Permissao
                            usuario.IdTipoUsuario = Convert.ToInt32(rdr["IdTipoUsuario"]);
                        }

                        // Retorna o objeto usuario
                        return usuario;
                    }
                }

                // Caso não encontre um email e senha correspondente, retorna null
                return null;
            }
        }

        public void Atualizar(int id, UsuarioDomain usuarioAtualizado)
        {
            // Declara a conexão passando a string de conexão
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                // Declara a query que será executada
                string queryUpdate = "UPDATE Usuarios " +
                                     "SET Email = @Email, Senha = @Senha, IdTipoUsuario = @IdTipoUsuario " +
                                     "WHERE IdUsuario = @ID";

                // Declara o SqlCommand passando o comando a ser executado e a conexão
                using (SqlCommand cmd = new SqlCommand(queryUpdate, con))
                {
                    // Passa os valores dos parâmetros
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Email", usuarioAtualizado.Email);
                    cmd.Parameters.AddWithValue("@Senha", usuarioAtualizado.Senha);
                    cmd.Parameters.AddWithValue("@IdTipoUsuario", usuarioAtualizado.IdTipoUsuario);

                    // Abre a conexão com o banco de dados
                    con.Open();

                    // Executa o comando
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public UsuarioDomain BuscarPorId(int id)
        {
            // Declara a conexão passando a string de conexão
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                // Declara a query que será executada
                string querySelectById = "SELECT IdUsuario, Email, Senha, IdUsuario FROM Usuarios" +
                                        "WHERE IdUsuario = @ID";

                // Abre a conexão com o banco de dados
                con.Open();

                // Declara o SqlDataReader para receber os dados do banco de dados
                SqlDataReader rdr;

                // Declara o SqlCommand passando o comando a ser executado e a conexão
                using (SqlCommand cmd = new SqlCommand(querySelectById, con))
                {
                    // Passa o valor do parâmetro
                    cmd.Parameters.AddWithValue("@ID", id);

                    // Executa a query e armazena os dados no rdr
                    rdr = cmd.ExecuteReader();

                    // Caso o resultado da query possua registro
                    if (rdr.Read())
                    {
                        // Instancia um objeto funcionario 
                        UsuarioDomain usuario = new UsuarioDomain
                        {
                            // Atribui à propriedade IdFuncionario o valor da coluna "IdFuncionario" da tabela do banco
                            IdUsuario = Convert.ToInt32(rdr["IdUsuario"])

                            // Atribui à propriedade Nome o valor da coluna "Nome" da tabela do banco
                            ,
                            Email = rdr["Email"].ToString()

                            // Atribui à propriedade Sobrenome o valor da coluna "Sobrenome" da tabela do banco
                            ,
                            Senha = rdr["Senha"].ToString()

                            // Atribui à propriedade DataNascimento o valor da coluna "DataNascimento" da tabela do banco
                            ,
                            IdTipoUsuario = Convert.ToInt32(rdr["TipoUsuario"])
                        };

                        // Retorna o funcionário buscado
                        return usuario;
                    }

                    // Caso o resultado da query não possua registros, retorna null
                    return null;
                }
            }
        }

        public void Cadastrar(UsuarioDomain novoUsuario)
        {
            // Declara a SqlConnection passando a string de conexão
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                // Declara a query que será executada
                string queryInsert = "INSERT INTO Usuarios(Email, Senha, IdTipoUsuario) " +
                                     "VALUES (@Email, @Senha, @IdTipoUsuario)";

                // Declara o comando passando a query e a conexão
                using (SqlCommand cmd = new SqlCommand(queryInsert, con))
                {
                    // Passa o valor do parâmetro
                    cmd.Parameters.AddWithValue("@Email", novoUsuario.Email);
                    cmd.Parameters.AddWithValue("@Senha", novoUsuario.Senha);
                    cmd.Parameters.AddWithValue("@IdTipoUsuario", novoUsuario.IdTipoUsuario);

                    // Abre a conexão com o banco de dados
                    con.Open();

                    // Executa o comando
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Deleta um funcionário existente
        /// </summary>
        /// <param name="id"></param>
        public void Deletar(int id)
        {
            // Declara a conexão passando a string de conexão
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                // Declara a query que será executada passando o valor como parâmetro
                string queryDelete = "DELETE FROM Usuarios WHERE IdUsuario = @ID";

                // Declara o comando passando a query e a conexão
                using (SqlCommand cmd = new SqlCommand(queryDelete, con))
                {
                    // Passa o valor do parâmetro
                    cmd.Parameters.AddWithValue("@ID", id);

                    // Abre a conexão com o banco de dados
                    con.Open();

                    // Executa o comando
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<UsuarioDomain> Listar()
        {
            // Cria uma lista funcionarios onde serão armazenados os dados
            List<UsuarioDomain> Usuarios = new List<UsuarioDomain>();

            // Declara a SqlConnection passando a string de conexão
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                // Declara a instrução a ser executada
                string querySelectAll = "SELECT * from Usuarios";

                // Abre a conexão com o banco de dados
                con.Open();

                // Declara o SqlDataReader para receber os dados do banco de dados
                SqlDataReader rdr;

                // Declara o SqlCommand passando o comando a ser executado e a conexão
                using (SqlCommand cmd = new SqlCommand(querySelectAll, con))
                {
                    // Executa a query e armazena os dados no rdr
                    rdr = cmd.ExecuteReader();

                    // Enquanto houver registros para serem lidos no rdr, o laço se repete
                    while (rdr.Read())
                    {
                        // Instancia um objeto funcionario 
                        UsuarioDomain usuario = new UsuarioDomain
                        {
                            // Atribui à propriedade IdFuncionario o valor da coluna "IdFuncionario" da tabela do banco
                            IdUsuario = Convert.ToInt32(rdr["IdUsuario"])

                            // Atribui à propriedade Nome o valor da coluna "Nome" da tabela do banco
                            ,
                            Email = rdr["Email"].ToString()

                            // Atribui à propriedade Sobrenome o valor da coluna "Sobrenome" da tabela do banco
                            ,
                            Senha = rdr["Senha"].ToString()

                            // Atribui à propriedade DataNascimento o valor da coluna "DataNascimento" da tabela do banco
                            ,
                            IdTipoUsuario = Convert.ToInt32(rdr["IdTipoUsuario"])
                        };

                        // Adiciona o funcionario criado à lista funcionarios
                        Usuarios.Add(usuario);
                    }
                }
            }

            // Retorna a lista de funcionarios
            return Usuarios;
        }

    }
}
