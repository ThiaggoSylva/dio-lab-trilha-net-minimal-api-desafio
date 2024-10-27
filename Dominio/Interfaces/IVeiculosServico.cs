using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.Interfaces
{
    public interface IVeiculosServico
    {
        List<Veiculo> Todos(int pagina = 1, string? nome = null, string? marca = null);
        Veiculo? BuscaPorId(int id);
        Veiculo Incluir(Veiculo veiculo);
        Veiculo Atualizar(Veiculo veiculo);
        void Apagar(Veiculo veiculo);
        
    }
}