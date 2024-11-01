using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.ModelViews
{
    public struct ErrosDeValidacao
    {
        public List<string> Mensagens {get;}
        
        public ErrosDeValidacao()
        {
            Mensagens = [];
        }
    }
}