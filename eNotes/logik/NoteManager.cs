using eNotes.model;
using eNotes.view;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace eNotes.logik
{
    /// <summary>
    /// Manager des Hauptprogramms. Alle neue Notizen sollen über diese Klasse erstellt werden.
    /// Singleton: Holen der Instanz über getInstance()
    /// </summary>
    public class NoteManager : INotifyPropertyChanged
    {
        /// <summary>
        /// PropertyChangedEventHandler auf dem Sich Registriert werden kann um Änderungen zu erfahren.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private List<NoteModel> list;
        private List<NoteView> listNote;
        private static NoteManager instance;
        private ConfigCenter cCenter;

        private NoteManager()
        {
            this.list = new List<NoteModel>();
            this.listNote = new List<NoteView>();
        }

        /// <summary>
        /// Singleton: holen der Instanz dieser Klasse
        /// </summary>
        /// <returns>Einzige Instanz der Klasse</returns>
        public static NoteManager getInstance()
        {
            if (instance == null)
            {
                instance = new NoteManager();
            }
            return instance;
        }

        /// <summary>
        /// Erstellt eine neue Instanz und delegiert sie an addNote()
        /// </summary>
        public void newNote()
        {
            //Eine neue Notiz erstellen
            NoteModel nm = new NoteModel();
            nm.addTimestamp(DateTime.Now);
            nm.Config = Storage.getConfig().clone();

            addNote(nm, true);
        }

        /// <summary>
        /// fügt eine neue Notiz hinzu. Und zeigt diese je nach EnumClose Option auf der GUI an.
        /// Objekte die auf Änderungen warten werden mit 'List' benachrichtigt.
        /// </summary>
        /// <param name="nm">hinzuzufügende Notiz</param>
        public void addNote(NoteModel nm)
        {
            addNote(nm, true);
        }

        private void addNote(NoteModel nm, bool notify)
        {
            this.list.Add(nm);

            if (nm.Config.EnumClose == EnumClose.SAVE) // Diese Notiz auch anzeigen
            {
                showNote(nm);

            }
            else if (nm.Config.EnumClose == EnumClose.DELETE) //Falls eine 'geloeschte' Notiz hinzugefuegt wurde, die bereits gezeigt wird, schliessen wir diese
            {
                foreach (NoteView note in listNote)
                {
                    if (note.NoteModel == nm)
                    {
                        note.Close();
                        break;
                    }
                }
            }

            if (notify)
            {
                OnPropertyChanged("List");
            }
        }

        /// <summary>
        /// Eine Liste von Notizen kann hinzugefügt werden. Intern wird mittels foreach addNote() ausgeführt.
        /// Objekte die auf Änderungen warten werden 1Mal mit 'List' benachrichtigt.
        /// </summary>
        /// <param name="list">Liste mit NoteModel die hinzugefügt werden soll.</param>
        public void addAllNotes(List<NoteModel> list)
        {
            foreach (NoteModel elem in list)
            {
                addNote(elem, false);
            }
            OnPropertyChanged("List");
        }

        /// <summary>
        /// Löscht die Notiz und schließt sie, falls angezeigt.
        /// </summary>
        /// <param name="noteModel">das zu entfernende NoteModel</param>
        public void removeNote(NoteModel noteModel)
        {
            this.list.Remove(noteModel);

            foreach (NoteView n in listNote)
            {
                if (n.NoteModel == noteModel)
                {
                    n.Close();
                    break;
                }
            }

            OnPropertyChanged("List");
        }

        /// <summary>
        /// es werden alle NoteModels zurückgegeben.
        /// </summary>
        /// <returns>Liste mit NoteView</returns>
        public List<NoteModel> getAllNotes()
        {
            return new List<NoteModel>(list);
        }

        /// <summary>
        /// Es werden alle tatsächlich angezeigte Notizen zurückgegeben
        /// </summary>
        /// <returns>Liste mit NoteView</returns>
        public List<NoteView> getDisplayedNotes()
        {
            return listNote;
        }

        /// <summary>
        /// Über dieser Methode wird das ConfigCenter angezeigt. War vorher das Fenster nicht sichtbar wird es beim Owner gezeigt
        /// </summary>
        /// <param name="Owner">Owner, bei dem das Fenster initialisiert wird.</param>
        public void showConfigCenter(Window Owner)
        {
            if (cCenter == null)
            {
                cCenter = new ConfigCenter();
                cCenter.Owner = Owner;
                cCenter.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            cCenter.Show();
            cCenter.Focus();
        }

        /// <summary>
        /// Schließt das ConfigCenter
        /// </summary>
        public void closeConfigCenter()
        {
            cCenter = null;
        }

        private void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Die übergebene NotewView wird geschlossen.
        /// Objekte die auf Änderungen warten werden mit 'List' benachrichtigt.
        /// </summary>
        /// <param name="note"> die NoteView, die geschlossen werden soll</param>
        public void closeNote(NoteView note)
        {
            if (note.NoteModel.Config.EnumClose == EnumClose.DELETE)
            {
                this.removeNote(note.NoteModel);
            }
            else
            {
                listNote.Remove(note);
                note.Close();
                OnPropertyChanged("List");                                
            }

        }

        /// <summary>
        /// Zeigt das NoteModel auf der GUI
        /// </summary>
        /// <param name="noteModel">Das NoteModel das auf der GUI angezeigt werden soll</param>
        public void showNote(NoteModel noteModel)
        {
            NoteView note = new NoteView();
            note.NoteModel = noteModel;
            note.setConfigValues(noteModel.Config);
            listNote.Add(note);
            note.Show();
        }

        /// <summary>
        /// Schließt alle angezeigten Notizen
        /// </summary>
        public void closeAllNotes()
        {
            List<NoteView> list = new List<NoteView>(getDisplayedNotes());
            foreach (NoteView nv in list)
            {
                closeNote(nv);
            }
        }
    }
}
