using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualST.Scheme.Atributes.SimpleAtributes
{
    class Number : Atribute
    {
        public double value;


        public Number()
        {

        }
        public Number(string[] str)
        {
            name = str[2];
            universal = str[3]=="False"? false : true;
        }

        override public string Get()
        {
            return value.ToString("F5");
        }
        override public void Set(string v)
        {
            value = Convert.ToDouble(v); //добавь проверку

        }

       override public void SaveAtributes(System.IO.StreamWriter sw,string hoster)
        {
            sw.WriteLine("._." +hoster+";;"+"simple-number"+";;"+name+";;"+ universal+"._.");
        }

        public override string Info()
        {
            return "simple-number";
        }
    }
}
