using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Interfaces;
using dio_lab_trilha_net_minimal_api_desafio.Infraestrutura.Db;

namespace dio_lab_trilha_net_minimal_api_desafio.Dominio.Servicos
{
    public class VeiculosServico(DBContexto contexto) : IVeiculosServico
    {
        private readonly DBContexto _contexto = contexto;

        public void Apagar(Veiculo veiculo)
        {
            _contexto.Veiculos.Remove(veiculo);
            _contexto.SaveChanges();
        }

        public Veiculo Atualizar(Veiculo veiculo)
        {
            _contexto.Veiculos.Update(veiculo);
            _contexto.SaveChanges();
            return veiculo;
        }

        public Veiculo? BuscaPorId(int id)
        {
            return _contexto.Veiculos.Find(id);
        }

        public Veiculo Incluir(Veiculo veiculo)
        {
            _contexto.Veiculos.Add(veiculo);
            _contexto.SaveChanges();
            return veiculo;
        }

        public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
        {
            int resultadosPorPagina = 10;
            int pag = pagina is null? 1 : (int)pagina;

            IQueryable<Veiculo> query = _contexto.Veiculos.AsQueryable();
            if(nome is not null)
                query = query.Where(v => v.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
            if(marca is not null)
                query = query.Where(v => v.Marca.Contains(marca, StringComparison.OrdinalIgnoreCase));
            
            return [.. _contexto.Veiculos.Skip((pag - 1)*resultadosPorPagina)
                                         .Take(resultadosPorPagina)];
        }
    }
}