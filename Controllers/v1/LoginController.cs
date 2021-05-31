using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project_API_SJP.IRepository;
using Project_API_SJP.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_API_SJP.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;

        private IMapper _mapper;

        private readonly IRepository_user _user_conect;

        public LoginController(IConfiguration configuration, IRepository_user user, IMapper mapper)
        {
            _config = configuration;
            _user_conect = user;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _user_conect.Authenticate(model);
            return Ok(response);
        }


    }
}
