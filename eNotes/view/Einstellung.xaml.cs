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
using System.Configuration;
using eNotes.model;
using eNotes.logik;
using System.Threading;

namespace eNotes.view
{
    /// <summary>
    /// Interaktionslogik für Einstellung.xaml
    /// </summary>
    public partial class Einstellung : UserControl
    {
        private Config conf;
        public bool WithLokalInfo { get; set; }

        /// <summary>
        /// Konstruktor: initialisert die Werte
        /// </summary>
        public Einstellung()
        {
            InitializeComponent();
            this.setConfigToBind(Storage.getConfig());
            WithLokalInfo = true;
        }

        /// <summary>
        /// Setzt das Config-Objekt auf/von dem gearbeitet werden soll.
        /// </summary>
        /// <param name="conf"></param>
        public void setConfigToBind(Config conf)
        {
            this.conf = conf;
            cmbFont.SelectedItem = conf.FontFamily;

            txtSize.Text = conf.FontSize.ToString();

            cmbFarbe.SelectedItem = conf.EnumColor.Name;
            if (Config.ShowFirstCreation)
            {
                radFirst.IsChecked = true;
            }
            else
            {
                radLast.IsChecked = true;
            }

            if (conf.EnumClose == EnumClose.SAVE)
            {
                radSav.IsChecked = true;

            }
            else if (conf.EnumClose == EnumClose.ARCHIVE)
            {
                radArch.IsChecked = true;

            }
            else
            {
                radDel.IsChecked = true;

            }

        }

        /// <summary>
        /// enabled oder disabled die Globale Konfigurationen
        /// </summary>
        /// <param name="b"></param>
        public void enableGlobalConfig(bool b)
        {
            grpGlobal.IsEnabled = b;
        }

        private void cmbFarbe_Initialized(object sender, EventArgs e)
        {
            IEnumerable<EnumColor> values = EnumColor.Values;
            foreach (EnumColor color in values)
            {
                (sender as ComboBox).Items.Add(color.Name);
            }
        }

        private void txtSize_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private bool IsTextAllowed(string p)
        {
            try
            {
                int i = Convert.ToInt16(txtSize.Text + p);
                if (i > 50 || i < 1)
                {
                    return false;
                }
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void onSave(object sender, RoutedEventArgs e)
        {
            // Erste oder Letzte Timestamp anzeigen
            if (radLast.IsChecked ?? false)
            {
                Config.ShowFirstCreation = false;
            }
            else
            {
                Config.ShowFirstCreation = true;
            }

            // Schliessverhalten
            if (radSav.IsChecked ?? false)
            {
                conf.EnumClose = EnumClose.SAVE;
            }
            else if (radArch.IsChecked ?? false)
            {
                conf.EnumClose = EnumClose.ARCHIVE;
            }
            else
            {
                conf.EnumClose = EnumClose.DELETE;
            }

            //FontFamily
            conf.FontFamily = cmbFont.SelectedItem as FontFamily;

            //FontSize
            conf.FontSize = Convert.ToInt16(txtSize.Text);

            //Farbe
            foreach(EnumColor enColor in EnumColor.Values){
                if(enColor.Name == (cmbFarbe.SelectedItem as String)){
                    conf.EnumColor = enColor;
                    break;
                }
            }

            Storage.storeConfig();
            Thread workerThread = new Thread(this.showSaved);
            workerThread.Start();
        }

        private void showInfo(object sender, MouseEventArgs e)
        {
            DependencyObject dpobj = sender as DependencyObject;
            string temp = dpobj.GetValue(FrameworkElement.NameProperty) as string;
            if (WithLokalInfo)
            {
                txtInfo.Text = ConfigurationManager.AppSettings["lokal"] + ConfigurationManager.AppSettings[temp];
            }
            else
            {
                txtInfo.Text = ConfigurationManager.AppSettings[temp];
            }
        }

        private void showGrpInfo(object sender, MouseEventArgs e)
        {
            DependencyObject dpobj = sender as DependencyObject;
            string temp = dpobj.GetValue(FrameworkElement.NameProperty) as string;
            txtInfo.Text = ConfigurationManager.AppSettings[temp];
        }

        public delegate void UpdateTextCallback(String message);
        private void UpdateText(string message)
        {
            lblChanges.Content = message;
        }

        private void showSaved()
        {
            lblChanges.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), "Gespeichert");
            Thread.Sleep(2000);
            lblChanges.Dispatcher.Invoke(new UpdateTextCallback(this.UpdateText), "");
        }
    }
}
