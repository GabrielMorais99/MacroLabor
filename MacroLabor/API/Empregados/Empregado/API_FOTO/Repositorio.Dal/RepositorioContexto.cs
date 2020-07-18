using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Repositorio.Dal.Model;

namespace Repositorio.Dal
{
    public class RepositorioContexto : DbContext
    {

        public RepositorioContexto(DbContextOptions<RepositorioContexto> options)
          : base(options)
        {
        }
        public RepositorioContexto()
            : base()
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Empregado>()
                           .HasOne(a => a.EmpregadoEndereco)
                           .WithOne()
                           .HasForeignKey<EnderecoEmpregado>(e => e.IdEmpregado);

            builder.Entity<Empregado>().ToTable("TEmpregado");
            builder.Entity<EnderecoEmpregado>().ToTable("TGEG_EmpregadoEndereco");
            

            base.OnModelCreating(builder);


        }

        public virtual DbSet<Arquivo> Arquivo { get; set; }
        public virtual DbSet<ArquivoTemporario> ArquivoTemporario { get; set; }
        public virtual DbSet<Repositorio> Repositorio { get; set; }
        public virtual DbSet<Sistema> Sistema { get; set; }
    }
}
