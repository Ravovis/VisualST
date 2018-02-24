using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace VisualST.Models
{
    class ActionContext : DbContext
    {
        public ActionContext()
            : base("DbConnection")
        { }

        public DbSet<Action> Actions { get; set; }
    }
}
