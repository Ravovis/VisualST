using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualST.Experiments
{
    class Action
    {
        public string name;
        Dictionary<string, string> table = new Dictionary<string, string>();
        public void Add(string _name, List<string> _atributes, List<string> _values)
        {
            name = _name;
            var A_and_V = _atributes.Zip(_values, (a, v) => new { Atribute = a, Value = v });
            foreach (var av in A_and_V)
            {
                table.Add(av.Atribute, av.Value);
            }
        }
    }
}
