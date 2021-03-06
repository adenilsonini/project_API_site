using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Project_API_SJP.IRepository;
using System.Linq;
using System.Threading.Tasks;


namespace Project_API_SJP.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
    

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IRepository_user userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateToken(token);
            if (userId != null)
            {
                // attach user to context on successful jwt validation
                context.Items["User"] = userService.GetById(userId.Value);
            }

            await _next(context);
        }
    }
}