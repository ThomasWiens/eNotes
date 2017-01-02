using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Specialized;
using System.Windows.Media;
using eNotes.model;
using eNotes.logik;
using eNotes.view;

namespace eNotes
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Config einmal initialisieren
            Storage.getConfig();

            // dem Notemanager alle bisherigen Notzen bekannt machen
            NoteManager nm = NoteManager.getInstance();
            List<NoteModel> storagelist = Storage.getCurrentFile();
            nm.addAllNotes(storagelist);
            storagelist = Storage.getArchiveFile();
            nm.addAllNotes(storagelist);

            //Falls vorher keine Notes offen waren einen neuen erstelln
            List<NoteView> notes = nm.getDisplayedNotes();
            if (notes.Count == 0)
            {
                nm.newNote();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            NoteManager nm = NoteManager.getInstance();
            List<NoteModel> list = nm.getAllNotes();
            Storage.writeNoteToFiles(list);
        }

    }
}
