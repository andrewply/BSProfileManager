using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace BSProfileManager
{
    class SettingHelper
    {
        private const string rootNode = "Setting";
        private const string pathNode = "BS_Path";

        public SettingHelper()
        {
            if (!File.Exists(Global.SETTING_FILE_PATH))
            {
                createSettingFile();
            }
        }

        public bool isSettingFileExist()
        {
            return File.Exists(Global.SETTING_FILE_PATH);
        }

        public void createSettingFile()
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlElement root = xmlDoc.CreateElement(rootNode);
            XmlElement bsPath = xmlDoc.CreateElement(pathNode);
            bsPath.InnerText = Global.DEFAULT_BS_FILE_PATH;

            xmlDoc.AppendChild(root);
            root.AppendChild(bsPath);

            xmlDoc.Save(Global.SETTING_FILE_PATH);
        }

        public void setBSFolderPath(string bsPath)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Global.SETTING_FILE_PATH);

            XmlNode node = xmldoc.DocumentElement.SelectSingleNode(@"//" + pathNode);

            node.InnerText = bsPath;

            xmldoc.Save(Global.SETTING_FILE_PATH);
        }

        public string getBSFolderPath()
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(Global.SETTING_FILE_PATH);

            XmlNode node = xmldoc.DocumentElement.SelectSingleNode(@"//" + pathNode);

            return node.InnerText;
        }

        public string getStartExePath()
        {
            return getBSFolderPath() + Global.BS_START;
        }

        public string getCloseExePath()
        {
            return getBSFolderPath() + Global.BS_QUIT;
        }
    }
}
