using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Servicos;
using dio_lab_trilha_net_minimal_api_desafio.Infraestrutura.Db;
using Microsoft.EntityFrameworkCore;

namespace Testes.Domain.Servicos
{
    [TestClass]
    public class AdministradorServicoTest
    {
        private static DBContexto CriarContextoDeTeste()
        {
            var options = new DbContextOptionsBuilder<DBContexto>()
                .UseSqlite("Filename=:memory:")
                .Options;

            var context = new DBContexto(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }

        [TestMethod]
        public void TestarIncluir()
        {
            // Arrange (preparar)
            using var context = CriarContextoDeTeste();
            Administrador adm = new()
            {
                Email = "teste@teste.com",
                Senha = "teste",
                Perfil = "Adm"
            };
            var administradorServico = new AdministradorServico(context);

            // Act (realizar a ação de fato)
            administradorServico.Incluir(adm);

            // Assert (Checar se bate com o que espera)
            // Já existe um quando é criado (Seed admin)
            Assert.AreEqual(2, administradorServico.Todos(1).Count);
        }

        [TestMethod]
        public void TestarBuscarPorID()
        {
            // Arrange (preparar)
            using var context = CriarContextoDeTeste();
            // Seed admin
            Administrador adm = new()
            {
                Id = 1,
                Email = "administrador@teste.com",
                Senha = "123456",
                Perfil = "Adm"
            };
            var administradorServico = new AdministradorServico(context);

            // Act (realizar a ação de fato)
            var admBanco = administradorServico.BuscarPorId(adm.Id);

            // Assert (Checar se bate com o que espera)
            // Já existe um quando é criado (Seed admin)
            Assert.IsNotNull(admBanco);
            Assert.AreEqual(adm.Id, admBanco.Id);
            Assert.AreEqual(adm.Email, admBanco.Email);
            Assert.AreEqual(adm.Senha, admBanco.Senha);
            Assert.AreEqual(adm.Perfil, admBanco.Perfil);
        }
    }
}