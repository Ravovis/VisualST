using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualST.Experiments
{
    class Experiment
    {
        public List<Action> Act = new List<Action>();
        public void Add(string _name, List<string> _atributes, List<string> _values)
        {
            Action A = new Action();
            A.Add(_name, _atributes, _values);
            Act.Add(A);
        }
        public void Remove(string _name)
        {
            Action tmp = new Action();
            foreach (Action a in Act)
            {
                if (a.name == _name)
                {
                    tmp = a;
                }
            }
            Act.Remove(tmp);
        }
    }
}
