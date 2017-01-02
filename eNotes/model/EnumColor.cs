using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotes.model
{
    /// <summary>
    /// Pseudo-Enum: Listet alle Farben auf.
    /// </summary>
    public class EnumColor
    {
        public static readonly EnumColor GELB = new EnumColor("Gelb", "#FFFF85", "#FFFFBB");
        public static readonly EnumColor GRUEN = new EnumColor("Grün", "#B3E9AF", "#CCFAC6");
        public static readonly EnumColor BLAU = new EnumColor("Blau", "#BBDDF4", "#D3EEF9");
        public static readonly EnumColor WEISS = new EnumColor("Weiß", "#EBEBEB", "#FDFDFD");
        public static readonly EnumColor ROSA = new EnumColor("Rosa", "#F0BFF0", "#F4CDF4");

        /// <summary>
        /// Holt eine Auflistung aller eingetragenen Enum-Objekte
        /// </summary>
        public static IEnumerable<EnumColor> Values
        {
            get
            {
                yield return GELB;
                yield return GRUEN;
                yield return BLAU;
                yield return WEISS;
                yield return ROSA;
            }
        }

        private readonly string name;
        private readonly string gradient1;
        private readonly string gradient2;

        EnumColor(string name, string gradient1, string gradient2)
        {
            this.name = name;
            this.gradient1 = gradient1;
            this.gradient2 = gradient2;
        }

        /// <summary>
        /// Holt den Namen des Objekts
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Holt die 'von Farbe' für den Farbverlauf
        /// </summary>
        public string Gradient1 { get { return gradient1; } }

        /// <summary>
        /// Holt die 'bis Farbe' für den Farbverlauf
        /// </summary>
        public string Gradient2 { get { return gradient2; } }
    }
}
