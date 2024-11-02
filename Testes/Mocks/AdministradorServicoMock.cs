using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.DTOs;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Interfaces;

namespace Testes.Mocks
{
    public class AdministradorServicoMock : IAdministradorServico
    {
        private static List<Administrador> administradores = 
                [
                    new Administrador
                    {
                        Id = 1,
                        Email = "administrador@teste.com",
                        Senha = "123456",
                        Perfil = "Adm"
                    }
                ];

        public Administrador? BuscarPorId(int id)
        {
            return administradores.Find(a => a.Id == id);
        }

        public int ContarTotal()
        {
            return administradores.Count;
        }

        public Administrador Incluir(Administrador administrador)
        {
            administrador.Id = administradores.Count + 1;
            administradores.Add(administrador);
            Administrador adm = administradores.Last();
            return adm;
        }

        public Administrador? Login(LoginDTO loginDTO)
        {
            Administrador? admLogin = administradores.Find(a => a.Email == loginDTO.Email);
            return admLogin?.Senha == loginDTO.Senha? admLogin : null; 
        }

        public List<Administrador> Todos(int? pagina)
        {
            return administradores;
        }
    }
}