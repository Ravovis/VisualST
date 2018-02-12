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
    public static class AtrEditor
    {

        public static int ActionActive = 0;
        public static Dictionary<string, string> activeAtribute = new Dictionary<string, string>();

        public static void CreateRenameWindow(string panel,string oldName)
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            string ActionName = ((ComboBox)mainWin.FindName("ActionsBox")).SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last(); ;

            RenameWindow rw= new RenameWindow();

            rw.RenameBox.Text = oldName; 
            rw.Show();
            rw.RenameButton.Click+=(i, e) =>
            {
                string newName = rw.RenameBox.Text;
                switch(panel)
                {
                    case "ActionsBox":
                        Scheme.Scheme.RenameAction(oldName, newName);
                        break;
                    case "AtributesBox":
                        Scheme.Scheme.RenameAtribute(ActionName,oldName, newName);
                        break;
                    default:
                        break;
                }
                mainWin.IsEnabled = true;
                rw.Close();
            };


        }

        
        public static void ActGenerate()
        {
            EditClear();

            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            WrapPanel ActionsPanel = new WrapPanel();

            Button NewAction = new Button();
            Button DeleteAction = new Button();
            ComboBox ActionsBox = new ComboBox();
            Button ChangeActionName = new Button();


            NewAction.Content = "Новое действие";
            NewAction.Margin = new Thickness(10, 10, 10, 10);
            NewAction.Click += (i, e) =>
              {
                  Scheme.Scheme.NewAction();
                  
                  ActGenerate();//для производительности надо будет заменить
              };
            ActionsPanel.Children.Add(NewAction);

            
            DeleteAction.Content = "Удалить действие";
            DeleteAction.Margin = new Thickness(10, 10, 10, 10);
            DeleteAction.Click += (i, e) =>
              {
                  string name = (ActionsBox.SelectedItem == null) ? null : ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                  Scheme.Scheme.DeleteAction(name);
                  ActGenerate();
              };
            ActionsPanel.Children.Add(DeleteAction);

            
            ActionsBox.MinWidth = 100;
            ActionsBox.Margin = new Thickness(10, 10, 10, 10);
            List<string> elements = Scheme.Scheme.getActionsNames();
            if(elements!=null)
                foreach(string s in elements)
                {
                    ComboBoxItem InnerItem = new ComboBoxItem();
                    InnerItem.Content = s;
                    ActionsBox.Items.Add(InnerItem);
                }
            ActionsBox.SelectedIndex = ActionsBox.Items.Count - 1;
            ActionsBox.SelectionChanged += (sender, i) => {

                ActionActive = ActionsBox.SelectedIndex;
                ComboBox cmb = sender as ComboBox;
                string ActiveElement=cmb.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last().ToLower();
                //Убрать все остальные элементы
                if (ActiveElement != "") { AtrDelete(); AtrGenerate(); }
            };
            ActionsPanel.Children.Add(ActionsBox);
            mainWin.RegisterName("ActionsBox", ActionsBox);
            
            ChangeActionName.Content = "Переименовать";
            ChangeActionName.Margin= new Thickness(10, 10, 10, 10);
            ChangeActionName.Click += (i, e) => 
            {
                string name = (ActionsBox.SelectedItem == null) ? null : ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                CreateRenameWindow("ActionsBox", name);
                mainWin.IsEnabled = false;
            };
            ActionsPanel.Children.Add(ChangeActionName);


            mainWin.Editor.Children.Add(ActionsPanel);

            if (ActionsBox.SelectedItem != null)
            {
                string ActivElement = ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last().ToLower();
                if (ActivElement != "") { AtrDelete(); AtrGenerate(); }
            }

            SaveLoadGenerate(); //Обязательный


        }
        public static void AtrGenerate()
        {


            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            ComboBox ActionsBox = (ComboBox)mainWin.FindName("ActionsBox");
            WrapPanel AtributesPanel = new WrapPanel();

            Button NewAtribute = new Button();
            Button DeleteAtribute = new Button();
            ComboBox AtributesBox = new ComboBox();
            Button ChangeAtrName = new Button();

            string ActiveAction = ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();

            NewAtribute.Content = "Новый атрибут";
            NewAtribute.Margin = new Thickness(10, 10, 10, 10);
            NewAtribute.Click += (i, e) =>
            {
                ActionsBox = (ComboBox)mainWin.FindName("ActionsBox");
                ActiveAction = ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                Scheme.Scheme.NewAtribute(ActiveAction);
                activeAtribute[ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last()]= "";
                
                AtrDelete();
                AtrGenerate();
            };
            AtributesPanel.Children.Add(NewAtribute);

            
            DeleteAtribute.Content = "Удалить атрибут";
            DeleteAtribute.Margin = new Thickness(10, 10, 10, 10);
            DeleteAtribute.Click += (i, e) =>
              {
                  ActiveAction = ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                  string ActiveAtribute = AtributesBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                  Scheme.Scheme.DeleteAtribute(ActiveAction,ActiveAtribute);
                  AtrDelete();
                  AtrGenerate();
              };
            AtributesPanel.Children.Add(DeleteAtribute);

           ///////////////////////////////////////////////////////////////
            AtributesBox.MinWidth = 100;
            AtributesBox.Margin = new Thickness(10, 10, 10, 10);
            List<string> elements = Scheme.Scheme.getAtributesNames(ActiveAction) ;
            if (elements != null)
                foreach (string s in elements)
                {
                    ComboBoxItem InnerItem = new ComboBoxItem();
                    InnerItem.Content = s;
                    AtributesBox.Items.Add(InnerItem);
                }
            AtributesBox.SelectionChanged += (sender, i) => {
                ComboBox cmb = sender as ComboBox;
                string ActiveElement = cmb.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last().ToLower();
                //Убрать все остальные элементы
                if (ActiveElement != "") { DescrDelete();  TTypeGenerate(); }
            };
   
             if ((activeAtribute.ContainsKey(ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())) &&
                (activeAtribute[ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last()] != ""))
            {
                int index = -1;
                foreach (var element in AtributesBox.Items)
                {
                    index++;
                    string test = element.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                    if (activeAtribute[ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last()] == element.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
                    {
                        AtributesBox.SelectedIndex = index; break;
                    }
                }
            }
            else
            {
                AtributesBox.SelectedIndex = AtributesBox.Items.Count - 1;
             }
            ///////////////////////////////////////

            AtributesPanel.Children.Add(AtributesBox);
            try { mainWin.RegisterName("AtributesBox", AtributesBox); } catch (Exception e) { }


            ChangeAtrName.Content = "Переименовать";
            ChangeAtrName.Margin = new Thickness(10, 10, 10, 10);
            ChangeAtrName.Click += (i, e) =>
            {
                string name = (AtributesBox.SelectedItem == null) ? null : AtributesBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                CreateRenameWindow("AtributesBox", name);
                mainWin.IsEnabled = false;
            };
            AtributesPanel.Children.Add(ChangeAtrName);
            


            try { mainWin.RegisterName("AtributesPanel", AtributesPanel); } catch (Exception e) { }

            mainWin.Editor.Children.Add(AtributesPanel);


            if (AtributesBox.SelectedItem != null)
            {
                string ActivElement = AtributesBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                string ActivAction =  ActionsBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                if (ActivElement != "")
                {
                    activeAtribute[ActivAction] = ActivElement;
                    DescrDelete();
                    TTypeGenerate();
                }
                else
                {
                    activeAtribute[ActivAction] = null;
                }
            }

        }
        public static void TTypeGenerate()
        {
            
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

            WrapPanel DescrPanel = new WrapPanel();
            DescrPanel.Name = "DescrPanel";
            try { mainWin.RegisterName("DescrPanel", DescrPanel); } catch (Exception e) { }

            String[] ttypes = { "Простой", "Диапазон", "Ряд", "Смысловой" };
            //String[] types1 = {"Число", "Время", "Дата", "Строка", "Логический" };


            ComboBox TTypesBox = new ComboBox();
            TTypesBox.MinWidth = 100;
            TTypesBox.Margin = new Thickness(10, 10, 10, 10);
            for (int i = 0; i < ttypes.Length; i++)
            {
                ComboBoxItem InnerItem = new ComboBoxItem();
                InnerItem.Content = ttypes[i];
                TTypesBox.Items.Add(InnerItem);
            }
            TTypesBox.SelectionChanged += (sender, i) => {
                ComboBox cmb = sender as ComboBox;
                string ActiveElement = cmb.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last().ToLower();
                //Убрать все остальные элементы
                if (ActiveElement != "") {
                    TypeDelete();
                    TypeGenerate(); }
            };
            try { mainWin.RegisterName("TTypesBox", TTypesBox); } catch (Exception e) { }

            WrapPanel TTypePanel = new WrapPanel();
            try { mainWin.RegisterName("TTypePanel", TTypePanel); }catch(Exception e) { }
            TTypePanel.Children.Add(TTypesBox);
            DescrPanel.Children.Add(TTypePanel);

            mainWin.Editor.Children.Add(DescrPanel);
            
        }
        public static void TypeGenerate()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            WrapPanel DescrPanel = (WrapPanel) mainWin.FindName("DescrPanel");
            String[] types1 = { "Число", "Время", "Дата", "Строка", "Логический" };

          

            ComboBox TypesBox = new ComboBox();
            TypesBox.MinWidth = 100;
            TypesBox.Margin = new Thickness(10, 10, 10, 10);
            for (int i = 0; i < types1.Length; i++)
            {
                ComboBoxItem InnerItem = new ComboBoxItem();
                InnerItem.Content = types1[i];
                TypesBox.Items.Add(InnerItem);
            }
            TypesBox.SelectionChanged += (sender, i) => {
                string ActiveAction = ((ComboBox)mainWin.FindName("ActionsBox")).SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                string ActiveAtribute = ((ComboBox)mainWin.FindName("AtributesBox")).SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                ComboBox cmb = sender as ComboBox;
                ComboBox TTypeBox = (ComboBox)mainWin.FindName("TTypesBox");
                string type = "";
                 
                string ActiveElement = TTypeBox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                switch(ActiveElement)
                {
                    case "Простой": type += "simple";break;
                    default: break;
                }
                type += "-";
                ActiveElement = cmb.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last();
                switch (ActiveElement)
                {
                    case "Число": type += "number"; break;
                    default: break;
                }
                Scheme.Scheme.ClasifyAtribute(ActiveAction,ActiveAtribute, type);
                //Убрать все остальные элементы
                if (ActiveElement != "") { RestDelete(); RestGenerate(); }

                
            };

            WrapPanel TypePanel = new WrapPanel();
            try { mainWin.RegisterName("TypePanel", TypePanel); }catch(Exception e) { }
            TypePanel.Children.Add(TypesBox);

            DescrPanel.Children.Add(TypePanel);
            
            
           
        }
        public static void RestGenerate()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            WrapPanel DescrPanel = (WrapPanel)mainWin.FindName("DescrPanel");

            WrapPanel RestPanel = new WrapPanel();
            try { mainWin.RegisterName("RestPanel", RestPanel); } catch (Exception e) { }


            Label l1 = new Label();
            l1.Content = "Количество байт на число";
            l1.Margin = new Thickness(10, 10, 10, 10);
            RestPanel.Children.Add(l1);

            TextBox bytes = new TextBox();
            bytes.MinWidth = 100;
            bytes.Margin = new Thickness(10, 10, 10, 10);
            RestPanel.Children.Add(bytes);

            CheckBox universal = new CheckBox();
            universal.Content = "Универсальный";
            universal.Margin = new Thickness(10, 10, 10, 10);
            RestPanel.Children.Add(universal);

            DescrPanel.Children.Add(RestPanel);
        }
        public static void SaveLoadGenerate()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            WrapPanel SaveLoadPanel = new WrapPanel();
            SaveLoadPanel.HorizontalAlignment = HorizontalAlignment.Right;
            SaveLoadPanel.VerticalAlignment = VerticalAlignment.Bottom;

            Button Save = new Button();
            Save.Content = "Сохранить";
            Save.Margin = new Thickness(10, 10, 10, 10);
            Save.Click += (i, f) =>
              {
                  SaveFileDialog sd = new SaveFileDialog();
                  string path="";
                  if (sd.ShowDialog() == true)
                  {
                      path = sd.FileName;
                  }
                  Scheme.Scheme.Save(path);
              };
            SaveLoadPanel.Children.Add(Save);

            Button Load = new Button();
            Load.Content = "Загрузить";
            Load.Margin = new Thickness(10, 10, 10, 10);
            Load.Click += (i, f) =>
            {
                OpenFileDialog od = new OpenFileDialog();
                string path = "";
                if (od.ShowDialog() == true)
                {
                    path = od.FileName;
                }
                Scheme.Scheme.Load(path);
                EditClear();
                ActGenerate();
            };
            SaveLoadPanel.Children.Add(Load);

            try { mainWin.RegisterName("SaveLoadPanel", SaveLoadPanel); }catch(Exception e) { }

            mainWin.MainPanel.Children.Add(SaveLoadPanel);
        }



        public static void EditClear()
        {
            
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            mainWin.Editor.Children.Clear();

            try { mainWin.UnregisterName("DescrPanel"); } catch (Exception e) { };
            try { mainWin.UnregisterName("TypePanel"); } catch (Exception e) { };
            try { mainWin.UnregisterName("TTypesBox"); } catch (Exception e) { }
            try { mainWin.UnregisterName("TTypePanel"); } catch (Exception e) { };
            try { mainWin.UnregisterName("RestPanel"); } catch (Exception e) { };
            try { mainWin.UnregisterName("AtributesBox"); } catch (Exception e) { }
            try { mainWin.UnregisterName("AtributesPanel"); } catch (Exception e) { }
            try { mainWin.UnregisterName("ActionsBox"); } catch (Exception e) { }
            
            

            SaveLoadDelete();
        }

        public static void AtrDelete()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            try
            {
                
                WrapPanel AtrPanel = (WrapPanel)mainWin.FindName("AtributesPanel");
                mainWin.Editor.Children.Remove(AtrPanel);
            }
            catch (Exception) { }
            try { mainWin.UnregisterName("AtributesBox"); } catch (Exception e) { }
            try { mainWin.UnregisterName("AtributesPanel"); } catch (Exception e) { }

            DescrDelete();
        }
        public static void DescrDelete()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            try
            {
                
                WrapPanel DescrPanel = (WrapPanel)mainWin.FindName("DescrPanel");
                mainWin.Editor.Children.Remove(DescrPanel);
            }
            catch(Exception e) { }
            try { mainWin.UnregisterName("DescrPanel"); } catch (Exception e) { }
            try { mainWin.UnregisterName("TTypesBox"); } catch (Exception e) { }
            try { mainWin.UnregisterName("TTypePanel"); } catch (Exception e) { };
            try { mainWin.UnregisterName("TypePanel"); } catch (Exception e) { };
            try { mainWin.UnregisterName("RestPanel"); } catch (Exception e) { };

        }

        public static void TypeDelete()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            try
            {
                WrapPanel DescrPanel = (WrapPanel)mainWin.FindName("DescrPanel");

                WrapPanel TypePanel = (WrapPanel)mainWin.FindName("TypePanel");
                DescrPanel.Children.Remove(TypePanel);
            }
            catch(Exception e) { }
            try { mainWin.UnregisterName("TypePanel"); } catch (Exception e) { };

            RestDelete();
        }
        public static void RestDelete()
        {
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            try
            {
                WrapPanel DescrPanel = (WrapPanel)mainWin.FindName("DescrPanel");

                WrapPanel RestPanel = (WrapPanel)mainWin.FindName("RestPanel");
                DescrPanel.Children.Remove(RestPanel);
            }
            catch(Exception e) { }

            try { mainWin.UnregisterName("RestPanel"); } catch (Exception e) { };
        }

        public static void SaveLoadDelete()//вызывается из RecActDelete
        {
            
            var mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            try
            {
                
                mainWin.MainPanel.Children.Remove(mainWin.FindName("SaveLoadPanel") as UIElement);
            }
            catch(Exception e) { }
            try { mainWin.UnregisterName("SaveLoadPanel"); } catch (Exception e) { };
        }
    }
}
