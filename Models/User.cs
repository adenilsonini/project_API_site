using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Project_API_SJP.Models
{
    public class User
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(250)")]
        public string UserName { get; set; }

        [Column(TypeName = "VARCHAR(250)")]
        public string Email { get; set; }

        [Column(TypeName = "VARCHAR(250)")]
        public string Password { get; set; }
    }
}
