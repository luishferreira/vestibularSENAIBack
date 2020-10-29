using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vestibularCNG;
using vestibularMDL;

namespace vestibular.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        [Route("VerifyExistsCPF")]
        public ActionResult<bool> LogaUsuario([FromQuery] string cpf)
        {
            try
            {
                return(UsuarioNG.Instance.VerifyExistsCPF(cpf));
            }
            catch (Exception)
            {
                return BadRequest("Erro ao cadastrar o cliente");
            }

        }

        [HttpPost]
        [Route("CadastraAluno")]
        public ActionResult<int> CadastraAluno([FromBody] Aluno aluno)
        {
            try
            {
                return Ok(UsuarioNG.Instance.CadastraAluno(aluno));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("ConfirmRegistration")]
        public void ConfirmRegistration([FromQuery] string token)
        {
            try
            {
                UsuarioNG.Instance.DecodeTokenConfirmacao(token);
                Response.Redirect("http://localhost:3000/confirmarEmail");
            }
            catch (Exception ex)
            {
                BadRequest("");
            }
        }

        [HttpPost]
        [Route("LogarAluno")]
        public ActionResult<Aluno> LogarAluno([FromBody] Aluno aluno)
        {
            try
            {
                return Ok(UsuarioNG.Instance.LogarAluno(aluno));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("ReenviarEmail")]
        public void ReenviarEmail([FromBody] Aluno aluno)
        {
            try
            {
                UsuarioNG.Instance.EnviarEmailConfirmacao(aluno);
            }
            catch (Exception ex)
            {
                BadRequest("");
            }
        }

        [HttpGet]
        [Route("EsqueciMinhaSenha")]
        public ActionResult<string> EsqueciMinhaSenha([FromQuery] string email, string cpf)
        {
            try
            {
                UsuarioNG.Instance.EsqueciMinhaSenha(email, cpf);
                return Ok();
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [HttpGet]
        [Route("ConfirmarEsqueciSenha")]
        public void ConfirmarEsqueciSenha([FromQuery] string token)
        {
            try
            {
                var cpf = UsuarioNG.Instance.DecodeTokenEsqueciSenha(token);
                Response.Redirect("http://localhost:3000/confirmarTrocaSenha?" + cpf);
            }
            catch (Exception ex)
            {
                //redirecionar para erro
                //Response.Redirect("http://localhost:3000/confirmarEmail?" + cpf); 
            }
        }

        [HttpPost]
        [Route("AlterarSenha")]
        public ActionResult<string> AlterarSenha([FromBody] Aluno aluno)
        {
            try
            {
                return Ok(UsuarioNG.Instance.AlterarSenha(aluno));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }




    }
}
