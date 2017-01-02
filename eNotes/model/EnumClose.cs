using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotes.model
{
    /// <summary>
    /// Pseudo-Enum: Listet alle Schließverhalten auf.
    /// </summary>
    public class EnumClose
    {
        public static readonly EnumClose SAVE = new EnumClose("save");
        public static readonly EnumClose DELETE = new EnumClose("delete");
        public static readonly EnumClose ARCHIVE = new EnumClose("archive");

        /// <summary>
        /// Holt eine Auflistung aller eingetragenen Enum-Objekte
        /// </summary>
        public static IEnumerable<EnumClose> Values
        {
            get
            {
                yield return SAVE;
                yield return DELETE;
                yield return ARCHIVE;
            }
        }

        private readonly string name;

        EnumClose(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Holt den Namen des Objekts
        /// </summary>
        public string Name { get { return name; } }
    }
}
