using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualST.Scheme
{
    class Action
    {
        public string name { set; get; }
        List<Atribute> atributes;
        List<string> atrlist;
        List<int> abstractions;

        int last = 0;

        public List<string> getAtrNames()
        {
            return atrlist;
        }
        public List<string> getAtributesNames()
        {
            List<string> tmp = new List<string>();
            foreach (Atribute a in atributes)
            {
                tmp.Add(a.name);
            }
            return tmp;
        }
        public string getInfo(string name)
        {
            foreach(Atribute atr in atributes)
            {
                if (atr.name == name) return atr.Info();
            }
            return "";
        }

        public Action()
        {
            name = "Действие" + (Scheme.actions.Count+1);
            atributes = new List<Atribute> { };
            abstractions = new List<int> { };
            atrlist = new List<string> { };
            abstractions.Add(1);//потом добавить интерфейс для абстракций
        }
        public Action(string answerLine)
        {
            string[] answerElement;
            string[] separators = { ";;" };

            answerLine = answerLine.Remove(0, 3);
            answerLine = answerLine.Remove(answerLine.Length - 3, 3);
            answerElement = answerLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            name = answerElement[0];

            atributes = new List<Atribute> { };
            abstractions = new List<int> { };
            atrlist = new List<string> { };
            abstractions.Add(1);//потом добавить интерфейс для абстракций
        }

        public void ClasifyAtribute(string name, string type)
        {
            //добавить проверку на существующий уже элемент
            foreach(Atribute atribute in atributes)
            {
                if(atribute.name==name)
                {
                    atributes.Remove(atribute);
                }
            }

            Atribute atr;
            switch (type)
            {
                case "simple-number": atr = new Atributes.SimpleAtributes.Number(); break;
                default: atr = null; break;
            }
            atr.name = name;
            atributes.Add(atr);

        }
        public void NewAtribute()
        {
            last++;
            string str = "Атрибут " + last;
            atrlist.Add(str);

        }
        public void DeleteAtribute(string Name)
        {
            if(atributes!=null)
                foreach (Atribute atr in atributes)
                {
                    if (atr.name == Name) { atributes.Remove(atr); return; }
                }
            if(atrlist!=null)
                foreach (string str in atrlist)
                {
                    if (str == Name) { atrlist.Remove(str); return; }
                }
        }
        public void RenameAtribute(string oldName, string newName)
        {
            for (int i = 0; i < atrlist.Count; i++)
            {
                if (atrlist[i] == oldName)
                {
                    atrlist[i] = newName;
                    break;
                }
            }
            foreach (Atribute atr in atributes)
            {
                if (atr.name == oldName) { atr.name = newName; return; }
            }
        }

        public void SaveAction(System.IO.StreamWriter sw)
        {
            sw.WriteLine("._."+name+"._.");
        }
        public void SaveAtributes(System.IO.StreamWriter sw)
        {
            foreach(Atribute atr in atributes)
            {
                atr.SaveAtributes(sw,name);
            }
        }

        public void LoadAtributes(string[] str)
        {
            Atribute atr;
            string type = str[1];

            switch (type)
            {
                case "simple-number": atr = new Atributes.SimpleAtributes.Number(str); break;
                default: atr = null; break;
            }
            atributes.Add(atr);
            atrlist.Add(atr.name);
        }

    }
}
