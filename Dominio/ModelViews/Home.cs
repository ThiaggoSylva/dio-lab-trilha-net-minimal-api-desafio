using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.ModelViews
{
    public struct Home
    {
        public readonly string Mensagem {get => "Bem vindo à API de veículos - Minimal API";}
        public readonly string Doc {get => "/swagger";}
    }
}