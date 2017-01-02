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
using eNotes.model;
using System.ComponentModel;

namespace eNotes.view
{
    /// <summary>
    /// Interaktionslogik für NotePreview.xaml
    /// </summary>
    public partial class NotePreview : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private NoteModel _noteModel;
        private string dateFormat;
        private bool _isSelected;
        private PropertyChangedEventHandler handler;

        /// <summary>
        /// Konstruktor: initialisert Werte
        /// </summary>
        public NotePreview()
        {
            InitializeComponent();
            check.DataContext = this;
            this.dateFormat = "dd MM yy";
            handler = new PropertyChangedEventHandler(update);
        }

        /// <summary>
        /// Setzt oder Holt das NoteModel. Die Werte werden anschließend an die Oberfläche delegiert.
        /// </summary>
        public NoteModel NoteModel
        {
            get
            {
                return _noteModel;
            }
            set
            {
                if (_noteModel != null)
                {
                    _noteModel.PropertyChanged -= handler;
                }
                _noteModel = value;
                txtBlock.DataContext = _noteModel;
                update(this, new PropertyChangedEventArgs("All"));
                _noteModel.PropertyChanged += handler;
            }
        }

        private void update(Object sender, PropertyChangedEventArgs e)
        {
            //Text wird durch Databinding geloest
            if (e.PropertyName == "FontFamily" || e.PropertyName == "All")
            {
                txtBlock.FontFamily = _noteModel.Config.FontFamily;
            }
            if (e.PropertyName == "EnumColor" || e.PropertyName == "All")
            {
                setBackgroundColor(_noteModel.Config.EnumColor);
            }
            if (e.PropertyName == "Time" || e.PropertyName == "All")
            {
                txtbdateFirst.Text = _noteModel.Creation.ToString(dateFormat) + " - " + _noteModel.LastChange.ToString(dateFormat);
            }
        }

        /// <summary>
        /// Setzt oder Holt das Flag, ob die View enabled ist
        /// </summary>
        public bool IsSelected
        {
            get
            { 
                return _isSelected;
            } 
            set 
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        private void setBackgroundColor(EnumColor enColor)
        {
            Color gradient1 = (Color)ColorConverter.ConvertFromString(enColor.Gradient1);
            Color gradient2 = (Color)ColorConverter.ConvertFromString(enColor.Gradient2);
            LinearGradientBrush lgb = new LinearGradientBrush(gradient1, gradient2, new Point(0, 0), new Point(1, 1));
            grid.Background = lgb;
        }

        private void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
