using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project_API_SJP.Models
{
    public class FileUploadResult
    {
        public long Length { get; set; }
        public string Name { get; set; }
    }
}
