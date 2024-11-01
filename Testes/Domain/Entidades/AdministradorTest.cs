using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;

namespace Testes.Domain.Entidades
{
    [TestClass]
    public class AdministradorTest
    {
        [TestMethod]
        public void TestarGetSetPropriedades()
        {
            // Arrange (preparar)
            Administrador adm = new();
            string emailEsperado = "teste@teste.com";
            string senhaEsperada = "teste";
            string perfilEsperado = "Adm";

            // Act (realizar a ação de fato)
            adm.Id = 1;
            adm.Email = emailEsperado;
            adm.Senha = senhaEsperada;
            adm.Perfil = perfilEsperado;

            // Assert (Checar se bate com o que espera)
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual(emailEsperado, adm.Email);
            Assert.AreEqual(senhaEsperada, adm.Senha);
            Assert.AreEqual(perfilEsperado, adm.Perfil);
        }
    }
}