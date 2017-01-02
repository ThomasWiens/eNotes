using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace eNotes.model
{
    /// <summary>
    /// Stellt die Konfiguration für jede einzelne Notiz dar.
    /// </summary>
    public class Config : INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChangedEventHandler auf dem Sich Registriert werden kann um Lokale Änderungen zu erfahren.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// PropertyChangedEventHandler auf dem Sich Registriert werden kann um Globale Änderungen zu erfahren.
        /// </summary>
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        private static bool _showFirstCreation { get; set; }
        private FontFamily _font { get; set; }
        private int _fontSize { get; set; }
        private EnumColor _enColor { get; set; }
        private EnumClose _enClose { get; set; }

        /// <summary>
        /// Setzt oder Holt die X-Koordinate der Notiz
        /// </summary>
        public int LocationX { get; set; }

        /// <summary>
        /// Setzt oder Holt die Y-Koordinate der Notiz
        /// </summary>
        public int LocationY { get; set; }

        /// <summary>
        /// Setzt oder Holt die Breite der Notiz
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Setzt oder Holt die Höhe der Notiz
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Setzt oder Holt das Flag für die Datumsanzeige
        /// Globale Registrenten werden mit 'showFirstCreation' informiert.
        /// </summary>
        public static bool ShowFirstCreation
        {
            get
            {
                return _showFirstCreation;
            }
            set
            {
                _showFirstCreation = value;
                OnStaticPropertyChanged(null, "ShowFirstCreation");
            }
        }

        /// <summary>
        /// Setzt oder Holt die FontFamily
        /// Globale Registrenten werden mit 'FontFamily' informiert.
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return _font;
            }
            set
            {
                _font = value;
                OnPropertyChanged("FontFamily");
            }
        }

        /// <summary>
        /// Setzt oder Holt die FontSize
        /// Globale Registrenten werden mit 'FontSize' informiert.
        /// </summary>
        public int FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        /// <summary>
        /// Setzt oder Holt das EnumColor
        /// Globale Registrenten werden mit 'EnumColor' informiert.
        /// </summary>
        public EnumColor EnumColor
        {
            get
            {
                return _enColor;
            }
            set
            {
                _enColor = value;
                OnPropertyChanged("EnumColor");
            }
        }

        /// <summary>
        /// Setzt oder Holt das EnumClose
        /// Globale Registrenten werden mit 'EnumClose' informiert.
        /// </summary>
        public EnumClose EnumClose
        {
            get
            {
                return _enClose;
            }
            set
            {
                _enClose = value;
                OnPropertyChanged("EnumClose");
            }
        }

        /// <summary>
        /// Erstellt eine Kopie des Objektes
        /// </summary>
        /// <returns>clone</returns>
        public Config clone()
        {
            Config conf = new Config();
            conf.FontFamily = this.FontFamily;
            conf.FontSize = this.FontSize;
            conf.EnumColor = this.EnumColor;
            conf.EnumClose = this.EnumClose;
            return conf;
        }

        private static void OnStaticPropertyChanged(Object sender, String PropertyName)
        {
            if (StaticPropertyChanged != null)
                StaticPropertyChanged(sender, new PropertyChangedEventArgs(PropertyName));
        }

        private void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
