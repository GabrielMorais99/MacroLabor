using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gerenciador.Dal
{
    public partial class GerenciadorContexto : DbContext
    {
        public GerenciadorContexto(DbContextOptions<GerenciadorContexto> dbContextOptions)
            : base(dbContextOptions)
        {
        }
        public GerenciadorContexto()
           : base()
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }
        public virtual DbSet<TCliente> TCliente { get; set; }
        public virtual DbSet<THistoricoTransferencia> THistoricoTransferencia { get; set; }
        public virtual DbSet<TIndice> TIndice { get; set; }
        public virtual DbSet<TRepositorio> TRepositorio { get; set; }
        public virtual DbSet<TRepositSistemaCliente> TRepositSistemaCliente { get; set; }
        public virtual DbSet<TSistema> TSistema { get; set; }
        public virtual DbSet<TSistemaCliente> TSistemaCliente { get; set; }

    }
}
