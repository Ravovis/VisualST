using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualST.Scheme
{
    abstract class Atribute
    {
        public string name { get; set; }
        protected bool universal;
        abstract public string Get();
        abstract public void Set(string v);

        abstract public void SaveAtributes(System.IO.StreamWriter sw, string hoster);
        abstract public string Info();
    }
}
