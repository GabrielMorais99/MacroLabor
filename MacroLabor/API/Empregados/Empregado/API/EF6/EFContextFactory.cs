using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF6
{
    class EFContextFactory : IDbContextFactory<EFContext>
    {
        public EFContext Create()
        {
            return new EF6.EFContext("Server=(localdb)\\mssqllocaldb;Database=EF6MVCCore;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
