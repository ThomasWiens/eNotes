using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eNotes.view;

namespace eNotes.logik
{
    /// <summary>
    /// Klasse zum Vergleichen von NotePreview
    /// </summary>
    public class NotePreviewComparison : IComparer<NotePreview>
    {
        /// <summary>
        /// Setzt oder holt das Flag, ob nach Erstelldatum (true) oder letzer Änderung (false) vergleicht werden soll.
        /// </summary>
        public bool First { get; set; }

        /// <summary>
        /// Vergleicht die übergebenen entweder nach Erstelldatum oder Letzte Änderung
        /// </summary>
        /// <param name="a">erste NotePreview</param>
        /// <param name="b">zweite NotePreview</param>
        /// <returns>int</returns>
        public int Compare(NotePreview a, NotePreview b)
        {
            if (First == true)
            {
                return DateTime.Compare(a.NoteModel.Creation, b.NoteModel.Creation);
            }
            else
            {
                return DateTime.Compare(a.NoteModel.LastChange, b.NoteModel.LastChange);
            }
        }
    }
}
