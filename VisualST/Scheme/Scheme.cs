using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace VisualST.Scheme
{
    static class Scheme
    {
        

        public static List<Action> actions = new List<Action> { };
        //public static int lastNumber { get; set; } = 0;

        public static string AtrInfo(string actName, string atrName)
        {
            foreach(Action act in actions)
            {
                if (act.name == actName) return act.getInfo(atrName);
            }
            return "";
        }
        
        public static List<string> getActionsNames()
        {
            List<string> lst = new List<string> { };
            if (actions == null) return null;
            foreach(Action act in actions)
            {
                lst.Add(act.name);
            }
            return lst;
        }
        public static List<string> getAtributesNames(string ActionName)
        {
            foreach (Action act in actions)
            {
                if (act.name == ActionName) return act.getAtrNames();
            }
            return null;
        }
        public static List<string> getExsistedAtributesNames(string ActionName)
        {
            foreach (Action act in actions)
            {
                if (act.name == ActionName) return act.getAtributesNames();
            }
            return null;
        }

        static public void NewAction()
        {
            //lastNumber++;
            Action act = new Action();
            actions.Add(act);

        }
        static public void DeleteAction(string Name)
        {
            if(actions == null) return;
            foreach (Action act in actions)
            {
                if (act.name == Name) { actions.Remove(act); return; }
            }
        }
        static public void RenameAction(string oldName, string newName)
        {
            foreach (Action act in actions)
            {
                if (act.name == oldName) { act.name = newName; return; }
            }
        }



        static public void NewAtribute(string ActionName)
        {
            foreach (Action act in actions)
            {
                if (act.name == ActionName)
                {
                    act.NewAtribute();
                }
            }
        }
        static public void ClasifyAtribute(string ActionName, string AtributeName, string AtributeType)
        {
            foreach (Action act in actions)
            {
                if (act.name == ActionName)
                    act.ClasifyAtribute(AtributeName,AtributeType);
            }
        }
        static public void DeleteAtribute(string ActionName, string AtributeName)
        {
            foreach (Action act in actions)
            {
                if (act.name == ActionName)
                {
                    act.DeleteAtribute(AtributeName);
                }
            }
        }
        static public void RenameAtribute(string ActionName, string oldName, string newName)
        {
            foreach (Action act in actions)
            {
                if (act.name == ActionName)
                {
                    act.RenameAtribute(oldName,newName);
                }
            }
        }


        static public void Save(string path)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
            sw.WriteLine("._.Actions._.");
            foreach(Action act in actions)
            {
                act.SaveAction(sw);
                
            }
            sw.WriteLine("._.Atributes._.");
            foreach (Action act in actions)
            {
                act.SaveAtributes(sw);
            }
            sw.Close();
        }
        static public void Load(string path)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(path);
            actions.Clear();
            string answerLine = sr.ReadLine();
            
            if (answerLine != "._.Actions._.")
                return;
            answerLine = sr.ReadLine();
            while (answerLine != "._.Atributes._.")
            {
                
                Action act = new Action(answerLine);
                actions.Add(act);
                answerLine = sr.ReadLine();
            }


            if (answerLine != "._.Atributes._.")
            {
                actions.Clear();
                return;
            }
            while (!sr.EndOfStream)
            {
                answerLine = sr.ReadLine();
                foreach (Action act in actions)
                {
                    while(true)
                    {
                        string[] answerElement;
                        string[] separators = { ";;" };

                        answerLine = answerLine.Remove(0, 3);
                        answerLine = answerLine.Remove(answerLine.Length - 3, 3);
                        answerElement = answerLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        if (answerElement[0] != act.name) break;

                        act.LoadAtributes(answerElement);
                    }
                }
            }
             sr.Close();
        }
    }
}
