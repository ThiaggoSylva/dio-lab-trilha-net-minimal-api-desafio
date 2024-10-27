using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dio_lab_trilha_net_minimal_api_desafio.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace dio_lab_trilha_net_minimal_api_desafio.Infraestrutura.Db
{
    public class DBContexto(IConfiguration configuracaoAppSettings) : DbContext
    {
        private readonly IConfiguration _configuracaoAppSettings = configuracaoAppSettings;

        public DbSet<Administrador> Administradores {get; set;} = default!;
        public DbSet<Veiculo> Veiculos {get; set;} = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>().HasData(
                new Administrador{
                    Id = 1,
                    Email = "administrador@teste.com",
                    Senha = "123456",
                    Perfil = "Admn"
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                var stringConexao = _configuracaoAppSettings.GetConnectionString("mysql")?.ToString();
                if(!string.IsNullOrEmpty(stringConexao))
                optionsBuilder.UseMySql(
                    stringConexao,
                    ServerVersion.AutoDetect(stringConexao)
                    );
            }
        }
    }
}