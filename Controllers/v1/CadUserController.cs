using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project_API_SJP.IRepository;
using Project_API_SJP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_API_SJP.Controllers.v1
{
     [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CadUserController : ControllerBase
    {
        private readonly IConfiguration _config;

        private readonly IRepository_user _user_conect;

        public CadUserController(IConfiguration configuration, IRepository_user user)
        {
            _config = configuration;
            _user_conect = user;
        }

        /// <summary>
        /// Lista todos os Usuario cadastrados no banco de dados
        /// </summary>
        /// <remarks>
        /// Não é possível retornar o Usuario sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada: Mínimo 1</param>
        /// <param name="qtde">Indica a quantidade de registros por página: Mínimo 1 e máximo 5</param>
        /// <response code="200">Retorna a lista de usuario</response>
        /// <response code="204">Retorna se não houver nenhum usuario cadastrado</response>

        [HttpGet]
       
        public async Task<ActionResult<IEnumerable<User>>> GetAllJogos([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int qtde = 5)
        {
            var result = await _user_conect.GetUserAsyncAll(pagina, qtde);

            if (result.Count() == 0)
                return BadRequest("Ocorre um erro na Pesquisa."); ;

            return Ok(result);
        }

        /// <summary>
        /// Pesquisa usuario por Id
        /// </summary>
        /// <remarks>
        /// O Id do usuario é obrigatório na Consulta
        /// </remarks>
        ///
        [HttpGet("{Id}")]
        public async Task<ActionResult<User>> GetByIdUser([FromRoute] int Id)
        {
            var jogo = await _user_conect.GetUserAsyncById(Id);
            if (jogo == null)
                return BadRequest("Não foi encontrado usuario com Id informado.");

            return Ok(jogo);
        }

        /// <summary>
        /// Rotina para Cadastra Novo usuario
        /// </summary>
        /// <remarks>
        /// O campo de Nome usuario, paswword e email são obrigatórios.
        /// O campo senha deve ser de no mínimo 6 caracter.
        /// </remarks>
        /// 

        [HttpPost]
        public async Task<ActionResult<User>> post([FromBody] User user)
        {
            var result = await _user_conect.Inserir(user);
            if (result == null)
                return BadRequest("Cadastro não realizado.");

            return Ok(result);
        }


        /// <summary>
        /// Rotina para Alterar o Cadastro do usuario
        /// </summary>
        /// <remarks>
        /// Obrigatórios Informar o Id para alterar as informações do usuario
        /// </remarks>
        /// 
        [HttpPut("{Id}")]
        public async Task<ActionResult> Updateuser([FromRoute] int Id, [FromBody] User Entity)
        {
            int ret = await _user_conect.Update(Id, Entity);
            if (ret != 0)
                return Ok("Usuario atualizado com sucesso.");

            return Ok("O Usuario informado não esta cadastrado no sistema.");
        }

        /// <summary>
        /// Rotina para alterar somente o preço do usuario
        /// </summary>
        /// <remarks>
        /// Obrigatórios informar o ID para atualizar a senha
        /// </remarks>
        /// 
        [HttpPatch("{Id}/senha/{password}")]
        public async Task<ActionResult> Update_password([FromRoute] int Id, [FromRoute] string password)
        {
            int ret = await _user_conect.Update_Password(Id, password);
            if (ret != 0)
                return Ok("A senha foi atualizado.");

            return Ok("O usuario informado não esta cadastrado no sistema.");
        }

        /// <summary>
        /// Rotina para Excluir o usuario cadastrado
        /// </summary>
        /// <remarks>
        /// Obrigatórios informar o Id do usuario para excluir
        /// </remarks>
        /// 
        [HttpDelete("{Id}")]
        public async Task<ActionResult> ExcluirJogos([FromRoute] int Id)
        {

            int ret = await _user_conect.Remover(Id);
            if (ret != 0)
                return Ok("O Usuario foi excluido Com sucesso.");

            return Ok("O Usuario informado não esta cadastrado no sistema.");


        }

    }
}
