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
using eNotes.view;
using eNotes.model;
using eNotes.logik;
using System.ComponentModel;

namespace eNotes.view
{
    /// <summary>
    /// Interaktionslogik für Archive.xaml
    /// </summary>
    public partial class Archive : UserControl
    {
        private List<NotePreview> notePreviews;
        private EnumColor filter;

        /// <summary>
        /// Konstruktor: initialisert die Werte
        /// </summary>
        public Archive()
        {
            this.notePreviews = new List<NotePreview>();
            InitializeComponent();
            NoteManager.getInstance().PropertyChanged += new PropertyChangedEventHandler(archiveContent_Initialized);
        }

        /// <summary>
        /// Wenn die Pinnwand geladen werden soll, wird diese Methode ausgeführt. Jegliche Anzeige zuvor wird gelöscht und neu aufgebaut
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void archiveContent_Initialized(object sender, EventArgs e)
        {
            notePreviews.Clear();
            List<NoteModel> list = NoteManager.getInstance().getAllNotes();
            foreach (NoteModel nm in list)
            {
                NotePreview np = new NotePreview();
                np.NoteModel = nm;
                notePreviews.Add(np);
            }
            this.Sort_SelectionChanged(null, null);
            this.updateArchiveContent();
        }

        private void updateArchiveContent()
        {
            if (archiveContent != null)
            {
                archiveContent.Children.Clear();
                foreach (NotePreview np in notePreviews)
                {
                    if (filter == null || np.NoteModel.Config.EnumColor == filter)
                    {
                        archiveContent.Children.Add(np);
                    }

                    if (np.NoteModel.Config.EnumClose == EnumClose.SAVE)
                    {
                        np.IsEnabled = true;
                        foreach (NoteView nv in NoteManager.getInstance().getDisplayedNotes()) // Wenn doch nicht angezeigt wieder enabled
                        {
                            if (nv.NoteModel == np.NoteModel)
                            {
                                np.IsEnabled = false;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void Wiederherstellen_Click(object sender, RoutedEventArgs e)
        {
            foreach (NotePreview np in notePreviews)
            {
                if (np.IsSelected)
                {
                    np.NoteModel.Config.EnumClose = EnumClose.SAVE;
                    np.IsSelected = false;
                    NoteManager.getInstance().showNote(np.NoteModel);
                }
            }
            updateArchiveContent();
        }

        private void Loeschen_Click(object sender, RoutedEventArgs e)
        {
            List<NotePreview> temp = new List<NotePreview>();
            foreach (NotePreview np in notePreviews)
            {
                if (np.IsSelected)
                {
                    np.NoteModel.Config.EnumClose = EnumClose.DELETE;
                    temp.Add(np);
                }
            }
            foreach (NotePreview np in temp) // Hier werden die NotePreviews erst entfernt -> in der ersten Schleife gabs eine Exception
            {
                notePreviews.Remove(np);
                NoteManager nm = NoteManager.getInstance();
                nm.removeNote(np.NoteModel);
            }
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string color = (sender as ComboBox).SelectedItem.ToString();
            this.filter = null;
            foreach (EnumColor enColor in EnumColor.Values)
            {
                if (enColor.Name == color)
                {
                    this.filter = enColor;
                    break;
                }
            }
            updateArchiveContent();

        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int sort = cmbSort.SelectedIndex;
            NotePreviewComparison npc = new NotePreviewComparison();
            switch (sort)
            {
                case 0:
                    npc.First = true;
                    notePreviews.Sort(npc);
                    break;
                case 1:
                    npc.First = true;
                    notePreviews.Sort(npc);
                    notePreviews.Reverse();
                    break;
                case 2:
                    npc.First = false;
                    notePreviews.Sort(npc);
                    break;
                case 3:
                    npc.First = false;
                    notePreviews.Sort(npc);
                    notePreviews.Reverse();
                    break;
            }
            updateArchiveContent();

        }

        private void cmbFilter_Initialized(object sender, EventArgs e)
        {
            IEnumerable<EnumColor> values = EnumColor.Values;
            foreach (EnumColor color in values)
            {
                (sender as ComboBox).Items.Add(color.Name);
            }
        }
    }

}
