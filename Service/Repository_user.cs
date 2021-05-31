using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Project_API_SJP.IRepository;
using Project_API_SJP.Models;
using Project_API_SJP.Service;
using AutoMapper;
using Project_API_SJP.ExceptionApp;
using Project_API_SJP.Authorization;

namespace Project_API_SJP.Service
{
    public class Repository_user : IRepository_user
    {
        private readonly SqliteConnection SqliteConnection;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;
        public Repository_user(IConfiguration configuration, IJwtUtils jwtUtils, IMapper mapper)
        {
            SqliteConnection = new SqliteConnection(configuration.GetConnectionString("Conn_sqlite"));
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public async Task<User> Inserir(User Entity)
        {

            // Rotina abaixo consulta se o jogo já esta cadastrodo com o mesmo Nome e preço
            var user_ret= await GetUserAsyncByCad(Entity.UserName, Entity.Password);

            if (user_ret != null)
            {
                return new User
                {
                    Id = user_ret.Id,
                    UserName = user_ret.UserName + ",  Obs.: O Usuario Já esta cadastrado no Id: " + user_ret.Id,
                    Email = user_ret.Email,
                };
            }

            // Rotina abaixo para cadastrar o jogo no banco de dados

            var comando = $"INSERT INTO User (UserName, Password, Email) VALUES ('{Entity.UserName}', '{Entity.Password}', '{Entity.Email}')";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            await sqliteCommand.ExecuteNonQueryAsync();
            sqliteCommand.Dispose();

            //Rotina abaixo para buscar o último codigo inserido no banco de dados
            //comando =  "SELECT * FROM jogos WHERE id = (SELECT max(Id) FROM jogos)";
            comando = "SELECT max(Id) FROM User";

            sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            var Id = await sqliteCommand.ExecuteScalarAsync();

            await SqliteConnection.CloseAsync();

            return new User
            {
                Id = Convert.ToInt32(Id),
                UserName = Entity.UserName,
                Email = Entity.Email,
                Password = Entity.Password,
            };

        }

        public async Task<int> Update(int Id, User Entity)
        {

            var comando = $"UPDATE User set UserName = '{Entity.UserName}', Password = '{Entity.Password}', Email = '{Entity.Email}' WHERE Id = '{Id}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            int ret = await sqliteCommand.ExecuteNonQueryAsync();

            await SqliteConnection.CloseAsync();

            return ret;
        }

        public async Task<int> Update_Password(int Id, string password)
        {

            var comando = $"UPDATE User set Password = '{password}' WHERE Id = '{Id}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            int ret = await sqliteCommand.ExecuteNonQueryAsync();

            await SqliteConnection.CloseAsync();

            return ret;
        }

        public async Task<int> Remover(int Id)
        {

            var comando = $"delete from User WHERE Id = '{Id}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            int ret = await sqliteCommand.ExecuteNonQueryAsync();

            await SqliteConnection.CloseAsync();

            return ret;
        }

        public async Task<List<User>> GetUserAsyncAll(int pagina, int qtde)
        {
            var List_user = new List<User>();

            var comando = $"select * from User order by id limit {qtde} offset {((pagina - 1) * qtde)}";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            SqliteDataReader dt = await sqliteCommand.ExecuteReaderAsync();

            while (dt.Read())
            {
                List_user.Add(new User
                {
                    Id = (int)(long)dt["Id"],
                    UserName = (string)dt["UserName"],
                    Email = (string)dt["Email"],
                    Password = (string)dt["Password"]
                });
            }

            await SqliteConnection.CloseAsync();
            return List_user;
        }

        public async Task<User> GetUserAsyncById(int id)
        {
            User UserId = null;

            var comando = $"select * from User where Id = '{id}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            SqliteDataReader dt = await sqliteCommand.ExecuteReaderAsync();

            while (dt.Read())
            {
                UserId = new User
                {
                    Id = (int)(long)dt["Id"],
                    UserName = (string)dt["UserName"],
                    Email = (string)dt["Email"],
                    Password = (string)dt["Password"]
                };
            }

            await SqliteConnection.CloseAsync();
            return UserId;
        }

        public async Task<User> GetUserAsyncByCad(string username, string password)
        {
            User UserId = null;

            var comando = $"select * from User where UserName = '{username}' and Password = '{password}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            SqliteDataReader dt = await sqliteCommand.ExecuteReaderAsync();

            while (dt.Read())
            {
                UserId = new User
                {
                    Id = (int)(long)dt["Id"],
                    UserName = (string)dt["UserName"],
                    Email = (string)dt["Email"],
                    Password = (string)dt["Password"]
                };
            }

            await SqliteConnection.CloseAsync();
            return UserId;
        }

        public async Task<User> GetLogin(string email, string password)
        {
            User UserId = null;

            var comando = $"select * from User where Email = '{email}' and Password = '{password}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
           
            SqliteDataReader dt = await sqliteCommand.ExecuteReaderAsync();

            while (dt.Read())
            {
                UserId = new User
                {
                    Id = (int)(long)dt["Id"],
                    UserName = (string)dt["UserName"],
                    Email = (string)dt["Email"],
                    Password = (string)dt["Password"]
                };
            }

            await SqliteConnection.CloseAsync();


            return UserId;
        }

        public async Task<User> GetById(int id)
        {
            return await getUser(id);
        }

        private async Task<User> getUser(int id)
        {

            User UserId = null;

            var comando = $"select * from User where Id = '{id}'";

            await SqliteConnection.OpenAsync();

            SqliteCommand sqliteCommand = new SqliteCommand(comando, SqliteConnection);
            SqliteDataReader dt = await sqliteCommand.ExecuteReaderAsync();

            while (dt.Read())
            {
                UserId = new User
                {
                    Id = (int)(long)dt["Id"],
                    
                };
            }


            await SqliteConnection.CloseAsync();

            
            if ( UserId == null) throw new KeyNotFoundException("User not found");
            return UserId;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await GetLogin(model.Username, model.Password);

            // validate
            if (user == null)
                throw new AppException("Username or password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }
    }
}
