using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Dtos
{
    public class UsuarioDto
    {
        public string nombreUsuario { get; set; }
        public byte[] PasswordHash { get; set; }

    }
}
