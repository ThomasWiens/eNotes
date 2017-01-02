using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Configuration;
using eNotes.model;
using System.Windows.Media;
using System.Windows;

namespace eNotes.logik
{
    /// <summary>
    /// Klasse, die das Laden und Speichern von Objekten auf der Festplatte übernimmt
    /// </summary>
    public class Storage
    {
        private static string currentFile = @ConfigurationManager.AppSettings["locationCurrentFile"];
        private static string archiveFile = @ConfigurationManager.AppSettings["locationArchiveFile"];
        private static string elementNotecollection = "Notecollection";
        private static string elementConfig = "Config";
        private static string elementContent = "Content";
        private static string elementFont = "Font";
        private static string elementSize = "Size";
        private static string elementColor = "Color";
        private static string elementPosition = "Position";
        private static string elementPositionX = elementPosition + "X";
        private static string elementPositionY = elementPosition + "Y";
        private static string elementWidth = "Width";
        private static string elementHeight = "Height";
        private static string elementNote = "Note";
        private static string elementTimestamps = "Timestamps";
        private static string elementTimestamp = "Timestamp";
        private static string elementText = "Text";

        private static Config conf;

        /// <summary>
        /// Lädt eine Liste von NoteModel, die zuvor mir dem EnumClose.SAVE gespeichert worden sind. Wurde vorher die Liste nicht angelegt wird eine leere Liste returnt.
        /// </summary>
        /// <returns>Liste mit NoteModel</returns>
        public static List<NoteModel> getCurrentFile()
        {
            List<NoteModel> ret = new List<NoteModel>();
            try
            {
                XmlReader reader = XmlReader.Create(currentFile);
                getFile(ret, reader, EnumClose.SAVE);
                reader.Close();
            }
            catch (FileNotFoundException) { /* NOP - wird beim beenden einfach erstellt*/}
            
            return ret;
        }

        /// <summary>
        /// Lädt eine Liste von NoteModel, die zuvor mir dem EnumClose.ARCHIVE gespeichert worden sind. Wurde vorher die Liste nicht angelegt wird eine leere Liste returnt.
        /// </summary>
        /// <returns>Liste mit NoteModel</returns>
        public static List<NoteModel> getArchiveFile()
        {
            List<NoteModel> ret = new List<NoteModel>();
            try
            {
                XmlReader reader = XmlReader.Create(archiveFile);
                getFile(ret, reader, EnumClose.ARCHIVE);
                reader.Close();

            }
            catch (FileNotFoundException) {/* NOP - wird beim beenden einfach erstellt*/ }
            return ret;
        }

