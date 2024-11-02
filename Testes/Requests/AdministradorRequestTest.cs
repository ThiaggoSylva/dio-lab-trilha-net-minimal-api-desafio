using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.DTOs;
using Testes.Helpers;

namespace Testes.Requests
{
    [TestClass]
    public class AdministradorRequestTest
    {
        [TestMethod]
        public async Task TestarLogin_Success()
        {
            // Arrange (preparar)
            var loginDTO = new LoginDTO{ Email = "administrador@teste.com", Senha = "123456"};
            var jsonContent = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");

            // Act (realizar a ação de fato)
            var response = await Setup.Client.PostAsync("/administradores/login", jsonContent);

            // Assert (Checar se bate com o que espera)
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(responseString.Contains("token"));
        }

        [TestMethod]
        public async Task TestarGetAdministradores_UnauthorizedWithoutToken()
        {
            // Arrange: vazio

            // Act: Chamar /administradores sem passar nada
            var response = await Setup.Client.GetAsync("/administradores");

            // Assert: Não autorizado
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}