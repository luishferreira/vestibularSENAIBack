using System;
using System.Collections.Generic;
using System.Text;

namespace vestibularMDL
{
    public class Aluno
    {
        public int IdAluno { get; set; }
        public int IdPerfilUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public string Telefone { get; set; }
        public string Senha { get; set; }
        public bool IsValido{ get; set; }
    }
}
