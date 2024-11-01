using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Enums;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.ModelViews
{
    public struct AdministradorModelView
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Perfil { get; set; }
    }
}