using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Cache;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using vestibularCDD;
using vestibularMDL;
using System.Web;
using System.Net;

namespace vestibularCNG
{
    public class UsuarioNG
    {
        private static UsuarioNG instance;
        private UsuarioNG() { }
        public static UsuarioNG Instance
        {
            get
            {
                if (instance == null)
                    instance = new UsuarioNG();
                return instance;
            }
        }

        public bool VerifyExistsCPF(string cpf)
        {
            return (UsuarioDD.Instance.VerifyExistsCPF(cpf));
        }

        public int CadastraAluno(Aluno aluno)
        {
            aluno = UsuarioDD.Instance.CadastraAluno(aluno);
            if (aluno.IdAluno > 0)
            {
                EnviarEmailConfirmacao(aluno);
            }
            return aluno.IdAluno;
        }

        public void EnviarEmailConfirmacao(Aluno aluno)
        {
            string emailTitle = "Confirme seu email";
            //fazer logica de tempo depois se der tempo
            //byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            byte[] id = Encoding.ASCII.GetBytes(aluno.IdAluno.ToString());
            //fazer logica de tempo depois se der tempo
            //var teste = time.Concat(key);
            string token = Convert.ToBase64String(key.Concat(id).ToArray());
            string url = "https://localhost:44382/Usuario/ConfirmRegistration?token=" + HttpUtility.UrlEncode(token);
            string emailContent = "Para confirmar seu email <a href=\"" + url + "\">clique aqui</a>";
            EmailNG.Instance.EnviarEmail(aluno.Email, emailTitle, emailContent);
        }

        public void DecodeTokenConfirmacao(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            //var a = data.Take(24).ToArray();
            var b = Encoding.ASCII.GetString(data.Skip(16).ToArray());
            UsuarioDD.Instance.ConfirmarCadastro(Convert.ToInt32(b));
            //fazer logica de tempo depois se der tempo
            //DateTime when = DateTime.FromBinary(BitConverter.ToInt64(a, 0));
            //if (when < DateTime.UtcNow.AddHours(-24))
            //{
            //    //too old
            //}
        }

        public Aluno LogarAluno(Aluno aluno)
        {
            return UsuarioDD.Instance.LogarAluno(aluno);
        }

        public void EsqueciMinhaSenha(string email, string cpf)
        {
            if (UsuarioDD.Instance.EsqueciMinhaSenha(email, cpf))
            {
                EnviarEmailEsqueciSenha(email, cpf);
            }
            else
                throw new Exception("email");
        }

        public void EnviarEmailEsqueciSenha(string email, string cpf)
        {
            string emailTitle = "Confirme seu email";
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            byte[] id = Encoding.ASCII.GetBytes(cpf.ToString());
            var teste = time.Concat(key);
            string token = Convert.ToBase64String(teste.Concat(id).ToArray());
            string url = "https://localhost:44382/Usuario/ConfirmarEsqueciSenha?token=" + HttpUtility.UrlEncode(token);
            string emailContent = "Você solicitou um esquecimento de senha. Para trocar sua senha <a href=\"" + url + "\">clique aqui</a>";
            EmailNG.Instance.EnviarEmail(email, emailTitle, emailContent);
        }

        public string DecodeTokenEsqueciSenha(string token)
        {
            byte[] array = Convert.FromBase64String(token);
            var data = array.Take(24).ToArray();
            var cpfArray = array.Skip(24).ToArray();
            var cpf = Convert.ToBase64String(cpfArray);
            //var cpf = Encoding.ASCII.GetString(array.Skip(24).ToArray());
            DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            if (when < DateTime.UtcNow.AddHours(-24))
            {
                throw new Exception("Este link expirou, por favor, gere um novo link para alterar a senha");
            }
            return HttpUtility.UrlEncode(cpf);
        }

        public string AlterarSenha(Aluno aluno)
        {
            byte[] array = Convert.FromBase64String(HttpUtility.UrlDecode(aluno.CPF));
            aluno.CPF = Encoding.ASCII.GetString(array.ToArray());
            return UsuarioDD.Instance.AlterarSenha(aluno);
        }

    }
}
