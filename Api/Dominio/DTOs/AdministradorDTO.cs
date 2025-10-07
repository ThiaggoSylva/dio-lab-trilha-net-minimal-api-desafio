using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Enums;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.DTOs
{
    //
    public class AdministradorDTO
    {
        public string Email { get; set; }

        public string Senha { get; set; }

        public Perfil Perfil { get; set; }
    }
}