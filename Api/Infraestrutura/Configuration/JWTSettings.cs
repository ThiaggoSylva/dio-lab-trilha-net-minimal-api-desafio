using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dio_lab_trilha_net_minimal_api_desafio.Infraestrutura.Configuration
{
    public class JWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}