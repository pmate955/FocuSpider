using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml;

namespace FocuSpider
{
    internal class Config
    {
        /// <summary>
        /// If true, the window stays on top
        /// </summary>
        public static bool IsAlwaysOnTop { get; set; } = false;

        /// <summary>
        /// Steps count setting
        /// </summary>
        public static int SliderValue { get; set; } = 800;

        /// <summary>
        /// How many pictures will be taken?
        /// </summary>
        public static int PictureCount { get; set; } = 10;

        /// <summary>
        /// How many seconds will wait between pictures?
        /// </summary>
        public static int TimeBetweenPictures { get; set; } = 3;

        /// <summary>
        /// The preset settings
        /// </summary>
        public static List<Preset> Presets { get; set; } = new List<Preset>()
        {
            new Preset() { Name = "4x", Steps = 6400 },
            new Preset() { Name = "10x", Steps = 3200 },
            new Preset() { Name = "20x", Steps = 1600 },
            new Preset() { Name = "40x", Steps = 800 }
        };

        /// <summary>
        /// Initialize config from xml file
        /// </summary>
        public static void Init()
        {
            try
            {
                if (!File.Exists("FocuSpiderConfig.xml"))
                {
                    File.Copy("Config.xml", "FocuSpiderConfig.xml");
                }

                XmlDocument doc = new XmlDocument();
                doc.Load("FocuSpiderConfig.xml");

                XmlNode alwaysOnTopNode = doc.DocumentElement.SelectSingleNode("/settings/IsAlwaysOnTop");
                if (alwaysOnTopNode != null) IsAlwaysOnTop = Convert.ToBoolean(alwaysOnTopNode.InnerText);

                XmlNode sliderValueNode = doc.DocumentElement.SelectSingleNode("/settings/SliderValue");
                if (sliderValueNode != null) SliderValue = Convert.ToInt32(sliderValueNode.InnerText);

                XmlNode pictureCountNode = doc.DocumentElement.SelectSingleNode("/settings/PictureCount");
                if (pictureCountNode != null) PictureCount = Convert.ToInt32(pictureCountNode.InnerText);

                XmlNode timeBetweenPicturesNode = doc.DocumentElement.SelectSingleNode("/settings/TimeBetweenPictures");
                if (timeBetweenPicturesNode != null) TimeBetweenPictures = Convert.ToInt32(timeBetweenPicturesNode.InnerText);

                XmlNode presetsNode = doc.DocumentElement.SelectSingleNode("/settings/Presets");

                if (presetsNode != null)
                {
                    Presets = new List<Preset>();
                    var presetNodes = presetsNode.SelectNodes("Preset");

                    foreach (XmlNode node in presetNodes)
                    {
                        string name = node.SelectSingleNode("Name").InnerText;
                        int steps = Convert.ToInt32(node.SelectSingleNode("Steps").InnerText);
                        Preset p = new Preset() { Name = name, Steps = steps };
                        Presets.Add(p);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.ERROR, ex.ToString());
            }
        }

        /// <summary>
        /// Save xml config
        /// </summary>
        public static void SaveConfig()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("FocuSpiderConfig.xml");

                XmlNode alwaysOnTopNode = doc.DocumentElement.SelectSingleNode("/settings/IsAlwaysOnTop");
                if (alwaysOnTopNode != null) alwaysOnTopNode.InnerText = IsAlwaysOnTop.ToString();

                XmlNode sliderValueNode = doc.DocumentElement.SelectSingleNode("/settings/SliderValue");
                if (sliderValueNode != null) sliderValueNode.InnerText = SliderValue.ToString();

                XmlNode pictureCountNode = doc.DocumentElement.SelectSingleNode("/settings/PictureCount");
                if (pictureCountNode != null) pictureCountNode.InnerText = PictureCount.ToString();

                XmlNode timeBetweenPicturesNode = doc.DocumentElement.SelectSingleNode("/settings/TimeBetweenPictures");
                if (timeBetweenPicturesNode != null) timeBetweenPicturesNode.InnerText = TimeBetweenPictures.ToString();

                XmlNode presetsNode = doc.DocumentElement.SelectSingleNode("/settings/Presets");

                if (presetsNode != null)
                {
                    var presetNodes = presetsNode.SelectNodes("Preset");

                    int ctr = 0;

                    foreach (XmlNode node in presetNodes)
                    {
                        node.SelectSingleNode("Name").InnerText = Presets[ctr].Name;
                        node.SelectSingleNode("Steps").InnerText = Presets[ctr].Steps.ToString();
                        ctr++;
                    }
                }

                doc.Save("FocuSpiderConfig.xml");
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.ERROR, ex.ToString());
            }
        }
    }

}
