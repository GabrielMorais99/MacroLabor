using EF6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class EFController
    {
        private readonly EFContext _context;

        public EFController(EFContext context)
        {
            _context = context;
        }
    }
}