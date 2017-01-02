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
using eNotes.model;

namespace eNotes.view
{
    /// <summary>
    /// Interaktionslogik für NoteConfig.xaml
    /// </summary>
    public partial class NoteConfig : Window
    {
        /// <summary>
        /// Setzt die Config, die an das verwendete Einstellungs UserControl delegiert wird.
        /// </summary>
        public Config Config
        {
            set
            {
                (cont as Einstellung).setConfigToBind(value);
            }
        } 

        /// <summary>
        /// Konstruktor: initialisert die Werte
        /// </summary>
        public NoteConfig()
        {
            InitializeComponent();
            (cont as Einstellung).enableGlobalConfig(false);
            (cont as Einstellung).WithLokalInfo = false;
        }
    }
}
