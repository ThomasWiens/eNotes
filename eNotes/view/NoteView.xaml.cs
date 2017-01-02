using eNotes;
using eNotes.model;
using eNotes.logik;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace eNotes.view
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class NoteView : Window
    {
        private string oldText;
        private bool textChanged;
        private NoteModel _noteM;
        private string dateFormat;
        private PropertyChangedEventHandler handler;

        /// <summary>
        /// Konstruktor: initialisiert die Werte
        /// </summary>
        public NoteView()
        {
            InitializeComponent();

            this.oldText = "";
            this.textChanged = false;
            this.handler = new PropertyChangedEventHandler(update);
            this.setBackgroundColor(EnumColor.GELB);
            this.dateFormat = "dd MMMM yyyy";

            Config.StaticPropertyChanged += handler; // Ueber globale Einstellungen Informieren lassen
        }

        /// <summary>
        /// Setzt die Config, die die Oberfläche konfigurieren soll.
        /// </summary>
        /// <param name="conf"></param>
        public void setConfigValues(Config conf)
        {
            if (conf != null)
            {
                if(conf.LocationX != 0)
                    this.Left = conf.LocationX;
                if(conf.LocationY != 0)
                    this.Top = conf.LocationY;
                if(conf.Width > this.MinWidth)
                    this.Width = conf.Width;
                if(conf.Height > this.MinHeight)
                    this.Height = conf.Height;

                this.setEnumClose(conf.EnumClose);
                this.setBackgroundColor(conf.EnumColor);
                txtbtext.FontFamily = conf.FontFamily;
                txtbtext.FontSize = conf.FontSize;
            }
        }

        /// <summary>
        /// Setzt die Hintergrundfarbe mittels des übergebenen EnumColor
        /// </summary>
        /// <param name="enColor"></param>
        public void setBackgroundColor(EnumColor enColor)
        {
            Color gradient1 = (Color)ColorConverter.ConvertFromString(enColor.Gradient1);
            Color gradient2 = (Color)ColorConverter.ConvertFromString(enColor.Gradient2);
            window.Background = new LinearGradientBrush(gradient1, gradient2, new Point(0,0), new Point(1,1));
        }

        private void onClose(object sender, RoutedEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {    
                NoteManager.getInstance().closeAllNotes();   
            }
            else
            {
                NoteManager.getInstance().closeNote(this);
            }
        }

        private void onCloseChoice(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;
        }

        private void onNewNote(object sender, RoutedEventArgs e)
        {
            NoteManager.getInstance().newNote();
        }

        private void onShowConfig(object sender, RoutedEventArgs e)
        {
            NoteManager.getInstance().showConfigCenter(this);
        }

        private void moveWindow(Object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        /// <summary>
        /// Setzt oder Holt das verwendete NoteModel.
        /// </summary>
        public NoteModel NoteModel
        {
            get
            {
                return _noteM;
            }
            set
            {
                if (_noteM != null)
                {
                    _noteM.PropertyChanged -= handler;
                }
                _noteM = value;
                setNoteModelToBind(_noteM);
                _noteM.PropertyChanged += handler;
            }
        }

        private void setNoteModelToBind(NoteModel noteM)
        {
            txtbtext.DataContext = noteM;
            oldText = noteM.Text;

            update(this, new PropertyChangedEventArgs("All"));
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string name = (sender as MenuItem).Name;
            if (String.Compare(name, mniSave.Name) == 0)
            {
                _noteM.Config.EnumClose = EnumClose.SAVE;
            }
            else if (String.Compare(name, mniArc.Name) == 0)
            {
                _noteM.Config.EnumClose = EnumClose.ARCHIVE;
            }
            else if (String.Compare(name, mniDel.Name) == 0)
            {
                _noteM.Config.EnumClose = EnumClose.DELETE;                
            }
        }

        private void setEnumClose(EnumClose enClose)
        {
            if (enClose == EnumClose.SAVE)
            {
                MenuItem_Click(mniSave, null);
            }
            else
            {
                MenuItem_Click(mniArc, null);
            }
        }

        private void update(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text" || e.PropertyName == "All")
            {
                if (_noteM.Text == oldText)// Text ist wie Ursprung
                {
                    if (textChanged == true)// Aber schonmal geaendert worden
                    {
                        textChanged = false;// Sonst durch Observable-Pattern endlos
                        _noteM.removeLastTimestamp();
                    }
                    textChanged = false;
                }
                else // neuer Text
                {
                    if (textChanged == false)//erste Aenderung
                    {
                        textChanged = true;
                        _noteM.addTimestamp(DateTime.Now);
                    }
                    textChanged = true;
                }
            }
            if (e.PropertyName == "FontFamily" || e.PropertyName == "All")
            {
                txtbtext.FontFamily = _noteM.Config.FontFamily;
            }
            if (e.PropertyName == "FontSize" || e.PropertyName == "All")
            {
                txtbtext.FontSize = _noteM.Config.FontSize;
            }
            if (e.PropertyName == "EnumColor" || e.PropertyName == "All")
            {
                setBackgroundColor(_noteM.Config.EnumColor);
            }
            if (e.PropertyName == "EnumClose" || e.PropertyName == "All")
            {
                if (_noteM.Config.EnumClose == EnumClose.SAVE)
                {
                    mniSave.IsChecked = true;
                    mniArc.IsChecked = false;
                    mniDel.IsChecked = false;
                }
                else if (_noteM.Config.EnumClose == EnumClose.ARCHIVE)
                {
                    mniSave.IsChecked = false;
                    mniArc.IsChecked = true;
                    mniDel.IsChecked = false;
                }
                else
                {
                    mniSave.IsChecked = false;
                    mniArc.IsChecked = false;
                    mniDel.IsChecked = true;
                }
            }

            if (Config.ShowFirstCreation)
            {
                txtbdate.Text = _noteM.Creation.ToString(dateFormat);
                txtbdate.ToolTip = "Erstellung am:\n" + txtbdate.Text;
            }
            else
            {
                txtbdate.Text = _noteM.LastChange.ToString(dateFormat);
                txtbdate.ToolTip = "Letzte Änderung am:\n" + txtbdate.Text;
            }
        }

        private void mniConfig_Click(object sender, RoutedEventArgs e)
        {
            NoteConfig nc = new NoteConfig();
            nc.Config = _noteM.Config;
            nc.Owner = this;
            nc.ShowDialog();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            _noteM.Config.LocationX = Convert.ToInt16(this.Left);
            _noteM.Config.LocationY = Convert.ToInt16(this.Top);
            _noteM.Config.Width = Convert.ToInt16(this.Width);
            _noteM.Config.Height = Convert.ToInt16(this.Height);
        }
    }
}