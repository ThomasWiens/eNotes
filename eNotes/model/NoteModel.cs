using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotes.model
{
    /// <summary>
    /// Gespeichertes Objekt der Notizen. Befasst sich nur mit der Datenhaltung von Notizen
    /// </summary>
    public class NoteModel : INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChangedEventHandler auf dem Sich Registriert werden kann um Änderungen zu erfahren.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private PropertyChangedEventHandler handler;
        /// <summary>
        /// Setzt oder Holt die Liste von Timestamps
        /// </summary>
        public List<DateTime> timestamps { get; set; }
        private DateTime fallbackTime;
        private Config _config;
        private DateTime _creation;
        private DateTime _lastChange;
        private String _text;

        /// <summary>
        /// Konstruktor: initialisiert Attribute des Objekts
        /// </summary>
        public NoteModel()
        {
            fallbackTime = new DateTime();
            _creation = fallbackTime;
            _lastChange = fallbackTime;
            timestamps = new List<DateTime>();
            handler = new PropertyChangedEventHandler(OnPropertyChanged);
        }

        /// <summary>
        /// Setzt oder Holt den Text
        /// </summary>
        public String Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged("Text");
            } 
        }

        /// <summary>
        /// Holt das Erstellungsdatum
        /// </summary>
        public DateTime Creation
        {
            get
            {
                if (_creation == fallbackTime)
                {
                    _creation = timestamps[0];
                    _lastChange = timestamps[timestamps.Count - 1];
                }
                return _creation;
            }
        }

        /// <summary>
        /// Setzt oder Holt das Config-Objekt zu der Notiz
        /// </summary>
        public Config Config
        {
            get
            {
                return _config;
            }
            set
            {
                if (_config != null)
                {
                    _config.PropertyChanged -= handler;
                }
                _config = value;
                _config.PropertyChanged += handler;
            }
        }

        /// <summary>
        /// Holt die Letzte Änderung Datum
        /// </summary>
        public DateTime LastChange
        {
            get
            {
                if (_lastChange == fallbackTime)
                {
                    _creation = timestamps[0];
                    _lastChange = timestamps[timestamps.Count - 1];
                }
                return _lastChange;
            }
        }

        /// <summary>
        /// Fügt einen neuen Zeitstempel hinzu. Die Liste von Timestamps wird neu sortiert.
        /// Registrenten erfahren die Änderung mit dem Wert 'Time'
        /// </summary>
        /// <param name="timestamp">DateTime, der hinzugefügt werden soll</param>
        public void addTimestamp(DateTime timestamp)
        {
            timestamps.Add(timestamp);

            _lastChange = fallbackTime;
            _creation = fallbackTime;
            timestamps.Sort();
            OnPropertyChanged("Time");
        }

        /// <summary>
        /// Löscht den jüngsten Zeitstempel
        /// Registrenten erfahren die Änderung mit dem Wert 'Time'
        /// </summary>
        public void removeLastTimestamp()
        {
            timestamps.RemoveAt(timestamps.Count - 1);
            _lastChange = fallbackTime;
            _creation = fallbackTime;
            timestamps.Sort();
            OnPropertyChanged("Time");
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        private void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                OnPropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}
