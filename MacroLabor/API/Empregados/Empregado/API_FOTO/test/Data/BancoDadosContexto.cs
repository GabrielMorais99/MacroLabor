
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using test.Data;
using System.Collections.Generic; 



namespace test.Data
{
    public class BancoDadosContexto
    {
    public BancoDadosContexto() { }

public BancoDadosContexto(  DbContextOptions<BancoDadosContexto> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Empregado>().HasKey(p => p.CPF);
            //builder.Entity<EnderecoEmpregado>().HasKey(p => p.empregado);

            
            builder.Entity<Empregado>()
                .HasOne(a => a.EmpregadoEndereco)
                .WithOne()
                .HasForeignKey<EnderecoEmpregado>(e => e.IdEmpregado);

            builder.Entity<Empregado>().ToTable("TEmpregado");
            builder.Entity<EnderecoEmpregado>().ToTable("TGEG_EmpregadoEndereco");
            //builder.Entity<Empregado>().ToTable("TEmpregado");
            //builder.Entity<EnderecoEmpregado>().ToTable("TEmpregado");

            base.OnModelCreating(builder);
        }

        public  DbSet<Empregado> Empregados { get; set; }

        public DbSet<EnderecoEmpregado> EnderecoEmpregado { get; set; }
    }










    }
}