        private static void getFile(List<NoteModel> list, XmlReader reader, EnumClose enClose){
            try
            {
                NoteModel noteM = null;
                Config conf = null;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "Note":
                                    noteM = new NoteModel();
                                    conf = new Config();
                                    break;
                                case "Font":
                                    reader.Read();
                                    conf.FontFamily = new FontFamily(reader.Value);
                                    break;
                                case "Size":
                                    conf.FontSize = reader.ReadElementContentAsInt();
                                    break;
                                case "Color":
                                    reader.Read();
                                    string value = reader.Value;
                                    foreach (EnumColor enColor in EnumColor.Values)
                                    {
                                        if (enColor.Name == value)
                                        {
                                            conf.EnumColor = enColor;
                                            break;
                                        }
                                    }
                                    break;
                                case "Position":
                                    break;
                                case "PositionX":
                                    reader.Read();
                                    conf.LocationX = Convert.ToInt16(reader.Value);
                                    break;
                                case "PositionY":
                                    reader.Read();
                                    conf.LocationY = Convert.ToInt16(reader.Value);
                                    break;
                                case "Width":
                                    reader.Read();
                                    conf.Width = Convert.ToInt16(reader.Value);
                                    break;
                                case "Height":
                                    reader.Read();
                                    conf.Height = Convert.ToInt16(reader.Value);
                                    break;
                                case "Timestamp":
                                    DateTime time = reader.ReadElementContentAsDateTime();
                                    noteM.addTimestamp(time);
                                    break;
                                case "Text":
                                    reader.Read();
                                    if(reader.NodeType != XmlNodeType.Whitespace)
                                        noteM.Text = reader.Value;
                                    break;
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (reader.Name == "Note" && noteM != null)
                            {
                                conf.EnumClose = enClose;
                                noteM.Config = conf;
                                list.Add(noteM);
                            }
                            break;
                    }
                }
            }
            catch (XmlException) { /*NOP - wird beim beenden einfach erstellt */}
            
        }

        /// <summary>
        /// Speichert die Notizen zurück in die davor vorgesehene Dateien zurück
        /// </summary>
        /// <param name="list">Liste mit NoteModel</param>
        public static void writeNoteToFiles(List<NoteModel> list)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";

            XmlWriter writerCurrent = XmlWriter.Create(currentFile, settings);
            XmlWriter writerArchive = XmlWriter.Create(archiveFile, settings);
            writerCurrent.WriteStartDocument();
            writerArchive.WriteStartDocument();
            //Start-Tag des Stammelements
            writerCurrent.WriteStartElement(elementNotecollection);
            writerArchive.WriteStartElement(elementNotecollection);

            foreach (NoteModel elem in list)
            {
                if (elem.Config.EnumClose == EnumClose.SAVE)
                {
                    writePartToSpecificFile(writerCurrent, elem);
                }
                else if (elem.Config.EnumClose == EnumClose.ARCHIVE)
                {
                    writePartToSpecificFile(writerArchive, elem);
                }
            }

            //Ende-Tag des Stammelements
            writerCurrent.WriteEndElement();
            writerArchive.WriteEndElement();
            writerCurrent.WriteEndDocument();
            writerArchive.WriteEndDocument();
            writerCurrent.Close();
            writerArchive.Close();
        }

        private static void writePartToSpecificFile(XmlWriter writer, NoteModel elem)
        {
            writer.WriteStartElement(elementNote); //Start Note
            writer.WriteStartElement(elementConfig); //Start Config
            writer.WriteStartElement(elementFont); //Start Font
            writer.WriteValue(elem.Config.FontFamily.Source);
            writer.WriteEndElement(); // Ende Font
            writer.WriteStartElement(elementSize); //Start Size
            writer.WriteValue(elem.Config.FontSize);
            writer.WriteEndElement(); //Ende Size
            writer.WriteStartElement(elementColor); //Start Color
            writer.WriteValue(elem.Config.EnumColor.Name);
            writer.WriteEndElement(); //Ende Color
            writer.WriteStartElement(elementPosition); //Start Position
            writer.WriteStartElement(elementPositionX); //Start PositionX
            writer.WriteValue(elem.Config.LocationX);
            writer.WriteEndElement();// Ende PositonX
            writer.WriteStartElement(elementPositionY); // Start PositionY
            writer.WriteValue(elem.Config.LocationY);
            writer.WriteEndElement(); //Ende PositionY
            writer.WriteStartElement(elementWidth); //Start Width
            writer.WriteValue(elem.Config.Width);
            writer.WriteEndElement(); //Ende Width
            writer.WriteStartElement(elementHeight); //Start Height
            writer.WriteValue(elem.Config.Height);
            writer.WriteEndElement(); //Ende Height
            writer.WriteEndElement(); //Ende Position
            writer.WriteEndElement(); //Ende Config

            writer.WriteStartElement(elementContent);
            writer.WriteStartElement(elementTimestamps);
            foreach (DateTime time in elem.timestamps)
            {
                writer.WriteStartElement(elementTimestamp);
                writer.WriteValue(time);
                writer.WriteEndElement();//Ende Timestamp
            }
            writer.WriteEndElement(); //Ende Timestamps
            
            writer.WriteStartElement(elementText);
            writer.WriteValue(elem.Text);
            writer.WriteEndElement(); // Ende Text
            writer.WriteEndElement(); //Ende Content
            writer.WriteEndElement(); // Ende Note
        }

        /// <summary>
        /// Erstellt falls noch nicht vorhanden das Config Object
        /// </summary>
        /// <returns>Config</returns>
        public static Config getConfig()
        {
            if (conf == null)
            {
                initConfig();
            }
            return conf;
        }

        /// <summary>
        /// Speichert die Config in das User Verzeichnis
        /// </summary>
        public static void storeConfig(){
            eNotes.Properties.Settings.Default.Font = conf.FontFamily.Source;
            eNotes.Properties.Settings.Default.FontSize = conf.FontSize;
            eNotes.Properties.Settings.Default.DefaultColor = conf.EnumColor.Name;
            eNotes.Properties.Settings.Default.Timestamp = Config.ShowFirstCreation;
            eNotes.Properties.Settings.Default.Close = conf.EnumClose.Name;
            eNotes.Properties.Settings.Default.Save();
        }

        private static void initConfig(){
            conf = new Config();
            conf.FontFamily = new FontFamily(eNotes.Properties.Settings.Default.Font);
            conf.FontSize = eNotes.Properties.Settings.Default.FontSize;
            string temp = eNotes.Properties.Settings.Default.DefaultColor;
            foreach(EnumColor en in EnumColor.Values)
            {
                if (en.Name == temp)
                {
                    conf.EnumColor = en;
                    break;
                }
            }
            Config.ShowFirstCreation = eNotes.Properties.Settings.Default.Timestamp;
            temp = eNotes.Properties.Settings.Default.Close;
            foreach(EnumClose en in EnumClose.Values)
            {
                if(en.Name == temp){
                    conf.EnumClose = en;
                    break;
                }
            }
        }
    }
}
