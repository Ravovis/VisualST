using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisualST
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            
            InitializeComponent();

        }
        //Events
        private void EditorChanged(object sender, SelectionChangedEventArgs e)
        {
            

            EditorsView.AtrEditor.EditClear();
            string choice = EditorCombobox.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last().ToLower();
            switch(choice)
            {
                case "опыты": EditorsView.SeqEditor.ActGenerate(); break;
                case "антология": EditorsView.AtrEditor.ActGenerate(); break;
                default: MessageBox.Show("Реакция еще не задана");break;
            }
             
        }

        private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            EditorsView.AtrEditor.EditClear();
            EditorsView.AtrEditor.ActGenerate();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DB.SQLActionRepository db = new DB.SQLActionRepository();
            db.Create(new Models.Action { Name = "Тестовое действие после закрытия программы" });
            db.Save();
        }
        //Help Functions


    }
}
