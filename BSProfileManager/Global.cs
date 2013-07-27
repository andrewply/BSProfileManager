using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSProfileManager
{
    static class Global
    {
        public static string DEFAULT_PROGRAM_FILE_PATH = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles) + "\\";
        public static string DEFAULT_BS_FILE_PATH = DEFAULT_PROGRAM_FILE_PATH + @"BlueStacks\";
        public static string CURRENT_FOLDER_PATH = System.Environment.CurrentDirectory + "\\";
        public static string SETTING_FILE_PATH = CURRENT_FOLDER_PATH + "setting.xml";
        public static string LOG_FILE_PATH = CURRENT_FOLDER_PATH + "log.txt";
        public static string DB_FILE_PATH = CURRENT_FOLDER_PATH + "datastore.db";
        
        public static string BS_QUIT = @"HD-Quit.exe";
        public static string BS_START = @"HD-StartLauncher.exe";
        public static string REG_KEYNAME = "BootParameters";
        public static string REG_BS_PATH = @"SOFTWARE\\BlueStacks\\Guests\\Android";
        public static string GUID_REG_EX = @"([a-z0-9]*[-]){4}[a-z0-9]*";
    }
}
