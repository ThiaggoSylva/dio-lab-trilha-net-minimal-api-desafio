using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.DTOs
{
    public record VeiculoDTO
    {
        public string Nome { get; set; }
        public string Marca { get; set; }
        public int Ano { get; set; }
    }
}