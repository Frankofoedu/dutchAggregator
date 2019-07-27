using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Classes
{
    public static class Jobs
    {

        /// <summary>
        /// Loads file to XML
        /// </summary>
        /// <param name="FileName">File path of the XML file</param>
        /// <returns>Bet9ja object from XML file</returns>
        public static List<T> LoadFromXML<T>(string FileName)
        {
            using (var stream = System.IO.File.OpenRead(FileName))
            {
                var obj = new List<T>();
                var xmlSerializer = new XmlSerializer(obj.GetType());
                obj = xmlSerializer.Deserialize(stream) as List<T>;
                return obj;
            }
        }

        /// <summary>
        /// Saves file to XML
        /// </summary>
        /// <param name="obj">Object to be saved</param>
        /// <param name="FileName">File path of the new XML file</param>
        ///// <returns>Confirmation that file was saved.</returns>
        public static string SaveToXML<T>(List<T> obj, string FileName)
        {
            try
            {
                using (var writer = new System.IO.StreamWriter(FileName))
                {
                    var xmlSerializer = new XmlSerializer(obj.GetType());
                    xmlSerializer.Serialize(writer, obj);
                    writer.Flush();
                }
                return "Object saved to XML file.";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

    }
}
