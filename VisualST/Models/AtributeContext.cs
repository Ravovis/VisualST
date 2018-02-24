using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace VisualST.Models
{
    class AtributeContext : DbContext
    {
        public AtributeContext()
            : base("DbConnection")
        { }

        public DbSet<Atribute> Atributes { get; set; }
    }
}
