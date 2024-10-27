using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.DTOs;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.Interfaces
{
    public interface IAdministradorServico
    {
        Administrador? Login(LoginDTO loginDTO);
    }
}