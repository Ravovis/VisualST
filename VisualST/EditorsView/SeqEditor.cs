using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Microsoft.Win32;

namespace VisualST.EditorsView
{
    class MyTable
    {
        public MyTable(string Atr_Name, string Atr_Value)
        {
            this.Atr_Name = Atr_Name;
            this.Atr_Value = Atr_Value;
        }
        public string Atr_Name { get; set; }
        public string Atr_Value { get; set; }
    }
    class SeqEditor
    {
        public static string ActiveAction = "";
        public static ListBox Act = new ListBox();
        public static Experiments.Experiment Experts = new Experiments.Experiment();

        public static void CreateNewSequationWindow()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            
            NewSequation ns = new NewSequation();

            List<string> elements = Scheme.Scheme.getActionsNames();
            foreach (string s in elements)
            {
                ComboBoxItem InnerItem = new ComboBoxItem();
                InnerItem.Content = s;
                ns.ChoseAction.Items.Add(InnerItem);
            }
            ns.Show();
            ns.Create.Click += (i, e) =>
            {
                if (ns.ChoseAction.Text == "")
                {
                }
                else
                {
                    ActiveAction = ns.ChoseAction.Text;
                    Act.Items.Add(ActiveAction);
                    List<string> at = Scheme.Scheme.getExsistedAtributesNames(ActiveAction);
                    List<string> val = new List<string>();
                    if (at != null)
                    {
                        foreach (string a in at)
                        {
                            val.Add("1");
                        }
                        Experts.Add(ActiveAction, at, val);
                    }      
                    ns.Close();
                }
            };


        }
        
        
        public static void DeleteSequationWindow()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            if (Act.SelectedItem != null)
            {
                string tmp = Act.SelectedItem.ToString();
                Act.SelectedItem = null;
                Act.Items.Remove(tmp);
                Experts.Remove(tmp);
            }
            else
            {
                List<string> elements = new List<string>();
                foreach (var item in Act.Items)
                {
                    elements.Add(item.ToString());
                }
                NewSequation ns = new NewSequation();
                foreach (string s in elements)
                {
                   
                    ns.ChoseAction.Items.Add(s);
                }
                ns.Create.Content = "Удалить";
                ns.Show();
                ns.Create.Click += (i, e) =>
                {
                    if (ns.ChoseAction.Text != "")
                    {
                        Act.Items.Remove(ns.ChoseAction.Text);
                        Experts.Remove(ns.ChoseAction.Text);
                        ns.Close();
                    }
                };
            }
        }


        public static void ActGenerate()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            WrapPanel ActionPanel = new WrapPanel();
            WrapPanel ButtomPanel = new WrapPanel();
            StackPanel ActionStackPanel = new StackPanel();
            Button NewAction = new Button();
            Button DeleteAction = new Button();
            Act = new ListBox();
            DataGrid Info = new DataGrid();


            if (Experts != null)
            {
                foreach (Experiments.Action exp in Experts.Act)
                {
                    Act.Items.Add(exp.name);
                }
            }


            NewAction.Content = "Новое действие";
            NewAction.Margin = new Thickness(10, 10, 10, 10);
            NewAction.Click += (i, e) =>
            {
                CreateNewSequationWindow();
            };
            ButtomPanel.Children.Add(NewAction);


            DeleteAction.Content = "Удалить действие";
            DeleteAction.Margin = new Thickness(10, 10, 10, 10);
            DeleteAction.Click += (i, e) =>
            {
                DeleteSequationWindow();
            };
            ButtomPanel.Children.Add(DeleteAction);


            Act.Height = 200;
            Act.Width = 220;
            ActionStackPanel.Children.Add(Act);
            Act.SelectionChanged += (i, e) =>
            {
                try
                {
                    Info.Items.Clear();
                }
                catch { }
                if (Act.SelectedItem != null)
                {
                    List<string> Atribures;
                    List<MyTable> Content = new List<MyTable>();
                    ActiveAction = Act.SelectedItem.ToString();
                    Atribures = Scheme.Scheme.getExsistedAtributesNames(ActiveAction);
                    Content = new List<MyTable>();
                    foreach (string a in Atribures)
                    {
                        Content.Add(new MyTable(a, "1"));
                    }
                    Info.ItemsSource = Content;
                    Info.Columns[0].Header = "Атрибут";
                    Info.Columns[0].Width = 200;
                    Info.Columns[1].Header = "Значение";
                    Info.Columns[1].Width = 200;
                }
                
            };
            
            
            ActionStackPanel.Children.Add(ButtomPanel);
            ActionPanel.Children.Add(ActionStackPanel);
            Info.Height = 240;
            Info.Width = 440;
            ActionPanel.Children.Add(Info);

            mainWin.Editor.Children.Add(ActionPanel);

            mainWin.SaveProject.Click += (i, e) =>
            {
                List<string> names = new List<string>();
                
            };
        }
    }
}
