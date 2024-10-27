using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.DTOs;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Interfaces;
using dio_lab_trilha_net_minimal_api_desafio.Infraestrutura.Db;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.Servicos
{
    public class AdministradorServico(DBContexto contexto) : IAdministradorServico
    {
        private readonly DBContexto _contexto = contexto;

        public Administrador? Login(LoginDTO loginDTO)
        {
            return _contexto.Administradores.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
        }
    }
}