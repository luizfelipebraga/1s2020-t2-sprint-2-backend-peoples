using senai.Peoples.WebApi.Domains;
using senai.Peoples.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace senai.Peoples.WebApi.Repositories
{
    public class TipoUsuarioRepository : ITipoUsuarioRepository
    {
        private string stringConexao = "Data Source=DEV301\\SQLEXPRESS; initial catalog=Peoples; user Id=sa; pwd=sa@132";

        public void Atualizar(int id, TipoUsuarioDomain usuarioAtualizado)
        {
            // Declara a conexão passando a string de conexão
            using (SqlConnection con = new SqlConnection(stringConexao))
            {
                // Declara a query que será executada
                string queryUpdate = "UPDATE TipoUsuario " +
                                     "SET NomeTipoUsuario = @Email, Senha = @Senha, IdTipoUsuario = @IdTipoUsuario " +
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
