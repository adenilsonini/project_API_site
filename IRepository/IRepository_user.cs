using Project_API_SJP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_API_SJP.IRepository
{
   public interface IRepository_user
    {

        //Login Geral
        Task<User> Inserir(User user);
        Task<int> Update(int Id, User user);
        Task<int> Update_Password(int Id, string password);
        Task<int> Remover(int Id);
        Task<User> GetLogin(string email, string senha);

        Task<User> GetById(int id);

        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        //User
        Task<List<User>> GetUserAsyncAll(int pagina, int qtde);
        Task<User> GetUserAsyncById(int UserId);
        Task<User> GetUserAsyncByCad(string UsarName, string Password);
        // Task<User[]> GetAlunoAsyncBy_name(string aluno_name, bool includeProfessor);

    }
}
