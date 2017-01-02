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
using System.Windows.Shapes;
using eNotes.logik;

namespace eNotes.view
{
    /// <summary>
    /// Interaktionslogik für ConfigCenter.xaml
    /// </summary>
    public partial class ConfigCenter : Window
    {
        public ConfigCenter()
        {
            InitializeComponent();
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NoteManager.getInstance().closeConfigCenter();
        }
    }

    
}
