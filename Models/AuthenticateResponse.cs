using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_API_SJP.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
