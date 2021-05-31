using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_API_SJP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project_API_SJP.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UploadController : Controller
    {
        
        [HttpPost("img")]
        public async Task<IActionResult> Upload([FromForm] string titleimg, List<IFormFile> files)
        {
            try
            {
                var result = new List<FileUploadResult>();
                long size = files.Sum(f => f.Length);
                foreach (var file in files)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/files/image", file.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    await file.CopyToAsync(stream);
                    result.Add(new FileUploadResult() { Name = file.FileName, Length = file.Length });
                }
                return Ok("A imagem foi Recebida com sucesso !");
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

    }
}
