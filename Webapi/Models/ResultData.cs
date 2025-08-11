using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webapi.Models
{
    public class ResultData
    {
        public UsuarioData Usuario { get; set; }
        public string Role { get; set; }
        public  string Token { get; set; }
    }
}