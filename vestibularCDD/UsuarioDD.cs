using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using vestibularMDL;

namespace vestibularCDD
{
    public class UsuarioDD
    {
        private static UsuarioDD instance;
        private UsuarioDD() { }
        public static UsuarioDD Instance
        {
            get
            {
                if (instance == null)
                    instance = new UsuarioDD();
                return instance;
            }
        }

        public bool VerifyExistsCPF(string cpf)
        {
            int id = 0;
            try
            {
                using (SqlConnection conexao = new SqlConnection("Data Source =.; Initial Catalog =Vestibular; Integrated Security = True;"))
                {
                    using (SqlCommand command = new SqlCommand("dbo.VerifyExistsCPF", conexao) { CommandType = CommandType.StoredProcedure })
                    {
                        command.Parameters.Add("@cpf", SqlDbType.VarChar).Value = cpf;

                        conexao.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                id = Convert.ToInt32(reader["idAluno"]);
                            }
                        }
                    }
                }
                if (id > 0) return true;
                else return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao cadastrar o Aluno", ex);
            }
        }

        public Aluno CadastraAluno(Aluno aluno)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection("Data Source =.; Initial Catalog =Vestibular; Integrated Security = True;"))
                {
                    using (SqlCommand command = new SqlCommand("dbo.CadastraAluno", conexao) { CommandType = CommandType.StoredProcedure })
                    {
                        command.Parameters.Add("@nome", SqlDbType.VarChar).Value = aluno.Nome;
                        command.Parameters.Add("@cpf", SqlDbType.Char).Value = aluno.CPF;
                        command.Parameters.Add("@email", SqlDbType.VarChar).Value = aluno.Email;
                        command.Parameters.Add("@telefone", SqlDbType.VarChar).Value = aluno.Telefone;
                        command.Parameters.Add("@senha", SqlDbType.VarChar).Value = aluno.Senha;

                        command.Parameters.Add("@idAluno", SqlDbType.Int).Direction = ParameterDirection.Output;

                        conexao.Open();
                        command.ExecuteNonQuery();
                        aluno.IdAluno = Convert.ToInt32(command.Parameters["@idAluno"].Value);
                    }
                }
                return aluno;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao cadastrar o Aluno", ex);
            }
        }

        public Aluno LogarAluno(Aluno aluno)
        {
            Aluno returnAluno = new Aluno();
            try
            {
                using (SqlConnection conexao = new SqlConnection("Data Source =.; Initial Catalog =Vestibular; Integrated Security = True;"))
                {
                    using (SqlCommand command = new SqlCommand("dbo.LogarAluno", conexao) { CommandType = CommandType.StoredProcedure })
                    {
                        command.Parameters.Add("@cpf", SqlDbType.Char).Value = aluno.CPF;
                        command.Parameters.Add("@senha", SqlDbType.VarChar).Value = aluno.Senha;

                        conexao.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                returnAluno = new Aluno
                                {
                                    IdAluno = Convert.ToInt32(reader["idAluno"]),
                                    IdPerfilUsuario = Convert.ToInt32(reader["idPerfilUsuario"]),
                                    CPF = Convert.ToString(reader["CPF"]),
                                    IsValido = Convert.ToBoolean(reader["isValido"]),
                                    Email = Convert.ToString(reader["Email"])
                                };
                            }
                        }
                    }
                }
                return returnAluno;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao realizar o login", ex);
            }
        }

        public void ConfirmarCadastro(int id)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection("Data Source =.; Initial Catalog =Vestibular; Integrated Security = True;"))
                {
                    using (SqlCommand command = new SqlCommand("UPDATE dbo.Aluno SET isValido=1 WHERE idAluno=" + id, conexao))
                    {
                        conexao.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possivel confirmar o cadastro", ex);
            }
        }

        public bool EsqueciMinhaSenha(string email, string cpf)
        {
            try
            {
                using (SqlConnection conexao = new SqlConnection("Data Source =.; Initial Catalog =Vestibular; Integrated Security = True;"))
                {
                    using (SqlCommand command = new SqlCommand("dbo.VerifyEmailCPF", conexao) { CommandType = CommandType.StoredProcedure })
                    {
                        command.Parameters.Add("@cpf", SqlDbType.VarChar).Value = cpf;
                        command.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                        conexao.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                return true;
                            else
                                return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao cadastrar o Aluno", ex);
            }
        }

        public string AlterarSenha(Aluno aluno)
        {
            string CPF = "";
            try
            {
                using (SqlConnection conexao = new SqlConnection("Data Source =.; Initial Catalog =Vestibular; Integrated Security = True;"))
                {
                    using (SqlCommand command = new SqlCommand("dbo.AlterarSenha", conexao) { CommandType = CommandType.StoredProcedure })
                    {
                        command.Parameters.Add("@cpf", SqlDbType.Char).Value = aluno.CPF;
                        command.Parameters.Add("@senha", SqlDbType.VarChar).Value = aluno.Senha;

                        command.Parameters.Add("@return", SqlDbType.VarChar,11).Direction = ParameterDirection.Output;

                        conexao.Open();
                        command.ExecuteNonQuery();
                        CPF = Convert.ToString(command.Parameters["@return"].Value);
                    }
                }
                return CPF;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao alterar a senha", ex);
            }
        }
    }
}
