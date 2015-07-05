using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TruckEntryList
{
    public static class PresenterSettings
    {
        public static float hourZone { get { return (float)Properties.Settings.Default["hourZone"]; } set { Properties.Settings.Default["hourZone"] = value; Save(); } }
        public static float nrAutoZone { get { return (float)Properties.Settings.Default["nrAutoZone"]; } set { Properties.Settings.Default["nrAutoZone"] = value; Save(); } }
        public static float payloadZone { get { return (float)Properties.Settings.Default["payloadZone"]; } set { Properties.Settings.Default["payloadZone"] = value; Save(); } }
        public static float nextTrucksZone { get { return (float)Properties.Settings.Default["nextTrucksZone"]; } set { Properties.Settings.Default["nextTrucksZone"] = value; Save(); } }

        static PresenterSettings()
        {
            string settingsPath = (string)Properties.Settings.Default["settingsFilePath"];
            if (!File.Exists(settingsPath))
            {
                XmlDocument xmlSettings = new XmlDocument();

                var root = (XmlElement)xmlSettings.AppendChild(xmlSettings.CreateElement("AppSettings"));
                var el = (XmlElement)root.AppendChild(xmlSettings.CreateElement("setting"));
                el.SetAttribute("Name", "hourZone");
                el.SetAttribute("Value", 0.2.ToString());

                el = (XmlElement)root.AppendChild(xmlSettings.CreateElement("setting"));
                el.SetAttribute("Name", "nrAutoZone");
                el.SetAttribute("Value", 0.45.ToString());

                el = (XmlElement)root.AppendChild(xmlSettings.CreateElement("setting"));
                el.SetAttribute("Name", "payloadZone");
                el.SetAttribute("Value", 0.25.ToString());

                el = (XmlElement)root.AppendChild(xmlSettings.CreateElement("setting"));
                el.SetAttribute("Name", "nextTrucksZone");
                el.SetAttribute("Value", 0.1.ToString());

                xmlSettings.Save(settingsPath);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(settingsPath);
            var rootE = (XmlElement)doc.GetElementsByTagName("AppSettings")[0];
            foreach (XmlElement el in rootE.GetElementsByTagName("setting"))
            {
                decimal f;
                if (decimal.TryParse(el.GetAttribute("Value").Replace('.', NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator[0]).Replace(',', NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator[0]), out f))
                    Properties.Settings.Default[el.GetAttribute("Name")] = (float)f;
                else { Properties.Settings.Default[el.GetAttribute("Name")] = 0.0f; }
            }
        }

        private static void Save()
        {
            string settingsPath = (string)Properties.Settings.Default["settingsFilePath"];
            XmlDocument xmlSettings = new XmlDocument();

            XmlElement root = (XmlElement)xmlSettings.AppendChild(xmlSettings.CreateElement("AppSettings"));
            var el = (XmlElement)root.AppendChild(xmlSettings.CreateElement("setting"));
            el.SetAttribute("Name", "hourZone");
            el.SetAttribute("Value", hourZone.ToString());

            el = (XmlElement)root.AppendChild(xmlSettings.CreateElement("setting"));
            el.SetAttribute("Name", "nrAutoZone");
            el.SetAttribute("Value", nrAutoZone.ToString());

            el = (XmlElement)root.AppendChild(xmlSettings.CreateElement("setting"));
            el.SetAttribute("Name", "payloadZone");
            el.SetAttribute("Value", payloadZone.ToString());

            el = (XmlElement)root.AppendChild(xmlSettings.CreateElement("setting"));
            el.SetAttribute("Name", "nextTrucksZone");
            el.SetAttribute("Value", nextTrucksZone.ToString());

            xmlSettings.Save(settingsPath);
        }
    }
}
