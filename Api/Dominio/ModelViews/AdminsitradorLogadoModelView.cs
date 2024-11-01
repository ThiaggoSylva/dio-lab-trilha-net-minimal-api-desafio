using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.ModelViews
{
    public struct AdminsitradorLogadoModelView
    {
        public string Email {get; set;}
        public string Perfil { get; set; }
        public string Token { get; set; } 
    }
}